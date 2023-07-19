using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPOIPointer : MonoBehaviour
{
    private Image image;
    private void OnEnable()
    {
        EventMessenger.StartListening("DisableCurrentPOIPointer", Disable);
        EventMessenger.StartListening("EnableCurrentPOIPointer", Enable);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("DisableCurrentPOIPointer", Disable);
        EventMessenger.StopListening("EnableCurrentPOIPointer", Enable);
    }
    void Start()
    {
        image = GetComponent<Image>();
    }
    private void Disable()
    {
        image.color = new Color(1, 1, 1, 0.5f);
    }
    private void Enable()
    {
        image.color = new Color(1, 1, 1, 1f);
    }
}
