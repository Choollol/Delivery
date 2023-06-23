using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInputController : InputController
{
    public override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Interact") && GameManager.isPlayerInVehicle)
        {
            InteractionManager.Instance.Interact(transform.GetChild(0).position);
        }
    }
}
