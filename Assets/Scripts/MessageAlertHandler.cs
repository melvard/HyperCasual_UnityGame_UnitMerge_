using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageAlertHandler : Singleton<MessageAlertHandler>
{
    [SerializeField]
    private Text messageText;
    [SerializeField]
    private Button acceptButton;
    [Tooltip("UI panel blocking click event, when waiting for user interaction with message dialog (like accept button).")]
    [SerializeField] private GameObject blockingPanel;


    private void Start()
    {
        gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        blockingPanel.SetActive(false);
    }

    /// <summary>
    /// <param name="message">Mesage that will displayed in the dialog window</param>
    /// <param name="messageTimeInSeconds"> Time in second after which the window will close.</param>
    /// </summary>

    public void DisplayTimedTextMessage(string message, float messageTimeInSeconds)
    {
        gameObject.SetActive(true);
        messageText.text = message;
        StopAllCoroutines();//stop all coroutines as the previous timed message can be active and affect of correct implementation
        StartCoroutine(ShowTimedMessage(messageTimeInSeconds));
    }


    public IEnumerator DisplayMessageWithAcceptButton(string message, Action onAcceptAction)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        acceptButton.gameObject.SetActive(true);
        blockingPanel.SetActive(true);
        messageText.text = message;

        // wait for a callback and continue when button is presed
        var waitForUIButtons = new WaitForUIButtons(acceptButton);
        yield return waitForUIButtons.Reset();

        if (waitForUIButtons.PressedButton == acceptButton)
        {
            onAcceptAction.Invoke(); // invoke the action calling the expected methods after accepting
            gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(false);
            blockingPanel.SetActive(false);
        }
    }

    private IEnumerator ShowTimedMessage(float time)
    {
        float _time = 0;
        while(_time <= time)
        {
            _time += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
