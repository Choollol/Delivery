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

    public static int currentOrderID;

    [SerializeField] private GameObject poiIndicator;
    [SerializeField] private GameObject poiIndicatorsHolder;

    [SerializeField] private GameObject leftPointer;
    [SerializeField] private GameObject rightPointer;
    [SerializeField] private GameObject upPointer;
    [SerializeField] private GameObject downPointer;

    public static Dictionary<GameManager.Area, int> areaPOICounts { get; private set; }  // Total POIs

    public static Dictionary<GameManager.Area, int> poiCounts = new Dictionary<GameManager.Area, int>(); // Current POIs

    public static Dictionary<KeyValuePair<GameManager.Area, int>, int> poiOrders = 
        new Dictionary<KeyValuePair<GameManager.Area, int>, int>(); // Area, ID, Number of orders

    private static float supplierTimer = 5;

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

        areaPOICounts = new Dictionary<GameManager.Area, int>
        {
            { GameManager.Area.Overworld, 8 },
            { GameManager.Area.Market, 4 },
            { GameManager.Area.Farm, 1 },
            { GameManager.Area.Suburbs, 8 },
            { GameManager.Area.City, 9 }
        };

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

        EventMessenger.StartListening("CompleteOrder", CompleteOrder);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("UpdatePOIPointers", UpdatePOIPointers);

        EventMessenger.StopListening("CompleteOrder", CompleteOrder);
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
        UpdateUI();
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
        UpdateUI();
        while (!GameManager.isGameActive)
        {
            yield return null;
        }
        StartCoroutine(AddSupplierPOI());
        yield break;
    }
    public static void CompleteOrder()
    {
        PrimitiveMessenger.EditObject("dishesToDropOff", 
            poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, currentOrderID)]);
        EventMessenger.TriggerEvent("DropOffDishesCapacity");
        poiCounts[GameManager.currentArea] -= poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, currentOrderID)];
        poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, currentOrderID)] = 0;
        UpdateUI();
    }
    public static void PickUpIngredients(GameManager.Area area, int id)
    {
        int spaceAvailable = PrimitiveMessenger.GetObject("maxCapacity") - PrimitiveMessenger.GetObject("capacityInUse");
        int ingredientsToPickUp;
        if (spaceAvailable >= poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)])
        {
            ingredientsToPickUp = poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)];
        }
        else
        {
            ingredientsToPickUp = spaceAvailable;
        }
        if (ingredientsToPickUp == 0)
        {
            return;
        }
        AudioManager.PlaySound("Supplier Pick Up Sound");
        PrimitiveMessenger.EditObject("ingredientsToPickUp", ingredientsToPickUp);
        EventMessenger.TriggerEvent("PickUpIngredients");
        poiOrders[new KeyValuePair<GameManager.Area, int>(area, id)] -= ingredientsToPickUp;
        poiCounts[area] -= ingredientsToPickUp;
        UpdateUI();
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
    public static void UpdateUI()
    {
        Instance.UpdatePOIPointers();
        EventMessenger.TriggerEvent("UpdatePOIIndicators");
    }
    private void ResetPointers()
    {
        leftPointer.SetActive(false);
        rightPointer.SetActive(false);
        upPointer.SetActive(false);
        downPointer.SetActive(false);
    }

}
