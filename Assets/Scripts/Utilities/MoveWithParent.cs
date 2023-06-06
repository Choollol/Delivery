using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithParent : MonoBehaviour
{
    private Vector2 offset;
    void Start()
    {
        offset = transform.localPosition;
    }

    void Update()
    {
        transform.position = transform.parent.position + offset.ToVector3(0);
    }
}
