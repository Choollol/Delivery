using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour, IBoundable
{
    protected float width;
    protected float height;

    protected float[] bounds = new float[4]; //Left, Right, Bottom, Top
    public float[] Bounds { get { return bounds; } protected set { } }

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        UpdateDimensions();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        bounds = BoundsManager.GetBounds();
    }

    void Update()
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
        if (transform.position.y - 0.001f < bounds[2])
        {
            transform.SetY(bounds[2] + 0.001f);
        }
        else if (transform.position.y + height - 0.001f > bounds[3])
        {
            transform.SetY(bounds[3] - height + 0.001f);
        }
    }
    public void SetBounds(float[] newBounds)
    {
        bounds = newBounds;
        /*bounds[0] = left;
        bounds[1] = right;
        bounds[2] = bottom;
        bounds[3] = top;*/
    }
    public void UpdateDimensions()
    {
        StartCoroutine(HandleUpdateDimensions());
    }
    private IEnumerator HandleUpdateDimensions()
    {
        yield return new WaitForEndOfFrame();
        width = spriteRenderer.sprite.textureRect.width / spriteRenderer.sprite.pixelsPerUnit;
        height = spriteRenderer.sprite.textureRect.height / spriteRenderer.sprite.pixelsPerUnit;
        yield break;
    }
}
