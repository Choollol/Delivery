using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    private string path;

    private SaveData data = new SaveData();
    private void Awake()
    {
        path = Application.persistentDataPath + "/syonpleqisn.json";
    }
    void Start()
    {
        Load();
    }

    private void Save()
    {
        data.sfxVolume = VolumeManager.sfxVolume;
        data.bgmVolume = VolumeManager.bgmVolume;

        bool doAdd = data.shopItemKeys.Count == 0;
        for (int i = 1; i < Enum.GetNames(typeof(ShopManager.VehicleType)).Length; i++)
        {
            for (int j = 0; j < Enum.GetNames(typeof(ShopManager.ItemType)).Length; j++)
            {
                string itemID = ((ShopManager.VehicleType)i).ToString() + (ShopManager.ItemType)j;
                int index = (i - 1) * Enum.GetNames(typeof(ShopManager.VehicleType)).Length + j;
                if (doAdd)
                {
                    data.shopItemKeys.Add(itemID);
                    data.availabilityCatalogueValues.Add(ShopManager.availabilityCatalogue[itemID]);
                    data.upgradeAmountsCatalogueValues.Add(ShopManager.upgradeAmountsCatalogue[itemID]);
                }
                else
                {
                    data.shopItemKeys[index] = itemID;
                    data.availabilityCatalogueValues[index] = ShopManager.availabilityCatalogue[itemID];
                    data.upgradeAmountsCatalogueValues[index] = ShopManager.upgradeAmountsCatalogue[itemID];
                }
                
            }
        }

        WriteData();
    }
    private void Load() 
    {
        ReadData();

        VolumeManager.Instance.SetVolumes(data.sfxVolume, data.bgmVolume);

        for (int i = 0; i < data.shopItemKeys.Count; i++)
        {
            ShopManager.availabilityCatalogue[data.shopItemKeys[i]] = data.availabilityCatalogueValues[i];
            ShopManager.upgradeAmountsCatalogue[data.shopItemKeys[i]] = data.upgradeAmountsCatalogueValues[i];
        }
    }
    private void WriteData()
    {
        string jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(path, jsonData);
    }
    private void ReadData()
    {
        if (File.Exists(path))
        {
            string contents = File.ReadAllText(path);

            data = JsonUtility.FromJson<SaveData>(contents);
        }
        else
        {
            WriteData();
        }
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}
