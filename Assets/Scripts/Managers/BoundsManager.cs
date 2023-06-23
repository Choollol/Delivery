using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BoundsManager : MonoBehaviour
{
    private static BoundsManager instance;
    public static BoundsManager Instance
    {
        get { return instance; }
    }

    private static float[] bounds;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public static void SetBounds(float left, float right, float bottom, float top)
    {
        MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();
        bounds = new float[] { left, right, bottom, top };
        for (int i = 0; i < scripts.Length; i++)
        {
            if (scripts[i] is IBoundable)
            {
                (scripts[i] as IBoundable).SetBounds(bounds);
            }
        }
    }
    public static float[] GetBounds()
    {
        return bounds;
        /*MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < scripts.Length; i++)
        {
            if (scripts[i] is IBoundable)
            {
                return (scripts[i] as IBoundable).Bounds;
            }
        }
        return null;*/
    }
}
