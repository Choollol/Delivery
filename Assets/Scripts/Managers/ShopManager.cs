using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public enum ItemType
    {
        Vehicle, Speed, Fuel
    }
    public enum VehicleType
    {
        None, SmallCar, LargeCar, Truck
    }


    private static ShopManager instance;
    public static ShopManager Instance
    {
        get { return instance; }
    }

    public static bool isShopOpen { get; private set; }

    public static Dictionary<string, int> availabilityCatalogue = new Dictionary<string, int>(); //0 = unlocked, 1 = locked, 2 = out of stock
    public static Dictionary<string, int> upgradeAmountsCatalogue = new Dictionary<string, int>();

    private GameObject vehicles;

    private static VehicleType currentVehicle = VehicleType.None;

    [SerializeField] private GameObject descriptionBox;
    private TextMeshProUGUI descriptionText;
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

        for (int i = 1; i < Enum.GetNames(typeof(VehicleType)).Length; i++)
        {
            for (int j = 0; j < Enum.GetNames(typeof(ItemType)).Length; j++)
            {
                string itemID = ((VehicleType)i).ToString() + (ItemType)j;

                int temp;
                if (!availabilityCatalogue.TryGetValue(((VehicleType)i).ToString() + (ItemType)j, out temp))
                {
                    availabilityCatalogue.Add(itemID, 1);
                }
                if (!upgradeAmountsCatalogue.TryGetValue(((VehicleType)i).ToString() + (ItemType)j, out temp))
                {
                    upgradeAmountsCatalogue.Add(itemID, 0);
                }

                switch (availabilityCatalogue[itemID])
                {
                    case 0:
                        {
                            EventManager.TriggerEvent("unlock" + itemID);
                            break;
                        }
                    case 1:
                        {
                            EventManager.TriggerEvent("lock" + itemID);
                            break;
                        }
                    case 2:
                        {
                            EventManager.TriggerEvent("outOfStock" + itemID);
                            break;
                        }
                }
            }
        }
    }
    void Start()
    {
        vehicles = GameObject.Find("Vehicles");

        isShopOpen = true;

        descriptionText = descriptionBox.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        descriptionBox.SetActive(false);

        UpdateItemAvailabilities();
    }

    public void OpenDescription(string description, float fontSize)
    {
        descriptionBox.SetActive(true);
        descriptionText.text = description;
        descriptionText.fontSize = fontSize;
    }
    public void CloseDescription()
    {
        descriptionBox.SetActive(false);
    }
    public void BuyItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Vehicle:
                {
                    PurchaseVehicle();
                    break;
                }
            case ItemType.Speed:
                {
                    PurchaseSpeedUpgrade();
                    break;
                }
            case ItemType.Fuel:
                {
                    PurchaseCapacityUpgrade();
                    break;
                }
        }
    }
    public void PurchaseVehicle()
    {
        if ((int)currentVehicle > 0)
        {
            vehicles.transform.GetChild((int)currentVehicle - 1).gameObject.SetActive(false);
        }

        currentVehicle = (VehicleType)((int)currentVehicle + 1);

        vehicles.transform.GetChild((int)currentVehicle - 1).gameObject.SetActive(true);

        UpdateItemAvailabilities();
    }
    public void PurchaseSpeedUpgrade()
    {
        switch (currentVehicle)
        {
            case VehicleType.SmallCar:
                {
                    break;
                }
            case VehicleType.LargeCar:
                {
                    break;
                }
            case VehicleType.Truck:
                {
                    break;
                }
        }
    }
    public void PurchaseCapacityUpgrade()
    {
        switch (currentVehicle)
        {
            case VehicleType.SmallCar:
                {
                    break;
                }
            case VehicleType.LargeCar:
                {
                    break;
                }
            case VehicleType.Truck:
                {
                    break;
                }
        }
    }
    public void UpdateItemAvailabilities()
    {
        for (int i = 1; i < Enum.GetNames(typeof(VehicleType)).Length; i++)
        {
            for (int j = 0; j < Enum.GetNames(typeof(ItemType)).Length; j++)
            {
                string itemID = ((VehicleType)i).ToString() + (ItemType)j;
                if (availabilityCatalogue[itemID] != 2 && (i == (int)currentVehicle || (i == (int)currentVehicle + 1 && j == (int)ItemType.Vehicle)))
                {
                    EventManager.TriggerEvent("unlock" + itemID);
                }
                else if (availabilityCatalogue[itemID] == 2)
                {
                    EventManager.TriggerEvent("outOfStock" + itemID);
                }
                else
                {
                    EventManager.TriggerEvent("lock" + itemID);
                }
            }
        }
    }
    public void CloseShop()
    {
        SceneManager.UnloadSceneAsync("Shop");
        EventManager.TriggerEvent("setCanActTrue");
        isShopOpen = false;
    }
}
