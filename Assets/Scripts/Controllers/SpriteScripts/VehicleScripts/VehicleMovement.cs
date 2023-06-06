using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : SpriteMovement
{
    private void Update()
    {
        if (inputController.type == InputController.InputType.None)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        rb.constraints = rb.constraints | RigidbodyConstraints2D.FreezeRotation;
    }
}
