using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAnimator : SpriteAnimator
{
    [SerializeField] private VehicleData vehicleData;

    private PlayerBounds vehicleBounds;

    private GameObject vehicleInteractable;
    public override void Start()
    {
        base.Start();
        vehicleBounds = GetComponent<PlayerBounds>();
        vehicleInteractable = transform.GetChild(0).gameObject;
    }
    protected override void OnDirectionChange()
    {
        if (direction == Direction.Down) 
        {
            boxCollider.size = vehicleData.colliderDimensionsDown;
            boxCollider.offset = new Vector2(boxCollider.offset.x, vehicleData.downColliderYOffset);
            vehicleInteractable.transform.localPosition = vehicleData.interactableOffsetDown;
        }
        else if (direction == Direction.Up)
        {
            boxCollider.size = vehicleData.colliderDimensionsUp;
            boxCollider.offset = new Vector2(boxCollider.offset.x, vehicleData.upColliderYOffset);
            vehicleInteractable.transform.localPosition = vehicleData.interactableOffsetUp;
        }
        else if (direction == Direction.Left)
        {
            boxCollider.size = vehicleData.colliderDimensionsHorizontal;
            boxCollider.offset = new Vector2(boxCollider.offset.x, vehicleData.horizontalColliderYOffset);
            vehicleInteractable.transform.localPosition = vehicleData.interactableOffsetLeft;
        }
        else if (direction == Direction.Right)
        {
            boxCollider.size = vehicleData.colliderDimensionsHorizontal;
            boxCollider.offset = new Vector2(boxCollider.offset.x, vehicleData.horizontalColliderYOffset);
            vehicleInteractable.transform.localPosition = vehicleData.interactableOffsetRight;

        }
        vehicleBounds.UpdateDimensions();
    }
    protected override void ActionUpdate()
    {
        if (inputController.VerticalInput == 0 && inputController.HorizontalInput == 0)
        {
            action = Action.Idle;
        }
        else
        {
            action = Action.Drive;
        }
    }
    protected override void DirectionUpdate()
    {
        if (PrimitiveMessenger.GetObject("currentVehicleFuel") <= 0)
        {
            return;
        }
        base.DirectionUpdate();
    }
}
