using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerer : MonoBehaviour
{
    /*[SerializeField] private string[] eventsToTriggerOnAwake;
    [SerializeField] private string[] eventsToTriggerOnStart;*/
    public void TriggerEvent(string eventName)
    {
        EventMessenger.TriggerEvent(eventName);
    }
    /*private void Awake()
    {
        foreach (string eventName in eventsToTriggerOnAwake)
        {
            EventMessenger.TriggerEvent(eventName);
        }
    }
    private void Start()
    {
        foreach (string eventName in eventsToTriggerOnStart)
        {
            EventMessenger.TriggerEvent(eventName);
        }
    }*/
}
