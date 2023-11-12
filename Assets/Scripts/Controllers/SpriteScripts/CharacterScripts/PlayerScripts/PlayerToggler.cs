using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToggler : MonoBehaviour
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

        EventMessenger.StartListening("RotatePillars", PillarRotateStart);
        EventMessenger.StartListening("RotatePillarsEnd", PillarRotateEnd);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("EnterVehicle", EnterVehicle);
        EventMessenger.StopListening("ExitVehicle", ExitVehicle);
        PrimitiveMessenger.RemoveObject("vehiclePosition");

        EventMessenger.StopListening("RotatePillars", PillarRotateStart);
        EventMessenger.StopListening("RotatePillarsEnd", PillarRotateEnd);
    }
    void Start()
    {
        inputController = GetComponent<InputController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterMovement = GetComponent<CharacterMovement>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    private void PillarRotateStart()
    {
        boxCollider.enabled = false;
    }
    private void PillarRotateEnd()
    {
        boxCollider.enabled = true;
    }
    private void EnterVehicle()
    {
        inputController.inputType = InputController.InputType.None;
        inputController.enabled = false;
        spriteRenderer.enabled = false;
        characterMovement.enabled = false;
        boxCollider.enabled = false;
        animator.enabled = false;
    }
    private void ExitVehicle()
    {
        inputController.enabled = true;
        inputController.inputType = InputController.InputType.User;
        spriteRenderer.enabled = true;
        transform.position = PrimitiveMessenger.GetObject("vehiclePosition") + new Vector3(0, 0.1f);
        CameraMovement.SetFollowTarget(gameObject);
        characterMovement.enabled = true;
        boxCollider.enabled = true;
        animator.enabled = true;
    }
}
