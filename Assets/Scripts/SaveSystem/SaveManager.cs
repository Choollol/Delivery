using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    private string path;

    private SaveData data = new SaveData();

    private string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,[]{}:\".";
    private string encryptionKey = "`[/';.,:|@mzlapqnxksowbcjdievhfurgtyMZLAPQNXKSOWBCJDIEVHFURGTY-=*%!^&$";

    private int saveInterval = 180;
    private void Awake()
    {
        path = Application.persistentDataPath + "/save.json";
    }
    void Start()
    {
        //debug
        //File.Delete(path);

        Load();

        InvokeRepeating("Save", saveInterval, saveInterval);
    }

    private void Save()
    {
        data.sfxVolume = VolumeManager.sfxVolume;
        data.bgmVolume = VolumeManager.bgmVolume;

        data.coins = CurrencyManager.coins;

        data.shopItemKeys.Clear();
        data.availabilityCatalogueValues.Clear();
        data.upgradeAmountsCatalogueValues.Clear();
        if (VehicleShopManager.hasPlayerOpenedShop)
        {
            for (int i = 1; i < Enum.GetNames(typeof(VehicleManager.VehicleType)).Length; i++)
            {
                for (int j = 0; j < Enum.GetNames(typeof(VehicleShopManager.ItemType)).Length; j++)
                {
                    string itemID = ((VehicleManager.VehicleType)i).ToString() + (VehicleShopManager.ItemType)j;

                    data.shopItemKeys.Add(itemID);
                    data.availabilityCatalogueValues.Add(VehicleShopManager.availabilityCatalogue[itemID]);
                    data.upgradeAmountsCatalogueValues.Add(VehicleShopManager.upgradeAmountsCatalogue[itemID]);
                }
            }
        }

        data.currentVehicleType = VehicleManager.currentVehicleType.ToString();

        if (VehicleManager.currentVehicleType != VehicleManager.VehicleType.None)
        {
            bool isVehicleDisabled = false;
            if (!VehicleManager.vehicle.activeSelf) 
            {
                isVehicleDisabled = true;
            }
            if (isVehicleDisabled)
            {
                VehicleManager.SetVehicleActive(true);
            }
            data.vehicleSpeed = PrimitiveMessenger.GetObject("vehicleSpeed");
            data.vehicleMaxFuel = PrimitiveMessenger.GetObject("maxVehicleFuel");
            data.vehicleCurrentFuel = PrimitiveMessenger.GetObject("currentVehicleFuel");
            if (isVehicleDisabled)
            {
                VehicleManager.SetVehicleActive(false);
            }
        }

        data.hasCompletedCoinfallTutorial = CoinfallManager.hasCompletedTutorial;

        data.restaurantIngredients = RestaurantManager.ingredients;
        data.restaurantDishes = PrimitiveMessenger.GetObject("numOfDishes");
        data.capacityIngredients = CapacityManager.ingredients;
        data.capacityDishes = CapacityManager.dishes;

        int numOfAreas = Enum.GetValues(typeof(GameManager.Area)).Length;
        data.poiOrders.Clear();
        int index = 0;
        for (int i = 0; i < numOfAreas; i++)
        {
            GameManager.Area area = (GameManager.Area)i;
            for (int j = 0; j < POIManager.areaPOICounts[area]; j++)
            {
                data.poiOrders.Add(POIManager.poiOrders[new KeyValuePair<GameManager.Area, int>(area, j)]);
                index++;
            }
        }

        data.hasKeycard = GameManager.hasKeycard;
        data.isKeycardUIEnabled = GameManager.isKeycardUIEnabled;

        data.coinfallStartingLevel = CoinfallManager.GetStartingLevel();

        WriteData();
    }
    private void Load() 
    {
        ReadData();

        VolumeManager.Instance.SetVolumes(data.sfxVolume, data.bgmVolume);
        CurrencyManager.Instance.SetCoins(data.coins);

        for (int i = 0; i < data.shopItemKeys.Count; i++)
        {
            string itemID = data.shopItemKeys[i];

            VehicleShopManager.availabilityCatalogue[itemID] = data.availabilityCatalogueValues[i];
            VehicleShopManager.upgradeAmountsCatalogue[itemID] = data.upgradeAmountsCatalogueValues[i];
        }

        VehicleManager.SetVehicleType(Enum.Parse<VehicleManager.VehicleType>(data.currentVehicleType));
        if (VehicleManager.currentVehicleType != VehicleManager.VehicleType.None)
        {
            GameObject vehicle = VehicleManager.vehicle;

            vehicle.SetActive(true);
            vehicle.GetComponent<VehicleMovement>().SetSpeed(data.vehicleSpeed);
            VehicleManager.SetFuel(data.vehicleMaxFuel, data.vehicleCurrentFuel);
        }

        CoinfallManager.hasCompletedTutorial = data.hasCompletedCoinfallTutorial;

        RestaurantManager.ingredients = data.restaurantIngredients;
        DishManager.SetDishes(data.restaurantDishes);
        CapacityManager.SetIngredients(data.capacityIngredients);
        CapacityManager.SetDishes(data.capacityDishes);

        int numOfAreas = Enum.GetValues(typeof(GameManager.Area)).Length;
        int index = 0;
        for (int i = 0; i < numOfAreas; i++)
        {
            GameManager.Area area = (GameManager.Area)i;
            for (int j = 0; j < POIManager.areaPOICounts[area]; j++)
            {
                POIManager.poiOrders[new KeyValuePair<GameManager.Area, int>(area, j)] = data.poiOrders[index];
                POIManager.poiCounts[area] += data.poiOrders[index];
                index++;
            }
        }
        POIManager.UpdateUI();

        GameManager.hasKeycard = data.hasKeycard;
        GameManager.isKeycardUIEnabled = data.isKeycardUIEnabled;

        CoinfallManager.SetStartingLevel(data.coinfallStartingLevel);
    }
    private void WriteData()
    {
        string jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(path, Encrypt(jsonData));
    }
    
    private void ReadData()
    {
        if (File.Exists(path))
        {
            string contents = File.ReadAllText(path);

            data = JsonUtility.FromJson<SaveData>(Decrypt(contents));
        }
        else
        {
            VolumeManager.Instance.SetVolumes(0.5f, 0.5f);
            Save();
            ReadData();
        }
    }
    private string Encrypt(string jsonData)
    {
        string encryptedData = "";
        for (int i = 0; i < jsonData.Length; i++)
        {
            encryptedData += encryptionKey[characters.IndexOf(jsonData[i])];
        }
        return encryptedData;
    }
    private string Decrypt(string contents)
    {
        string decryptedString = "";
        for (int i = 0; i < contents.Length; i++)
        {
            decryptedString += characters[encryptionKey.IndexOf(contents[i])];
        }
        return decryptedString;
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}
