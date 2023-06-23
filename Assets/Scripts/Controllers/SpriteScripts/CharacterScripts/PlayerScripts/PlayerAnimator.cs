using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : CharacterAnimator
{
    private string body;
    private void Awake()
    {
        body = "Female";
    }
    public override void Start()
    {
        base.Start();
    }
    private void OnEnable()
    {
        EventMessenger.StartListening("SwitchPlayerBody", SwitchBody);
        PrimitiveMessenger.AddObject("playerBody", body);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("SwitchPlayerBody", SwitchBody);
        PrimitiveMessenger.RemoveObject("playerBody");
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
        PrimitiveMessenger.EditObject("playerBody", body);
        EventMessenger.TriggerEvent("UpdateSwitchBodyText");
    }
}
