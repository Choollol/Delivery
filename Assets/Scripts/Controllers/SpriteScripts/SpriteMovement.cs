using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovement : MonoBehaviour
{
    public float speed;

    protected Rigidbody2D rb;
    protected InputController inputController;

    protected bool canMove;
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputController = GetComponent<InputController>();
    }
    private void FixedUpdate()
    {
        MoveUpdate();
    }
    private void MoveUpdate()
    {
        rb.velocity = new Vector2(inputController.HorizontalInput, inputController.VerticalInput).normalized * speed;
    }
    protected virtual void SetCanMoveTrue()
    {
        canMove = true;
    }
    protected virtual void SetCanMoveFalse()
    {
        canMove = false;
    }
}
