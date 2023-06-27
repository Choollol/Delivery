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
        EventMessenger.StartListening("OpenRestaurant", OpenRestaurant);
        EventMessenger.StartListening("OpenVehicleShop", OpenVehicleShop);
        EventMessenger.StartListening("OpenGasShop", OpenGasShop);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("ExitGame", ExitGame);
        EventMessenger.StopListening("OpenRestaurant", OpenRestaurant);
        EventMessenger.StopListening("OpenVehicleShop", OpenVehicleShop);
        EventMessenger.StopListening("OpenGasShop", OpenGasShop);
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
    public static void OtherMenuOpened()
    {
        isMenuOpen = true;
        isMenuOnMain = true;
    }
    public static void OtherMenuClosed()
    {
        isMenuOpen = false;
        EventMessenger.TriggerEvent("SetPlayerCanActTrue");
    }
    private void OpenRestaurant()
    {
        SceneManager.LoadSceneAsync("Restaurant", LoadSceneMode.Additive);
        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        OtherMenuOpened();
    }
    private void OpenVehicleShop()
    {
        SceneManager.LoadSceneAsync("Vehicle_Shop", LoadSceneMode.Additive);
        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        AudioManager.PlaySound("Shop Chime");
        OtherMenuOpened();
    }
    private void OpenGasShop()
    {
        SceneManager.LoadSceneAsync("Gas_Shop", LoadSceneMode.Additive);
        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        OtherMenuOpened();
    }
    public void SwitchArea(Area newArea)
    {
        StartCoroutine(HandleSwitchArea(newArea));
    }
    private IEnumerator HandleSwitchArea(Area newArea)
    {
        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
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
        SceneManager.UnloadSceneAsync(currentArea.ToString());
        while (!loadNewArea.isDone)
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
                                currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[2], currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.Farm:
                            {
                                currentPlayer.transform.position = new Vector3(BoundsManager.GetBounds()[1], 0, currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.Suburbs:
                            {
                                currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[3], currentPlayer.transform.position.z);
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
                    BoundsManager.SetBounds(-5, 5, -5, 5);
                    currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[2], currentPlayer.transform.position.z);
                    break;
                }
            case Area.Farm:
                {
                    BoundsManager.SetBounds(-5, 5, -5, 5);
                    currentPlayer.transform.position = new Vector3(BoundsManager.GetBounds()[0], 0, currentPlayer.transform.position.z);
                    break;
                }
            case Area.Suburbs:
                {
                    BoundsManager.SetBounds(-5, 5, -5, 5);
                    currentPlayer.transform.position = new Vector3(0, BoundsManager.GetBounds()[3], currentPlayer.transform.position.z);
                    break;
                }
            case Area.City:
                {
                    BoundsManager.SetBounds(-6.4f, 6.4f, -3.2f, 3.2f);
                    currentPlayer.transform.position = new Vector3(BoundsManager.GetBounds()[1], 0, currentPlayer.transform.position.z);
                    break;
                }
        }
        currentArea = newArea;
        if (isPlayerInVehicle)
        {
            VehicleManager.SetVehicleArea();
        }
        else if (VehicleManager.currentVehicleType != VehicleManager.VehicleType.None)
        {
            EventMessenger.TriggerEvent("UpdateVehicleArea");
            VehicleManager.UpdateVehicleArea();
        }
        EventMessenger.TriggerEvent("SetPlayerCanActTrue");
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
    
    public void PauseGame()
    {
        isGameActive = false;
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        isGameActive = true;
        Time.timeScale = 1;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
