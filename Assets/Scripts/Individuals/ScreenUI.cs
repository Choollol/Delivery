using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    private GameObject fuelUI;
    void Start()
    {
        fuelUI = transform.Find("Fuel UI").gameObject;
    }
    private void OnEnable()
    {
        EventMessenger.StartListening("EnableFuelUI", EnableFuelUI);
        EventMessenger.StartListening("DisableFuelUI", DisableFuelUI);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("EnableFuelUI", EnableFuelUI);
        EventMessenger.StopListening("DisableFuelUI", DisableFuelUI);
    }
    private void EnableFuelUI()
    {
        fuelUI.gameObject.SetActive(true);
    }
    private void DisableFuelUI()
    {
        fuelUI.gameObject.SetActive(false);
    }
}
