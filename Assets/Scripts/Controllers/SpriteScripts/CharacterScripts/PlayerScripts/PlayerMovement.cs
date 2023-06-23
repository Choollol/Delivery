using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    public override void Start()
    {
        base.Start();
        CameraMovement.SetFollowTarget(gameObject);
    }
}
