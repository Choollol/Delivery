using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] public float interactRange { get; protected set; }
    [SerializeField] private float InteractRange;
    [SerializeField] private string interactEventName;
    private void Start()
    {
        interactRange = InteractRange;
    }
    public void OnInteract()
    {
        EventMessenger.TriggerEvent(interactEventName);
    }
}
