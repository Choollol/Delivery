using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputController : InputController
{
    private void OnEnable()
    {
        EventManager.StartListening("setCanActTrue", SetCanActTrue);
        EventManager.StartListening("setCanActFalse", SetCanActFalse);
    }
    private void OnDisable()
    {
        EventManager.StopListening("setCanActTrue", SetCanActTrue);
        EventManager.StopListening("setCanActFalse", SetCanActFalse);
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
        if (GameManager.Instance.currentArea == GameManager.Area.Overworld)
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
