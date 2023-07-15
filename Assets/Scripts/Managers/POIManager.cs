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
    public static void AddSupplierPOI()
    {

    }

    public static void AddPOIIndicator(Vector3 targetPos)
    {
        ObjectPoolManager.PullFromPool("POIIndicators", targetPos); //+ new Vector3(0, 0.12f));
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
    }
    
    private void ResetPointers()
    {
        leftPointer.SetActive(false);
        rightPointer.SetActive(false);
        upPointer.SetActive(false);
        downPointer.SetActive(false);
    }

}
