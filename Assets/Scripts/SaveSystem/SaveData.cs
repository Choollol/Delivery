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

    public bool hasCompletedCoinfallTutorial;

    public int restaurantIngredients;
    public int restaurantDishes;
    public int capacityIngredients;
    public int capacityDishes;

    public List<int> poiOrders = new List<int>();

    public bool hasKeycard;
    public bool isKeycardUIEnabled;

    public int coinfallStartingLevel;
}
