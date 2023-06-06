using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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

    public bool doContinueTransition;

    public bool isInTransition { get; private set; }

    public bool isPlayerInVehicle;
    public string vehicleName;

    public Area currentArea { get; private set; }

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
    private void Start()
    {
        currentArea = Area.Overworld;

        BoundsManager.Instance.SetBounds(-8, 8, -5.76f, 5.76f);

        isGameActive = true;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (ShopManager.isShopOpen)
            {
                if (UIManager.Instance.currentUI == "Main UI")
                {
                    ShopManager.Instance.CloseShop();
                }
                else
                {
                    UIManager.Instance.SwitchUI("Main UI");
                }
            }
            else if (isGameActive && !isInTransition && !ShopManager.isShopOpen)
            {
                MenuManager.Instance.OpenPauseMenu();
            }
            else
            {
                MenuManager.Instance.ClosePauseMenu();
            }
        }
    }
    public static void OpenShop()
    {
        SceneManager.LoadSceneAsync("Shop", LoadSceneMode.Additive);
        EventManager.TriggerEvent("setCanActFalse");
        AudioManager.PlaySound("Shop Chime");
    }
    public void SwitchArea(Area newArea)
    {
        StartCoroutine(HandleSwitchArea(newArea));
    }
    private IEnumerator HandleSwitchArea(Area newArea)
    {
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
                    BoundsManager.Instance.SetBounds(-8, 8, -5.76f, 5.76f);
                    switch (currentArea)
                    {
                        case Area.Market:
                            {
                                currentPlayer.transform.position = new Vector3(0, BoundsManager.Instance.GetBounds()[2], currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.Farm:
                            {
                                currentPlayer.transform.position = new Vector3(BoundsManager.Instance.GetBounds()[1], 0, currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.Suburbs:
                            {
                                currentPlayer.transform.position = new Vector3(0, BoundsManager.Instance.GetBounds()[3], currentPlayer.transform.position.z);
                                break;
                            }
                        case Area.City:
                            {
                                currentPlayer.transform.position = new Vector3(BoundsManager.Instance.GetBounds()[0], 0, currentPlayer.transform.position.z);
                                break;
                            }
                    }
                    break;
                }
            case Area.Market:
                {
                    BoundsManager.Instance.SetBounds(-5, 5, -5, 5);
                    currentPlayer.transform.position = new Vector3(0, BoundsManager.Instance.GetBounds()[2], currentPlayer.transform.position.z);
                    break;
                }
            case Area.Farm:
                {
                    BoundsManager.Instance.SetBounds(-5, 5, -5, 5);
                    currentPlayer.transform.position = new Vector3(BoundsManager.Instance.GetBounds()[0], 0, currentPlayer.transform.position.z);
                    break;
                }
            case Area.Suburbs:
                {
                    BoundsManager.Instance.SetBounds(-5, 5, -5, 5);
                    currentPlayer.transform.position = new Vector3(0, BoundsManager.Instance.GetBounds()[3], currentPlayer.transform.position.z);
                    break;
                }
            case Area.City:
                {
                    BoundsManager.Instance.SetBounds(-6.4f, 6.4f, -3.2f, 3.2f);
                    currentPlayer.transform.position = new Vector3(BoundsManager.Instance.GetBounds()[1], 0, currentPlayer.transform.position.z);
                    break;
                }
        }
        currentArea = newArea;
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
