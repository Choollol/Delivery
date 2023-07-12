using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CapacityText : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private void OnEnable()
    {
        EventMessenger.StartListening("UpdateCapacityText", UpdateCapacityText);

        UpdateCapacityText();
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("UpdateCapacityText", UpdateCapacityText);
    }
    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateCapacityText()
    {
        tmp.text = "CAP: " + PrimitiveMessenger.GetObject("capacityInUse") + "/" +
            PrimitiveMessenger.GetObject("maxCapacity");
    }
}
