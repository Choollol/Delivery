using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour, IBoundable
{
    private float[] bounds = new float[4]; //Left, Right, Bottom, Top
    public float[] Bounds { get; private set; }

    private Camera mainCamera;

    private float width
    {
        get { return mainCamera.orthographicSize * mainCamera.aspect * 2; }
    }
    private float height
    {
        get { return mainCamera.orthographicSize * 2; }
    }
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        BoundsUpdate();
    }
    public void BoundsUpdate()
    {
        if (transform.position.x - width / 2 < bounds[0])
        {
            transform.SetX(bounds[0] + width / 2);
        }
        else if (transform.position.x + width / 2 > bounds[1])
        {
            transform.SetX(bounds[1] - width / 2);
        }
        if (transform.position.y - height / 2 < bounds[2])
        {
            transform.SetY(bounds[2] + height / 2);
        }
        else if (transform.position.y + height / 2 > bounds[3])
        {
            transform.SetY(bounds[3] - height / 2);
        }
    }
    public void SetBounds(float[] newBounds)
    {
        bounds = newBounds;
    }
}
