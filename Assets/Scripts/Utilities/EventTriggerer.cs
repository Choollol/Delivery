using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerer : MonoBehaviour
{
    public void TriggerEvent(string eventName)
    {
        EventMessenger.TriggerEvent(eventName);
    }
}
