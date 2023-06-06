using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public enum InputType
    {
        None, User, Cutscene
    }

    protected float horizontalInput;
    public float HorizontalInput 
    { 
        get 
        { 
            return horizontalInput; 
        }
        private set { }
    }
    protected float verticalInput;
    public float VerticalInput
    {
        get
        {
            return verticalInput;
        }
        private set { }
    }

    public InputType type;

    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D boxCollider;

    public bool canAct { get; protected set; }
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        canAct = true;
    }
    public virtual void Update()
    {
        horizontalInput = 0;
        verticalInput = 0;
        if (canAct && GameManager.isGameActive)
        {
            if (type == InputType.User)
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
                verticalInput = Input.GetAxisRaw("Vertical");
            }
        }
    }
    protected virtual void OnInteract()
    {

    }
    protected void SetCanActTrue()
    {
        canAct = true;
    }
    protected void SetCanActFalse()
    {
        canAct = false;
        GetComponent<SpriteAnimator>().SetAction(SpriteAnimator.Action.Idle);
    }
}
