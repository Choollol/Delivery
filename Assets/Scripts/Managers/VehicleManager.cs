using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public enum VehicleType
    {
        None, SmallCar, LargeCar, Truck
    }

    private static VehicleManager instance;
    public static VehicleManager Instance
    {
        get { return instance; }
    }

    public static VehicleType currentVehicleType { get; private set; }
    private static GameManager.Area area;

    private static GameObject vehicle
    {
        get
        {
            return GameObject.Find("Vehicles").transform.Find(currentVehicleType.ToString().AddSpaces()).gameObject;
        }
    }
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

        currentVehicleType = VehicleType.None;
    }
    private void Start()
    {
        area = GameManager.currentArea;
    }
    public static void SetVehicleType(VehicleType newType)
    {
        currentVehicleType = newType;
    }
    public static void UpdateVehicleArea()
    {
        if (GameManager.currentArea != area)
        {
            vehicle.SetActive(false);
        }
        else
        {
            vehicle.SetActive(true);
        }
    }
    public static void SetVehicleArea()
    {
        area = GameManager.currentArea;
    }
    public static void SetVehicleActive(bool active)
    {
        vehicle.SetActive(active);
    }
}
