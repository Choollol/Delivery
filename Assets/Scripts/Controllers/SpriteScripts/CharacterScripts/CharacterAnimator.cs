using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : SpriteAnimator
{
    protected override void ActionUpdate()
    {
        if (inputController.VerticalInput == 0 && inputController.HorizontalInput == 0)
        {
            action = Action.Idle;
        }
        else
        {
            action = Action.Walk;
        }
    }
}
