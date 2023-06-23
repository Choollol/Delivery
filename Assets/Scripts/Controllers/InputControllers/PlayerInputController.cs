using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : InputController
{
    private void OnEnable()
    {
        EventMessenger.StartListening("SetPlayerCanActTrue", SetCanActTrue);
        EventMessenger.StartListening("SetPlayerCanActFalse", SetCanActFalse);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("SetPlayerCanActTrue", SetCanActTrue);
        EventMessenger.StopListening("SetPlayerCanActFalse", SetCanActFalse);
    }
    public override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Interact") && canAct)
        {
            InteractionManager.Instance.Interact(transform.position);
        }
    }
    /*protected override void OnInteract()
    {
        if (GameManager.currentArea == GameManager.Area.Overworld)
        {
            if (Vector2.Distance(transform.position, GameObject.Find("Shop Building").transform.position) < 0.1f)
            {
                GameManager.OpenShop();
                canAct = false;
                EventManager.StartListening("canActTrue", SetCanActTrue);
                GetComponent<SpriteAnimator>().SetAction(SpriteAnimator.Action.Idle);
            }
        }
    }*/
    
}
