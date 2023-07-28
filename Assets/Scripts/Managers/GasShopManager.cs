using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GasShopManager : MonoBehaviour
{
    private static GasShopManager instance;
    public static GasShopManager Instance
    {
        get { return instance; }
    }

    private static float gasCost = 0.15f;
    private int cost;

    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private Slider slider;

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
    }
    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("vehicleRefillAmount", 0);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("vehicleRefillAmount");
    }
    void Start()
    {
        UIManager.doStayMain = true;

        if (VehicleManager.currentVehicleType != VehicleManager.VehicleType.None)
        {
            UpdateTexts();
            UpdateUI();
        }
        else
        {
            UIManager.Instance.SwitchUI("Vehicle Locked UI");
        }
    }
    public void PurchaseGas()
    {
        if (CurrencyManager.coins >= cost)
        {
            CurrencyManager.Instance.DecreaseCoins(cost);

            AudioManager.PlaySound("Purchase Sound");

            PrimitiveMessenger.EditObject("vehicleRefillAmount", 
                (PrimitiveMessenger.GetObject("maxVehicleFuel") - PrimitiveMessenger.GetObject("currentVehicleFuel")) * slider.value);
            EventMessenger.TriggerEvent("RefillVehicleFuel");
            UpdateTexts();
            UpdateUI();
        }
        else
        {
            AudioManager.PlaySound("Purchase Fail Sound");
        }
    }
    public void UpdateTexts()
    {
        percentageText.text = "% of missing fuel: " + (int)(slider.value * 100) + "%";
        costText.text = Mathf.Ceil(gasCost * slider.value * 
            (PrimitiveMessenger.GetObject("maxVehicleFuel") - PrimitiveMessenger.GetObject("currentVehicleFuel"))).ToString();
        cost = int.Parse(costText.text);
    }
    private void UpdateUI()
    {
        if (PrimitiveMessenger.GetObject("currentVehicleFuel") == PrimitiveMessenger.GetObject("maxVehicleFuel"))
        {
            UIManager.Instance.SwitchUI("Tank Full UI");
        }
    }
}
