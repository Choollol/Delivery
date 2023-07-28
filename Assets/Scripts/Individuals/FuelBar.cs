using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    private static FuelBar instance;
    public static FuelBar Instance
    {
        get { return instance; }
    }

    private RectTransform rectTransform;
    private Image image;

    private float startingWidth;
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
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        startingWidth = rectTransform.sizeDelta.x;
        image = GetComponent<Image>();
    }
    private void Update()
    {
        UpdateBar(PrimitiveMessenger.GetObject("currentVehicleFuel") / PrimitiveMessenger.GetObject("maxVehicleFuel"));
    }
    public void UpdateBar(float amount)
    {
        rectTransform.sizeDelta = new Vector2(amount * startingWidth, rectTransform.sizeDelta.y);
        image.color = Color.HSVToRGB(amount * 0.3f, 1, 0.9f);
    }
}
