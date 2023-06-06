using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    private string body;
    public override void Start()
    {
        base.Start();
        body = "Female";
        MenuManager.Instance.UpdateSwitchBodyText(body);
    }

    void Update()
    {
        if (GameManager.isGameActive && inputController.canAct)
        {
            DirectionUpdate();
            ActionUpdate();
        }

        animator.Play("Base Layer." + action + "." + spriteName + "_" + body + "_" + action + "_" + direction);
    }
    public void SwitchBody()
    {
        if (body == "Male")
        {
            body = "Female";
        }
        else
        {
            body = "Male";
        }
        MenuManager.Instance.UpdateSwitchBodyText(body);
    }
}
