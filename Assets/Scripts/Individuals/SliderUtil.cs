using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUtil : MonoBehaviour
{
    [SerializeField] private string setSliderEventName;
    [SerializeField] private string sliderValueName;
    [SerializeField] private string valueChangedEventName;

    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(SliderValueUpdate);
    }
    private void OnEnable()
    {
        EventMessenger.StartListening(setSliderEventName, SetSliderValue);
        PrimitiveMessenger.AddObject(sliderValueName, slider.value);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening(setSliderEventName, SetSliderValue);
        PrimitiveMessenger.RemoveObject(sliderValueName);
    }
    private void SetSliderValue()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        slider.value = PrimitiveMessenger.GetObject(sliderValueName);
    }
    private void SliderValueUpdate(float sliderValue)
    {
        PrimitiveMessenger.EditObject(sliderValueName, slider.value);
        EventMessenger.TriggerEvent(valueChangedEventName);
    }
}
