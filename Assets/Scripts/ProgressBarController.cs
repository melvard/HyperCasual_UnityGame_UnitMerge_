using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : Singleton<ProgressBarController>
{
    #region PRIVATE_FIELDS

    [SerializeField] private Text levelText;
    [SerializeField] [Tooltip("Lvl 1 goal scores to be reached")] private float StartGoal = 100;
    [SerializeField] [Tooltip("How much will the level score goal be increased")] private float LevelUpGoalMultiplier = 2f;

    private float deltaScore; // the score that remains after LevelUp, we keep this one to add after congratulation dialog
    private Slider progressBar;

    #endregion

    public int CurrenLevel { get; private set; } = 1; //the level player reached

    private void Start()
    {
        progressBar = GetComponent<Slider>();
        SetLevelText();
    }
    public void AddScore(float score)
    {

        if (progressBar.value + score >= progressBar.maxValue) //if newly added scores cross the Rubicon ))
        {
            //all the stuff that must be implemented after levelUp
            CurrenLevel++;
            deltaScore = score - (progressBar.maxValue - progressBar.value);
            progressBar.value = progressBar.maxValue;
            SetLevelText();
            StartCoroutine(MessageAlertHandler.Instance.DisplayMessageWithAcceptButton($"Congrats, you reached level {CurrenLevel}", () => ResetProgressBar()));
        }
        else
        {
            progressBar.value += score;
        }
    }

    /// <summary>
    /// This funtion will be passed as an action, so that it will wait for a callback from congratualtion dialog accept button.
    /// </summary>
    ///

#region PROGRESS_BAR_METHODS

    public void ResetProgressBar()
    {
        progressBar.value = 0 + deltaScore;
        SetMaxValue();
    }

    public float GetProgressbarValue()
    {
        return progressBar.value;
    }
    public void SetMaxValue()
    {
        progressBar.maxValue = StartGoal * Mathf.Pow(LevelUpGoalMultiplier, CurrenLevel - 1);
    }

    public void SetLevelText()
    {
        levelText.text = $"Level {CurrenLevel}";
    }
    public float GetProgressbarMaxValue()
    {
        return progressBar.maxValue;
    }

    #endregion

    /// <summary>
    /// The function will get player progress data. Method will be called from the GameManager if any data is found.
    /// </summary>
    /// <param name="pd"> The PlayerData loaded from storage, that contains player progress info</param>
    ///

    public void LoadInfoFromPlayerData(PlayerData pd)
    {
        var level = pd.Level;
        CurrenLevel = level;
        this.SetMaxValue();
        this.SetLevelText();
        this.AddScore(pd.ScoresReached);
    }
}

