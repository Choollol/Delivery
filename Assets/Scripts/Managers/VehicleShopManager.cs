using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VehicleShopManager : MonoBehaviour
{
    public enum ItemType
    {
        Vehicle, Speed, Fuel
    }

    private static VehicleShopManager instance;
    public static VehicleShopManager Instance
    {
        get { return instance; }
    }

    public static bool hasPlayerOpenedShop { get; private set; }

    public static Dictionary<string, int> availabilityCatalogue = new Dictionary<string, int>(); //0 = unlocked, 1 = locked, 2 = out of stock
    public static Dictionary<string, int> upgradeAmountsCatalogue = new Dictionary<string, int>();

    private GameObject vehicles;

    [SerializeField] private GameObject popupUI;
    private TextMeshProUGUI descriptionText;
    private GameObject popupPriceUI;
    private TextMeshProUGUI popupPriceText;
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
        for (int i = 1; i < Enum.GetNames(typeof(VehicleManager.VehicleType)).Length; i++)
        {
            for (int j = 0; j < Enum.GetNames(typeof(ItemType)).Length; j++)
            {
                string itemID = ((VehicleManager.VehicleType)i).ToString() + (ItemType)j;

                int temp;
                if (!availabilityCatalogue.TryGetValue(((VehicleManager.VehicleType)i).ToString() + (ItemType)j, out temp))
                {
                    availabilityCatalogue.Add(itemID, 1);
                }
                if (!upgradeAmountsCatalogue.TryGetValue(((VehicleManager.VehicleType)i).ToString() + (ItemType)j, out temp))
                {
                    upgradeAmountsCatalogue.Add(itemID, 0);
                }

                switch (availabilityCatalogue[itemID])
                {
                    case 0:
                        {
                            EventMessenger.TriggerEvent("Unlock" + itemID);
                            break;
                        }
                    case 1:
                        {
                            EventMessenger.TriggerEvent("Lock" + itemID);
                            break;
                        }
                    case 2:
                        {
                            EventMessenger.TriggerEvent("OutOfStock" + itemID);
                            break;
                        }
                }
            }
        }
    }
    private void OnEnable()
    {
        EventMessenger.StartListening("SwitchMenuToMain", ClosePopup);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("SwitchMenuToMain", ClosePopup);
    }
    void Start()
    {
        vehicles = GameObject.Find("Vehicles");

        hasPlayerOpenedShop = true;

        descriptionText = popupUI.transform.Find("Description").Find("Text").GetComponent<TextMeshProUGUI>();
        popupPriceUI = popupUI.transform.Find("Price").gameObject;
        popupPriceText = popupPriceUI.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        popupUI.SetActive(false);

        UpdateItemAvailabilities();
    }

    public void OpenPopup(string description, float fontSize, bool doShowPrice, Vector2 itemPosition, int itemPrice)
    {
        popupUI.SetActive(true);
        descriptionText.text = description;
        descriptionText.fontSize = fontSize;
        if (doShowPrice)
        {
            popupPriceUI.SetActive(true);
            popupPriceUI.transform.position = itemPosition;
            popupPriceText.text = itemPrice.ToString();
        }
        else
        {
            popupPriceUI.SetActive(false);
        }
    }
    public void ClosePopup()
    {
        popupUI.SetActive(false);
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
                    PurchaseFuelUpgrade();
                    break;
                }
        }
    }
    public void PurchaseVehicle()
    {
        if ((int)VehicleManager.currentVehicleType > 0)
        {
            vehicles.transform.GetChild((int)VehicleManager.currentVehicleType - 1).gameObject.SetActive(false);
        }

        VehicleManager.SetVehicleType((VehicleManager.VehicleType)((int)VehicleManager.currentVehicleType + 1));

        vehicles.transform.GetChild((int)VehicleManager.currentVehicleType - 1).gameObject.SetActive(true);

        UpdateItemAvailabilities();
    }
    public void PurchaseSpeedUpgrade()
    {
        GameObject vehicle = GameObject.Find(GameManager.vehicleName);

        EventMessenger.TriggerEvent("AddVehicleSpeed");
    }
    public void PurchaseFuelUpgrade()
    {
        GameObject vehicle = GameObject.Find(GameManager.vehicleName);

        EventMessenger.TriggerEvent("AddVehicleFuel");
    }
    public void UpdateItemAvailabilities()
    {
        for (int i = 1; i < Enum.GetNames(typeof(VehicleManager.VehicleType)).Length; i++)
        {
            for (int j = 0; j < Enum.GetNames(typeof(ItemType)).Length; j++)
            {
                string itemID = ((VehicleManager.VehicleType)i).ToString() + (ItemType)j;
                if (availabilityCatalogue[itemID] == 2)
                {
                    EventMessenger.TriggerEvent("OutOfStock" + itemID);
                }
                else if (i == (int)VehicleManager.currentVehicleType || (i == (int)VehicleManager.currentVehicleType + 1 && j == (int)ItemType.Vehicle))
                {
                    EventMessenger.TriggerEvent("Unlock" + itemID);
                }
                else
                {
                    EventMessenger.TriggerEvent("Lock" + itemID);
                }
            }
        }
    }
}
