using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GameStart());
    }
    private IEnumerator GameStart()
    {
        AsyncOperation loadMain = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        while (loadMain.progress < 0.9f)
        {
            yield return null;
        }
        loadMain.allowSceneActivation = false;
        AsyncOperation loadOverworld = SceneManager.LoadSceneAsync("Overworld", LoadSceneMode.Additive);
        while (loadOverworld.progress < 0.9f)
        {
            yield return null;
        }
        loadMain.allowSceneActivation = true;
        while (!loadMain.isDone || !loadOverworld.isDone)
        {
            yield return null;
        }
        EventMessenger.TriggerEvent("GameLoaded");
        SceneManager.UnloadSceneAsync("Start_Game");
        yield break;
    }
}
