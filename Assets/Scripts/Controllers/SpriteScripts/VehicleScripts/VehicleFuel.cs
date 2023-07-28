using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleFuel : MonoBehaviour
{
    [SerializeField] private float maxFuel;

    private Rigidbody2D rb;

    [SerializeField] private float fuelAddAmount;

    /*private void OnEnable()
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
    }*/
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        VehicleManager.SetFuel(maxFuel, maxFuel);
        PrimitiveMessenger.EditObject("fuelAddAmount", fuelAddAmount);
    }
    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            VehicleManager.Drive();
        }
    }
    /*private void DeductAreaSwitchFuel()
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
    }*/
}
