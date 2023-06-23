using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPUtil : MonoBehaviour
{
    [SerializeField] private string eventName;
    [SerializeField] private string valueName;

    private TextMeshProUGUI tmp;
    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        EventMessenger.StartListening(eventName, UpdateText);
        PrimitiveMessenger.AddObject(valueName, tmp.text);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening(eventName, UpdateText);
        PrimitiveMessenger.RemoveObject(valueName);
    }
    private void UpdateText()
    {
        tmp.text = PrimitiveMessenger.GetObject(valueName);
    }
}
