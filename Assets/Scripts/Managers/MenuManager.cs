using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public enum UI
    {
        None,
        Main, 
        Settings, 
        Controls,
        BodySwitch,
        ExitGame,
    }

    private static MenuManager instance;
    public static MenuManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private List<GameObject> uiList;
    [SerializeField] private List<GameObject> tabButtonList;

    public UI currentUI;

    [SerializeField] public GameObject pausedUI;

    private Dictionary<string, GameObject> uiDict = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> tabButtonDict = new Dictionary<string, GameObject>();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        foreach (GameObject ui in uiList)
        {
            uiDict.Add(ui.name, ui);
        }
        foreach (GameObject tab in tabButtonList)
        {
            tabButtonDict.Add(tab.name, tab);
        }

        currentUI = UI.None;

        //debug
    }

    void Update()
    {
        /*if (Input.GetButtonDown("Cancel"))
        {
            if (GameManager.isGameActive && !GameManager.Instance.isInTransition && !ShopManager.Instance.isShopOpen)
            {
                OpenPauseMenu();
            }
            else
            {
                ClosePauseMenu();
            }
        }*/
    }
    public void ClearUI()
    {
        foreach (GameObject ui in uiList)
        {
            ui.SetActive(false);
        }
        foreach (GameObject tab in tabButtonList)
        {
            tab.GetComponent<UIButton>().canChangeOpacity = true;
            tab.GetComponent<UIButton>().SetOpacity(0.5f);
        }
    }
    public void OpenPauseMenu()
    {
        GameManager.Instance.PauseGame();
        pausedUI.SetActive(true);
        OpenSettings();
        AudioManager.PlaySound("Menu Open Sound");
    }
    public void ClosePauseMenu()
    {
        GameManager.Instance.UnpauseGame();
        pausedUI.SetActive(false);
    }
    public void OpenSettings()
    {
        ClearUI();
        currentUI = UI.Settings;
        UpdateUI();
    }
    public void OpenControls()
    {
        ClearUI();
        currentUI = UI.Controls;
        UpdateUI();
    }
    public void OpenBodySwitch()
    {
        ClearUI();
        currentUI = UI.BodySwitch;
        UpdateUI();
    }
    public void OpenExitGame()
    {
        ClearUI();
        currentUI = UI.ExitGame;
        UpdateUI();
    }
    public void UpdateSwitchBodyText(string body)
    {
        StartCoroutine(SwitchBodyText(body));
    }
    private IEnumerator SwitchBodyText(string body)
    {
        yield return new WaitForEndOfFrame();
        uiDict["BodySwitch UI"].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Current Body: " + body;
    }
    private void UpdateUI()
    {
        uiDict[currentUI.ToString() + " UI"].SetActive(true);
        tabButtonDict[currentUI.ToString() + " Tab Button"].GetComponent<UIButton>().SetOpacity(1);
        tabButtonDict[currentUI.ToString() + " Tab Button"].GetComponent<UIButton>().canChangeOpacity = false;
    }
}
