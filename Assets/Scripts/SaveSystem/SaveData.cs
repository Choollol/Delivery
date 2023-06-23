using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float sfxVolume;
    public float bgmVolume;

    public int coins;

    public List<string> shopItemKeys = new List<string>();
    public List<int> availabilityCatalogueValues = new List<int>();
    public List<int> upgradeAmountsCatalogueValues = new List<int>();

    public string currentVehicleType;
    public float vehicleSpeed;
    public float vehicleMaxFuel;
    public float vehicleCurrentFuel;
}
