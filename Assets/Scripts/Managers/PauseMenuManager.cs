using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    private static PauseMenuManager instance;
    public static PauseMenuManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private List<GameObject> tabButtonList;

    private Dictionary<string, ImageUtil> tabButtonDict = new Dictionary<string, ImageUtil>();
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
        foreach (GameObject tab in tabButtonList)
        {
            tabButtonDict.Add(tab.name, tab.GetComponent<ImageUtil>());
        }

        UIManager.doStayMain = true;
        GameManager.PauseGame();
        UIManager.Instance.SwitchUI("Settings UI");
        AudioManager.PlaySound("Pause Menu Open Sound");

        EventMessenger.TriggerEvent("UpdateVolumeSliders");

        //debug
    }
    private void OnEnable()
    {
        EventMessenger.StartListening("CloseMenu", ClosePauseMenu);
        EventMessenger.StartListening("UISwitched", UpdateUI);
        EventMessenger.StartListening("UpdateSwitchBodyText", UpdateSwitchBodyText);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("CloseMenu", ClosePauseMenu);
        EventMessenger.StopListening("UISwitched", UpdateUI);
        EventMessenger.StopListening("UpdateSwitchBodyText", UpdateSwitchBodyText);
    }
    public void ClosePauseMenu()
    {
        GameManager.UnpauseGame();
    }
    public void UpdateSwitchBodyText()
    {
        StartCoroutine(SwitchBodyText(PrimitiveMessenger.GetObject("playerBody")));
    }
    private IEnumerator SwitchBodyText(string body)
    {
        yield return new WaitForEndOfFrame();
        PrimitiveMessenger.EditObject("bodySwitchText", "Current Body: " + body);
        EventMessenger.TriggerEvent("UpdateBodySwitchText");
    }
    private void UpdateUI()
    {
        foreach (ImageUtil tabButton in tabButtonDict.Values)
        {
            tabButton.canChangeOpacity = true;
            tabButton.SetOpacity(0.5f);
        }
        tabButtonDict[UIManager.Instance.currentUI + " Tab Button"].SetOpacity(1);
        tabButtonDict[UIManager.Instance.currentUI + " Tab Button"].canChangeOpacity = false;
    }
}
