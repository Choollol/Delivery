using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public enum Direction
    {
        Down, Up, Left, Right
    }
    public enum Action
    {
        Idle, Walk, Drive
    }
    public string spriteName;

    public Direction direction { get; protected set; }
    protected Direction directionOld;
    protected Action action;

    protected Animator animator;
    protected InputController inputController;
    protected BoxCollider2D boxCollider;
    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        inputController = GetComponent<InputController>();
        boxCollider = GetComponent<BoxCollider2D>();

        direction = Direction.Down;
    }

    void Update()
    {
        if (GameManager.isGameActive && inputController.canAct)
        {
            DirectionUpdate();
            ActionUpdate();
        }

        animator.Play("Base Layer." + action + "." + spriteName + "_" + action + "_" + direction);
    }
    protected virtual void DirectionUpdate()
    {
        if (inputController.HorizontalInput < 0)
        {
            direction = Direction.Left;
        }
        else if (inputController.HorizontalInput > 0)
        {
            direction = Direction.Right;
        }
        else if (inputController.VerticalInput < 0)
        {
            direction = Direction.Down;
        }
        else if (inputController.VerticalInput > 0)
        {
            direction = Direction.Up;
        }

        if (direction != directionOld)
        {
            OnDirectionChange();
        }

        directionOld = direction;
    }
    protected virtual void OnDirectionChange()
    {

    }
    protected virtual void ActionUpdate()
    {
        
    }
    public void SetAction(Action newAction)
    {
        action = newAction;
    }
}
