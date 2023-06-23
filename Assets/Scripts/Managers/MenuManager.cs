using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void OnEnable()
    {
        EventMessenger.StartListening("CloseMenu", CloseScene);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("CloseMenu", CloseScene);
    }
    private void CloseScene()
    {
        GameManager.OtherMenuClosed();
        SceneManager.UnloadSceneAsync(sceneName);
        UIManager.doStayMain = false;
    }
}
