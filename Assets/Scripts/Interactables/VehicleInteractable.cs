using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteractable : MonoBehaviour, IInteractable
{
    public bool isUnlocked;

    public float interactRange { get; private set; }

    private VehicleInputController inputController;
    void Start()
    {
        inputController = GetComponentInParent<VehicleInputController>();

        interactRange = 0.4f;
    }
    public void OnInteract()
    {
        if (isUnlocked && GameManager.isGameActive)
        {
            if (!GameManager.isPlayerInVehicle)
            {
                EventMessenger.TriggerEvent("EnterVehicle");
                if (PrimitiveMessenger.GetObject("currentVehicleFuel") > 0)
                {
                    inputController.inputType = InputController.InputType.User;
                }
                CameraMovement.SetFollowTarget(transform.parent.gameObject);
                GameManager.isPlayerInVehicle = true;
                GameManager.vehicleName = transform.parent.name;
                GetComponentInParent<VehicleAudioController>().PlayStartAudio();
                EventMessenger.TriggerEvent("EnableVehicleUI");
            }
            else if (GameManager.isPlayerInVehicle)
            {
                PrimitiveMessenger.EditObject("vehiclePosition", transform.position);
                EventMessenger.TriggerEvent("ExitVehicle");
                inputController.inputType = InputController.InputType.None;
                GameManager.isPlayerInVehicle = false;
                GetComponentInParent<VehicleAudioController>().PlayExitAudio();
                EventMessenger.TriggerEvent("DisableVehicleUI");
            }
        }
    }
}
