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
            if (inputController.type == InputController.InputType.None)
            {
                player.gameObject.SetActive(false);
                inputController.type = InputController.InputType.User;
                CameraMovement.Instance.SetFollowTarget(CameraMovement.FollowTarget.Vehicle, transform.parent.gameObject);
                GameManager.Instance.isPlayerInVehicle = true;
                GameManager.Instance.vehicleName = transform.parent.name;
                GetComponentInParent<VehicleAudioController>().PlayStartAudio();
            }
            else if (inputController.type == InputController.InputType.User)
            {
                player.gameObject.SetActive(true);
                player.transform.position = transform.position;
                inputController.type = InputController.InputType.None;
                CameraMovement.Instance.SetFollowTarget(CameraMovement.FollowTarget.Player, player.gameObject);
                GameManager.Instance.isPlayerInVehicle = false;
                GetComponentInParent<VehicleAudioController>().PlayExitAudio();
            }
        }
    }
}
