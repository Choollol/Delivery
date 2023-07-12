using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPUtil : MonoBehaviour
{
    [SerializeField] private string textUpdateEventName;
    [SerializeField] private bool doAddValueName;
    [SerializeField] private string valueName;
    [SerializeField] private string textPrefix;

    private TextMeshProUGUI tmp;
    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        EventMessenger.StartListening(textUpdateEventName, UpdateText);
        if (doAddValueName)
        {
            PrimitiveMessenger.AddObject(valueName, tmp.text);
        }

        UpdateText();
    }
    private void OnDisable()
    {
        EventMessenger.StopListening(textUpdateEventName, UpdateText);
        if (doAddValueName)
        {
            PrimitiveMessenger.RemoveObject(valueName);
        }
    }
    private void UpdateText()
    {
        tmp.text = textPrefix + PrimitiveMessenger.GetObject(valueName);
    }
}
