using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Area
    {
        Overworld, Farm, Market, Suburbs, City
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public static bool isGameActive { get; private set; }

    public static bool doContinueTransition;
    public static bool isInTransition { get; private set; }

    public static bool isInWorld = true;

    public static bool isPlayerInVehicle;
    public static string vehicleName;

    public static bool isMenuOpen;
    public static bool isMenuOnMain;
    public static string menuName;
    public static Area currentArea { get; private set; }

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
    private void OnEnable()
    {
        EventMessenger.StartListening("ExitGame", ExitGame);

        EventMessenger.StartListening("StartTransition", StartTransition);

        EventMessenger.StartListening("OpenRestaurant", OpenRestaurant);
        EventMessenger.StartListening("OpenVehicleShop", OpenVehicleShop);
        EventMessenger.StartListening("OpenGasShop", OpenGasShop);

        EventMessenger.StartListening("OpenCoinfall", OpenCoinfall);

        PrimitiveMessenger.AddObject("CoinfallBaseAmount", 0);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("ExitGame", ExitGame);

        EventMessenger.StopListening("StartTransition", StartTransition);

        EventMessenger.StopListening("OpenRestaurant", OpenRestaurant);
        EventMessenger.StopListening("OpenVehicleShop", OpenVehicleShop);
        EventMessenger.StopListening("OpenGasShop", OpenGasShop);

        EventMessenger.StopListening("OpenCoinfall", OpenCoinfall);

        PrimitiveMessenger.RemoveObject("CoinfallBaseAmount");
    }
    private void Start()
    {
        currentArea = Area.Overworld;

        BoundsManager.SetBounds(-8, 8, -5.76f, 5.76f);

        isGameActive = true;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isMenuOpen)
            {
                if (isMenuOnMain)
                {
                    EventMessenger.TriggerEvent("CloseMenu");
                }
                else
                {
                    EventMessenger.TriggerEvent("SwitchMenuToMain");
                }
            }
            else if (isGameActive && !isInTransition)
            {
                SceneManager.LoadSceneAsync("Pause_Menu", LoadSceneMode.Additive);
            }
        }
    }
    private void OpenCoinfall()
    {
        OpenSceneWithTransition("Coinfall", "");
    }
    public static void OtherMenuOpened()
    {
        isMenuOpen = true;
        isMenuOnMain = true;
        if (isInWorld)
        {
            EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        }
    }
    public static void OtherMenuClosed()
    {
        isMenuOpen = false;
        if (isInWorld)
        {
            EventMessenger.TriggerEvent("SetPlayerCanActTrue");
        }
    }
    private void OpenRestaurant()
    {
        SceneManager.LoadSceneAsync("Restaurant", LoadSceneMode.Additive);
    }
    private void OpenVehicleShop()
    {
        SceneManager.LoadSceneAsync("Vehicle_Shop", LoadSceneMode.Additive);
        AudioManager.PlaySound("Shop Chime");
    }
    private void OpenGasShop()
    {
        SceneManager.LoadSceneAsync("Gas_Shop", LoadSceneMode.Additive);
    }
    public void OpenSceneWithTransition(string sceneName, string transitionEventName)
    {
        StartCoroutine(HandleOpenSceneWithTransition(sceneName, transitionEventName));
    }
    private IEnumerator HandleOpenSceneWithTransition(string sceneName, string transitionEventName)
    {
        StartTransition();
        doContinueTransition = false;
        while (!doContinueTransition)
        {
            yield return null;
        }
        if (transitionEventName != "")
        {
            EventMessenger.TriggerEvent(transitionEventName);
        }
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield break;
    }
    public void CloseSceneWithTransition(string sceneName, string transitionEventName)
    {
        StartCoroutine(HandleCloseSceneWithTransition(sceneName, transitionEventName));
    }
    private IEnumerator HandleCloseSceneWithTransition(string sceneName, string transitionEventName)
    {
        StartTransition();
        doContinueTransition = false;
        while (!doContinueTransition)
        {
            yield return null;
        }
        if (transitionEventName != "")
        {
            EventMessenger.TriggerEvent(transitionEventName);
        }
        SceneManager.UnloadSceneAsync(sceneName);
        yield break;
    }
    public void SwitchArea(Area newArea)
    {
        StartCoroutine(HandleSwitchArea(newArea));
    }
    private IEnumerator HandleSwitchArea(Area newArea)
    {
        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        POIManager.ID = 0;
        StartTransition();
        GameObject currentPlayer;
        if (isPlayerInVehicle)
        {
            currentPlayer = GameObject.Find(vehicleName);
        }
        else
        {
            currentPlayer = GameObject.Find("Player");
        }
        currentPlayer.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        doContinueTransition = false;
        while (!doContinueTransition)
        {
            yield return null;
        }
        AsyncOperation loadNewArea = SceneManager.LoadSceneAsync(newArea.ToString(), LoadSceneMode.Additive);
        AsyncOperation unloadOldArea = SceneManager.UnloadSceneAsync(currentArea.ToString());
        while (!loadNewArea.isDone || !unloadOldArea.isDone)
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        switch (newArea)
        {
            case Area.Overworld:
                {
                    BoundsManager.SetBounds(-8, 8, -5.76f, 5.76f);
                    switch (currentArea)
                    {
                        case Area.Market:
                            {
                                currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[3], currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.Farm:
                            {
                                currentPlayer.transform.position = new Vector3(BoundsManager.GetBounds()[1], 0, currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.Suburbs:
                            {
                                currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[2], currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.City:
                            {
                                currentPlayer.transform.position = new Vector3(BoundsManager.GetBounds()[0], 0, currentPlayer.transform.position.z);
                                break;
                            }
                    }
                    break;
                }
            case Area.Market:
                {
                    BoundsManager.SetBounds(-1.92f, 1.92f, -2.4f, 2.4f);
                    currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[2], currentPlayer.transform.position.z);
                    break;
                }
            case Area.Farm:
                {
                    BoundsManager.SetBounds(-3.84f, 3.84f, -1.6f, 1.6f);
                    currentPlayer.transform.position = new Vector3(BoundsManager.GetBounds()[0], 0, currentPlayer.transform.position.z);
                    break;
                }
            case Area.Suburbs:
                {
                    BoundsManager.SetBounds(-3.84f, 3.84f, -3.2f, 3.2f);
                    currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[3], currentPlayer.transform.position.z);
                    break;
                }
            case Area.City:
                {
                    BoundsManager.SetBounds(-6.4f, 6.4f, -1.72f, 1.72f);
                    currentPlayer.transform.position = new Vector3(BoundsManager.GetBounds()[1], 0, currentPlayer.transform.position.z);
                    break;
                }
        }
        currentArea = newArea;
        if (isPlayerInVehicle)
        {
            VehicleManager.SetVehicleArea();
            EventMessenger.TriggerEvent("DeductAreaSwitchFuel");
        }
        else if (VehicleManager.currentVehicleType != VehicleManager.VehicleType.None)
        {
            EventMessenger.TriggerEvent("UpdateVehicleArea");
            VehicleManager.UpdateVehicleArea();
        }
        EventMessenger.TriggerEvent("SetPlayerCanActTrue");
        EventMessenger.TriggerEvent("UpdatePOIPointers");
        ObjectPoolManager.ResetPool("POIIndicators");
        EventMessenger.TriggerEvent("UpdatePOIIndicators");
        yield break;
    }
    private void StartTransition()
    {
        isGameActive = false;
        isInTransition = true;
        SceneManager.LoadSceneAsync("Transition", LoadSceneMode.Additive);
    }
    public void EndTransition()
    {
        isGameActive = true;
        isInTransition = false;
        SceneManager.UnloadSceneAsync("Transition");
    }
    
    public static void PauseGame()
    {
        isGameActive = false;
        Time.timeScale = 0;
    }
    public static void UnpauseGame()
    {
        isGameActive = true;
        Time.timeScale = 1;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
