using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteractable : MonoBehaviour, IInteractable
{
    public bool isUnlocked;

    public float interactRange { get; private set; }

    private VehicleInputController inputController;

    [SerializeField] private GameObject player;
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
                player.gameObject.SetActive(false);
                if (PrimitiveMessenger.GetObject("currentVehicleFuel") > 0)
                {
                    inputController.type = InputController.InputType.User;
                }
                CameraMovement.SetFollowTarget(transform.parent.gameObject);
                GameManager.isPlayerInVehicle = true;
                GameManager.vehicleName = transform.parent.name;
                GetComponentInParent<VehicleAudioController>().PlayStartAudio();
                EventMessenger.TriggerEvent("EnableVehicleUI");
            }
            else if (GameManager.isPlayerInVehicle)
            {
                player.gameObject.SetActive(true);
                player.transform.position = transform.position;
                inputController.type = InputController.InputType.None;
                CameraMovement.SetFollowTarget(player.gameObject);
                GameManager.isPlayerInVehicle = false;
                GetComponentInParent<VehicleAudioController>().PlayExitAudio();
                EventMessenger.TriggerEvent("DisableVehicleUI");
            }
        }
    }
}
