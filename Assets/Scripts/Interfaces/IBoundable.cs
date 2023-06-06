using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoundable
{
    public float[] Bounds { get; }
    void BoundsUpdate();
    void SetBounds(float[] newBounds);
}
