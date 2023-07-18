using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventMessenger.StartListening("CloseMenu", CloseScene);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("CloseMenu", CloseScene);
    }
    private void Awake()
    {
        GameManager.OtherMenuOpened();
    }
    private void CloseScene()
    {
        GameManager.OtherMenuClosed();
        SceneManager.UnloadSceneAsync(gameObject.scene);
        UIManager.doStayMain = false;
    }
}
