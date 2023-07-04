using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleFuel : MonoBehaviour
{
    private static float fuelConsumptionRate = 0.01f;

    private static int areaSwitchFuelCost = 30;
    public float currentFuel { get; private set; }
    [SerializeField] private float maxFuel;

    private Rigidbody2D rb;

    [SerializeField] private Image fuelBar;

    [SerializeField] private float fuelAddAmount;
    private void Awake()
    {
        currentFuel = maxFuel;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
    }

    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("currentVehicleFuel", currentFuel);
        PrimitiveMessenger.AddObject("maxVehicleFuel", maxFuel);
        EventMessenger.StartListening("AddVehicleFuel", AddFuel);
        EventMessenger.StartListening("RefillVehicleFuel", RefillFuel);

        EventMessenger.StartListening("DeductAreaSwitchFuel", DeductAreaSwitchFuel);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("currentVehicleFuel");
        PrimitiveMessenger.RemoveObject("maxVehicleFuel");
        EventMessenger.StopListening("AddVehicleFuel", AddFuel);
        EventMessenger.StopListening("RefillVehicleFuel", RefillFuel);

        EventMessenger.StopListening("DeductAreaSwitchFuel", DeductAreaSwitchFuel);
    }
    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            currentFuel -= fuelConsumptionRate;
            PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
            if (currentFuel <= 0)
            {
                FuelEmpty();
            }
        }
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
    private void FuelEmpty()
    {
        currentFuel = 0;
        EventMessenger.TriggerEvent("CanVehicleMoveFalse");
        AudioManager.StopSound("Vehicle Engine");
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
    private void AddFuel()
    {
        maxFuel += fuelAddAmount;
        currentFuel += fuelAddAmount;
        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
        PrimitiveMessenger.EditObject("maxVehicleFuel", maxFuel);
    }
    public void SetFuel(float amount, bool doSetMax)
    {
        if (doSetMax)
        {
            maxFuel = amount;
            PrimitiveMessenger.EditObject("maxVehicleFuel", maxFuel);
        }
        currentFuel = amount;
        PrimitiveMessenger.EditObject("currentVehicleFuel", currentFuel);
    }
}
