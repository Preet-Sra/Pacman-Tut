using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string LevelName;
    public Image bar;

    public void PlayGame()
    {
        StartCoroutine(Loadlevel());
    }

    IEnumerator Loadlevel()
    {
        AsyncOperation loadoperation = SceneManager.LoadSceneAsync(LevelName);

        while (!loadoperation.isDone)
        {
            float progress = Mathf.Clamp01(loadoperation.progress / 0.9f);
            bar.fillAmount = progress;
            yield return new  WaitForEndOfFrame();
        }
    }
}
