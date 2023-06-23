using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VehicleData : ScriptableObject
{
    public Vector2 colliderDimensionsUp;
    public float upColliderYOffset;

    public Vector2 colliderDimensionsDown;
    public float downColliderYOffset;

    public Vector2 colliderDimensionsHorizontal;
    public float horizontalColliderYOffset;

    public Vector2 interactableOffsetDown;
    public Vector2 interactableOffsetUp;
    public Vector2 interactableOffsetLeft;
    public Vector2 interactableOffsetRight;

    public int capacity;
}
