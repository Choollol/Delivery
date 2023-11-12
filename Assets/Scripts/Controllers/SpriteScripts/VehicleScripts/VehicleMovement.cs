using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : SpriteMovement
{
    [SerializeField] private float speedAddAmount = 0.1f;
    private void OnEnable()
    {
        EventMessenger.StartListening("CanVehicleMoveTrue", SetCanMoveTrue);
        EventMessenger.StartListening("CanVehicleMoveFalse", SetCanMoveFalse);
        EventMessenger.StartListening("AddVehicleSpeed", AddSpeed);
        PrimitiveMessenger.AddObject("vehicleSpeed", speed);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("CanVehicleMoveTrue", SetCanMoveTrue);
        EventMessenger.StopListening("CanVehicleMoveFalse", SetCanMoveFalse);
        EventMessenger.StopListening("AddVehicleSpeed", AddSpeed);
        PrimitiveMessenger.RemoveObject("vehicleSpeed");
    }
    private void Update()
    {
        if (inputController.inputType == InputController.InputType.None)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        rb.constraints = rb.constraints | RigidbodyConstraints2D.FreezeRotation;
    }
    protected override void SetCanMoveTrue()
    {
        base.SetCanMoveTrue();
        inputController.inputType = InputController.InputType.User;
    }
    protected override void SetCanMoveFalse()
    {
        base.SetCanMoveFalse();
        inputController.inputType = InputController.InputType.None;
    }
    private void AddSpeed()
    {
        speed += speedAddAmount;
        PrimitiveMessenger.EditObject("vehicleSpeed", speed);
    }
    public void SetSpeed(float amount)
    {
        speed = amount;
        PrimitiveMessenger.EditObject("vehicleSpeed", speed);
    }
}
