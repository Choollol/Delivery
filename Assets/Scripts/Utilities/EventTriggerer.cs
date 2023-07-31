using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerer : MonoBehaviour
{
    [SerializeField] private string[] eventsToTriggerOnAwake;
    [SerializeField] private string[] eventsToTriggerOnStart;
    [SerializeField] private string[] eventsToTriggerOnLoad;

    private static bool doTriggerOnLoad = true;
    public void TriggerEvent(string eventName)
    {
        EventMessenger.TriggerEvent(eventName);
    }
    private void Awake()
    {
        if (eventsToTriggerOnAwake != null && eventsToTriggerOnAwake.Length > 0)
        {
            foreach (string eventName in eventsToTriggerOnAwake)
            {
                EventMessenger.TriggerEvent(eventName);
            }
        }
    }
    private void Start()
    {
        if (eventsToTriggerOnStart != null && eventsToTriggerOnStart.Length > 0)
        {
            foreach (string eventName in eventsToTriggerOnStart)
            {
                EventMessenger.TriggerEvent(eventName);
            }
        }
        if (doTriggerOnLoad && eventsToTriggerOnLoad != null && eventsToTriggerOnLoad.Length > 0)
        {
            foreach (string eventName in eventsToTriggerOnLoad)
            {
                EventMessenger.TriggerEvent(eventName);
            }
            doTriggerOnLoad = false;
        }
    }
}
