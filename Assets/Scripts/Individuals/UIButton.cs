using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public bool canChangeOpacity;

    private Image image;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        
    }
    public void SetOpacity(float opacity)
    {
        if (!canChangeOpacity)
        {
            return;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Image childImage = transform.GetChild(i).GetComponent<Image>();
                childImage.color = new Color(childImage.color.r, childImage.color.g, childImage.color.b, opacity);
            }
        }
    }
    private void OnMouseEnter()
    {
        if (gameObject.name.Split(" ")[1] == "Tab")
        {
            SetOpacity(0.7f);
        }
    }
    private void OnMouseExit()
    {
        if (gameObject.name.Split(" ")[1] == "Tab") 
        { 
            SetOpacity(0.5f);
        }
    }
}
