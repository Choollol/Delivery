using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIManager : MonoBehaviour
{
    private static POIManager instance;
    public static POIManager Instance
    {
        get { return instance; }
    }

    public static int ID = 0;

    [SerializeField] private GameObject poiIndicator;
    [SerializeField] private GameObject poiIndicatorsHolder;

    [SerializeField] private GameObject leftPointer;
    [SerializeField] private GameObject rightPointer;
    [SerializeField] private GameObject upPointer;
    [SerializeField] private GameObject downPointer;

    private static Dictionary<GameManager.Area, int> areaPOICounts = new Dictionary<GameManager.Area, int>(); // Total POIs

    private static Dictionary<GameManager.Area, int> poiCounts = new Dictionary<GameManager.Area, int>(); // Current POIs

    public static Dictionary<KeyValuePair<GameManager.Area, int>, int> poiOrders { get; private set; } // Area, ID, Number of orders

    private float supplierTimer = 5;
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

        poiOrders = new Dictionary<KeyValuePair<GameManager.Area, int>, int>();

        areaPOICounts.Add(GameManager.Area.Overworld, 8);
        areaPOICounts.Add(GameManager.Area.Market, 4);
        areaPOICounts.Add(GameManager.Area.Farm, 1);
        areaPOICounts.Add(GameManager.Area.Suburbs, 8);
        areaPOICounts.Add(GameManager.Area.City, 9);
        foreach (GameManager.Area area in Enum.GetValues(typeof(GameManager.Area)))
        {
            poiCounts.Add(area, 0);
            for (int i = 0; i < areaPOICounts[area]; i++)
            {
                poiOrders.Add(new KeyValuePair<GameManager.Area, int>(area, i), 0);
            }
        }

    }
    private void OnEnable()
    {
        EventMessenger.StartListening("UpdatePOIPointers", UpdatePOIPointers);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("UpdatePOIPointers", UpdatePOIPointers);
    }
    void Start()
    {
        ObjectPoolManager.AddPool("POIIndicators", poiIndicator, 20, poiIndicatorsHolder);

        StartCoroutine(AddSupplierPOI());

        UpdatePOIPointers();
    }
    public static void AddCustomerPOI(int orders)
    {
        GameManager.Area area = GameManager.Area.Overworld;
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 1:
                {
                    area = GameManager.Area.Suburbs;
                    break;
                }
            case 2:
                {
                    area = GameManager.Area.City;
                    break;
                }
        }
        int id = UnityEngine.Random.Range(0, areaPOICounts[area]);
        if (poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)] > 0)
        {
            for (int i = 0; i < areaPOICounts[area]; i++)
            {
                if (poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)] == 0)
                {
                    id = i;
                    break;
                }
            }
        }
        poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)] += orders;
        poiCounts[area]++;
        Instance.UpdatePOIPointers();
    }
    private IEnumerator AddSupplierPOI()
    {
        if (RestaurantManager.ingredients < 10)
        {
            supplierTimer = 5;
        }
        else if (RestaurantManager.ingredients < 50)
        {
            supplierTimer = 20;
        }
        else
        {
            supplierTimer = 60;
        }
        yield return new WaitForSeconds(supplierTimer);
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                {
                    int id = UnityEngine.Random.Range(0, areaPOICounts[GameManager.Area.Market]);
                    int orders = UnityEngine.Random.Range(1, 4);
                    poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.Area.Market, id)] += orders;
                    if (poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.Area.Market, id)] > 12)
                    {
                        poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.Area.Market, id)] = 12;
                    }
                    poiCounts[GameManager.Area.Market] += orders;
                    break;
                }
            case 1:
                {
                    int orders = UnityEngine.Random.Range(2, 6);
                    poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.Area.Farm, 0)] += orders;
                    if (poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.Area.Farm, 0)] > 50)
                    {
                        poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.Area.Farm, 0)] = 50;
                    }
                    poiCounts[GameManager.Area.Farm] += orders;
                    break;
                }
        }
        UpdatePOIPointers();
        while (!GameManager.isGameActive)
        {
            yield return null;
        }
        StartCoroutine(AddSupplierPOI());
        yield break;
    }
    public static void CompleteOrder(GameManager.Area area, int id)
    {
        PrimitiveMessenger.EditObject("dishesToDropOff", poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)]);
        EventMessenger.TriggerEvent("DropOffDishesCapacity");
        poiCounts[area] -= poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)];
        poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)] = 0;
        EventMessenger.TriggerEvent("UpdatePOIIndicators");
        Instance.UpdatePOIPointers();
        Debug.Log(poiCounts[area] + " " + area);
    }
    public static void PickUpIngredients(GameManager.Area area, int id)
    {
        PrimitiveMessenger.EditObject("ingredientsToPickUp", poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)]);
        EventMessenger.TriggerEvent("PickUpIngredients");
        poiCounts[area] -= poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)];
    }
    public static void AddPOIIndicator(Vector3 targetPos)
    {
        ObjectPoolManager.PullFromPool("POIIndicators", targetPos);
    }
    private void UpdatePOIPointers()
    {
        ResetPointers();
        switch (GameManager.currentArea)
        {
            case GameManager.Area.Overworld:
                {
                    if (poiCounts[GameManager.Area.Market] > 0)
                    {
                        upPointer.SetActive(true);
                    }
                    if (poiCounts[GameManager.Area.Farm] > 0)
                    {
                        rightPointer.SetActive(true);
                    }
                    if (poiCounts[GameManager.Area.Suburbs] > 0)
                    {
                        downPointer.SetActive(true);
                    }
                    if (poiCounts[GameManager.Area.City] > 0)
                    {
                        leftPointer.SetActive(true);
                    }
                    break;
                }
            case GameManager.Area.Market:
                {
                    if (poiCounts[GameManager.Area.Farm] > 0)
                    {
                        rightPointer.SetActive(true);
                    }
                    if (poiCounts[GameManager.Area.Suburbs] > 0 || poiCounts[GameManager.Area.Overworld] > 0)
                    {
                        downPointer.SetActive(true);
                    }
                    if (poiCounts[GameManager.Area.City] > 0)
                    {
                        leftPointer.SetActive(true);
                    }
                    break;
                }
            case GameManager.Area.Farm:
                {
                    if (poiCounts[GameManager.Area.Overworld] > 0 || poiCounts[GameManager.Area.Suburbs] > 0 ||
                        poiCounts[GameManager.Area.Market] > 0 || poiCounts[GameManager.Area.City] > 0)
                    {
                        leftPointer.SetActive(true);
                    }
                    break;
                }
            case GameManager.Area.Suburbs:
                {
                    if (poiCounts[GameManager.Area.Farm] > 0)
                    {
                        rightPointer.SetActive(true);
                    }
                    if (poiCounts[GameManager.Area.Market] > 0 || poiCounts[GameManager.Area.Overworld] > 0)
                    {
                        upPointer.SetActive(true);
                    }
                    if (poiCounts[GameManager.Area.City] > 0)
                    {
                        leftPointer.SetActive(true);
                    }
                    break;
                }
            case GameManager.Area.City:
                {
                    if (poiCounts[GameManager.Area.Overworld] > 0 || poiCounts[GameManager.Area.Suburbs] > 0 ||
                        poiCounts[GameManager.Area.Market] > 0 || poiCounts[GameManager.Area.Farm] > 0)
                    {
                        rightPointer.SetActive(true);
                    }
                    break;
                }
        }
        if (poiCounts[GameManager.currentArea] > 0)
        {
            EventMessenger.TriggerEvent("EnableCurrentPOIPointer");
        }
        else
        {
            EventMessenger.TriggerEvent("DisableCurrentPOIPointer");
        }
    }
    
    private void ResetPointers()
    {
        leftPointer.SetActive(false);
        rightPointer.SetActive(false);
        upPointer.SetActive(false);
        downPointer.SetActive(false);
    }

}
