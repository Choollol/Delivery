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
        AsyncOperation loadOverworld = SceneManager.LoadSceneAsync("Overworld", LoadSceneMode.Additive);
        while (loadOverworld.progress < 1f)
        {
            yield return null;
        }
        while (!SceneManager.GetSceneByName("Overworld").isLoaded)
        {
            yield return null;
        }
        SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Start_Game");
        yield break;
    }
}
