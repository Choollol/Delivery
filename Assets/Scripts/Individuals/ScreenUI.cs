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
        EventMessenger.StartListening("EnableScreenUI", EnableScreenUI);
        EventMessenger.StartListening("DisableScreenUI", DisableScreenUI);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("EnableFuelUI", EnableFuelUI);
        EventMessenger.StopListening("DisableFuelUI", DisableFuelUI);
        EventMessenger.StopListening("EnableScreenUI", EnableScreenUI);
        EventMessenger.StopListening("DisableScreenUI", DisableScreenUI);
    }
    private void EnableFuelUI()
    {
        fuelUI.gameObject.SetActive(true);
    }
    private void DisableFuelUI()
    {
        fuelUI.gameObject.SetActive(false);
    }
    private void EnableScreenUI()
    {
        gameObject.SetActive(true);
    }
    private void DisableScreenUI()
    {
        gameObject.SetActive(false);
    }
}
