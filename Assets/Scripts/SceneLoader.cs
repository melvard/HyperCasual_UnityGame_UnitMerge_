using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider loadBar;
    [SerializeField] private int sceneIndex = 1 ;
    


    private void Start()
    {
        if (loadBar != null )
        {
            loadBar.gameObject.SetActive(false);
            loadBar.value = 0;
        }
    }


    public void LoadScene()
    {
        if (loadBar != null)
        {
            StartCoroutine(LoadSceneAsync(sceneIndex));
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
            if(sceneIndex == 0)
            {
                GameManager.Instance.OnApplicationQuit();
            }
        }
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        var operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        loadBar.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            loadBar.value = operation.progress;
            yield return null;
        }
    }
}
