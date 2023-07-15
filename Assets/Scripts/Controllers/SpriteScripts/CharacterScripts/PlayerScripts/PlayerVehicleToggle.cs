using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleToggle : MonoBehaviour
{
    private InputController inputController;
    private PlayerBounds bounds;
    private SpriteRenderer spriteRenderer;
    private void OnEnable()
    {
        EventMessenger.StartListening("EnterVehicle", EnterVehicle);
        EventMessenger.StartListening("ExitVehicle", ExitVehicle);
        PrimitiveMessenger.AddObject("vehiclePosition", Vector3.zero);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("EnterVehicle", EnterVehicle);
        EventMessenger.StopListening("ExitVehicle", ExitVehicle);
        PrimitiveMessenger.RemoveObject("vehiclePosition");
    }
    void Start()
    {
        inputController = GetComponent<InputController>();
        bounds = GetComponent<PlayerBounds>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void EnterVehicle()
    {
        inputController.type = InputController.InputType.None;
        bounds.enabled = false;
        spriteRenderer.enabled = false;
        transform.position += new Vector3(100, 100);
    }
    private void ExitVehicle()
    {
        inputController.type = InputController.InputType.User;
        bounds.enabled = true;
        spriteRenderer.enabled = true;
        transform.position = PrimitiveMessenger.GetObject("vehiclePosition");
        CameraMovement.SetFollowTarget(gameObject);
    }
}
