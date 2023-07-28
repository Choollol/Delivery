using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    private GameObject vehicleUI;

    private List<GameObject> screenUIList = new List<GameObject>();

    private bool isScreenUIEnabled = true;
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
    private void Update()
    {
        if (Input.GetButtonDown("Toggle Screen UI") && GameManager.isInWorld)
        {
            if (isScreenUIEnabled)
            {
                DisableScreenUI();
            }
            else
            {
                EnableScreenUI();
            }
        }
    }
    private void EnableFuelUI()
    {
        if (isScreenUIEnabled)
        {
            vehicleUI.gameObject.SetActive(true);
        }
    }
    private void DisableFuelUI()
    {
        vehicleUI.gameObject.SetActive(false);
    }
    private void EnableScreenUI()
    {
        foreach (GameObject ui in screenUIList)
        {
            if (GameManager.isPlayerInVehicle || ui.name != vehicleUI.name)
            {
                ui.SetActive(true);
            }
        }
        isScreenUIEnabled = true;
    }
    private void DisableScreenUI()
    {
        foreach (GameObject ui in screenUIList)
        {
            ui.SetActive(false);
        }
        isScreenUIEnabled = false;
    }
}
