using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    private GameObject vehicleUI;

    private List<GameObject> screenUIList = new List<GameObject>();
    void Start()
    {
        vehicleUI = transform.Find("Vehicle UI").gameObject;

        for (int i = 0; i < transform.childCount; i++)
        {
            screenUIList.Add(transform.GetChild(i).gameObject);
        }
    }
    private void OnEnable()
    {
        EventMessenger.StartListening("EnableVehicleUI", EnableFuelUI);
        EventMessenger.StartListening("DisableVehicleUI", DisableFuelUI);
        EventMessenger.StartListening("EnableScreenUI", EnableScreenUI);
        EventMessenger.StartListening("DisableScreenUI", DisableScreenUI);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("EnableVehicleUI", EnableFuelUI);
        EventMessenger.StopListening("DisableVehicleUI", DisableFuelUI);
        EventMessenger.StopListening("EnableScreenUI", EnableScreenUI);
        EventMessenger.StopListening("DisableScreenUI", DisableScreenUI);
    }
    private void EnableFuelUI()
    {
        vehicleUI.gameObject.SetActive(true);
    }
    private void DisableFuelUI()
    {
        vehicleUI.gameObject.SetActive(false);
    }
    private void EnableScreenUI()
    {
        foreach (GameObject ui in screenUIList)
        {
            if (ui.name != vehicleUI.name)
            {
                ui.SetActive(true);
            }
        }
    }
    private void DisableScreenUI()
    {
        foreach (GameObject ui in screenUIList)
        {
            ui.SetActive(false);
        }
    }
}
