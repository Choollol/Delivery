using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMovement : MonoBehaviour
{
    public float speed;

    protected Rigidbody2D rb;
    protected InputController inputController;

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
}
