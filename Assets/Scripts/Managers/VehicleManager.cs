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

    public static GameObject vehicle
    {
        get
        {
            return GameObject.Find("Vehicles").transform.Find(currentVehicleType.ToString().AddSpaces()).gameObject;
        }
        private set { }
    }

    private static float maxFuel;
    private static float currentFuel;

    private static float fuelConsumptionRate = 0.01f;
    private static int areaSwitchFuelCost = 30;
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
    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("currentVehicleFuel", currentFuel);
        PrimitiveMessenger.AddObject("maxVehicleFuel", maxFuel);

        EventMessenger.StartListening("AddVehicleFuel", AddFuel);
        EventMessenger.StartListening("RefillVehicleFuel", RefillFuel);

        EventMessenger.StartListening("DeductAreaSwitchFuel", DeductAreaSwitchFuel);

        PrimitiveMessenger.AddObject("fuelAddAmount", 0);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("currentVehicleFuel");
        PrimitiveMessenger.RemoveObject("maxVehicleFuel");

        EventMessenger.StopListening("AddVehicleFuel", AddFuel);
        EventMessenger.StopListening("RefillVehicleFuel", RefillFuel);

        EventMessenger.StopListening("DeductAreaSwitchFuel", DeductAreaSwitchFuel);

        PrimitiveMessenger.RemoveObject("fuelAddAmount");
    }
    private void Start()
    {
        area = GameManager.currentArea;
    }
    public static void SetFuel(float newMaxFuel, float newCurrentFuel)
    {
        maxFuel = newMaxFuel;
        currentFuel = newCurrentFuel;
        PrimitiveMessenger.EditObject("maxVehicleFuel", maxFuel);
        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
    }
    public static void Drive()
    {
        currentFuel -= fuelConsumptionRate;
        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
        if (currentFuel <= 0)
        {
            FuelEmpty();
        }
    }
    private void RefillFuel()
    {
        float amount = PrimitiveMessenger.GetObject("vehicleRefillAmount");
        currentFuel += amount;

        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
    }
    private void DeductAreaSwitchFuel()
    {
        currentFuel -= areaSwitchFuelCost;
        if (currentFuel < 0)
        {
            FuelEmpty();
        }
        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
    }
    private void AddFuel()
    {
        maxFuel += PrimitiveMessenger.GetObject("fuelAddAmount");
        currentFuel += PrimitiveMessenger.GetObject("fuelAddAmount");
        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
        PrimitiveMessenger.EditObject("maxVehicleFuel", maxFuel);
    }
    public static void FuelEmpty()
    {
        currentFuel = 0;
        EventMessenger.TriggerEvent("CanVehicleMoveFalse");
        AudioManager.StopSound("Vehicle Engine");
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
