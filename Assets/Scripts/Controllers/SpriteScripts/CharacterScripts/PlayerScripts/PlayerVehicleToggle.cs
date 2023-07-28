using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicleToggle : MonoBehaviour
{
    private InputController inputController;
    private SpriteRenderer spriteRenderer;
    private CharacterMovement characterMovement;
    private BoxCollider2D boxCollider;
    private Animator animator;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterMovement = GetComponent<CharacterMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void EnterVehicle()
    {
        inputController.type = InputController.InputType.None;
        inputController.enabled = false;
        spriteRenderer.enabled = false;
        characterMovement.enabled = false;
        boxCollider.enabled = false;
        animator.enabled = false;
    }
    private void ExitVehicle()
    {
        inputController.enabled = true;
        inputController.type = InputController.InputType.User;
        spriteRenderer.enabled = true;
        transform.position = PrimitiveMessenger.GetObject("vehiclePosition") + new Vector3(0, 0.1f);
        CameraMovement.SetFollowTarget(gameObject);
        characterMovement.enabled = true;
        boxCollider.enabled = true;
        animator.enabled = true;
    }
}
