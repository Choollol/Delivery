using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Codice.Client.Common;

public class SaveManager : MonoBehaviour
{
    private string path;

    private SaveData data = new SaveData();

    private string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,[]{}:\".";
    private string encryptionKey = "`[/';.,:|@mzlapqnxksowbcjdievhfurgtyMZLAPQNXKSOWBCJDIEVHFURGTY-=*%!^&$";

    private int saveInterval = 180;
    private void Awake()
    {
        path = Application.persistentDataPath + "/syonpleqisn.json";
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

        if (VehicleShopManager.hasPlayerOpenedShop)
        {
            bool doAdd = data.shopItemKeys.Count == 0;
            for (int i = 1; i < Enum.GetNames(typeof(VehicleManager.VehicleType)).Length; i++)
            {
                for (int j = 0; j < Enum.GetNames(typeof(VehicleShopManager.ItemType)).Length; j++)
                {
                    string itemID = ((VehicleManager.VehicleType)i).ToString() + (VehicleShopManager.ItemType)j;
                    int index = (i - 1) * (Enum.GetNames(typeof(VehicleManager.VehicleType)).Length - 1) + j;
                    if (doAdd)
                    {
                        data.shopItemKeys.Add(itemID);
                        data.availabilityCatalogueValues.Add(VehicleShopManager.availabilityCatalogue[itemID]);
                        data.upgradeAmountsCatalogueValues.Add(VehicleShopManager.upgradeAmountsCatalogue[itemID]);
                    }
                    else
                    {
                        data.shopItemKeys[index] = itemID;
                        data.availabilityCatalogueValues[index] = VehicleShopManager.availabilityCatalogue[itemID];
                        data.upgradeAmountsCatalogueValues[index] = VehicleShopManager.upgradeAmountsCatalogue[itemID];
                    }
                }
            }
        }

        data.currentVehicleType = VehicleManager.currentVehicleType.ToString();

        if (VehicleManager.currentVehicleType != VehicleManager.VehicleType.None)
        {
            VehicleManager.SetVehicleActive(true);
            data.vehicleSpeed = PrimitiveMessenger.GetObject("vehicleSpeed");
            data.vehicleMaxFuel = PrimitiveMessenger.GetObject("maxVehicleFuel");
            data.vehicleCurrentFuel = PrimitiveMessenger.GetObject("currentVehicleFuel");
            VehicleManager.SetVehicleActive(false);
        }

        data.hasCompletedCoinfallTutorial = CoinfallManager.hasCompletedTutorial;

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
            GameObject vehicle = GameObject.Find("Vehicles").transform.Find(VehicleManager.currentVehicleType.ToString().AddSpaces()).gameObject;

            vehicle.SetActive(true);
            vehicle.GetComponent<VehicleMovement>().SetSpeed(data.vehicleSpeed);
            vehicle.GetComponent<VehicleFuel>().SetFuel(data.vehicleMaxFuel, true);
            vehicle.GetComponent<VehicleFuel>().SetFuel(data.vehicleCurrentFuel, false);
        }

        CoinfallManager.hasCompletedTutorial = data.hasCompletedCoinfallTutorial;
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
            Application.Quit();
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
