using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : MonoBehaviour, IInteractable
{
    private int id;

    public float interactRange { get; private set; }
    private void OnEnable()
    {
        //EventMessenger.StartListening("ResetPOIIDs", ResetIDs);

        EventMessenger.StartListening("UpdatePOIIndicators", UpdatePOIIndicator);
    }
    private void OnDisable()
    {
        //EventMessenger.StopListening("ResetPOIIDs", ResetIDs);

        EventMessenger.StopListening("UpdatePOIIndicators", UpdatePOIIndicator);
    }
    private void Awake()
    {
        id = POIManager.ID;
        POIManager.ID++;

        interactRange = 0.1f;
        Debug.Log(id);
    }
    public void OnInteract()
    {
        Debug.Log("order " + id + ": " + POIManager.poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, id)]);
    }
    private void UpdatePOIIndicator()
    {
        if (POIManager.poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, id)] > 0 &&
            ObjectPoolManager.GetPooledObject("POIIndicators", transform.position) == null)
        {
            POIManager.AddPOIIndicator(transform.position);
        }
        else if (ObjectPoolManager.GetPooledObject("POIIndicators", transform.position) != null)
        {
            ObjectPoolManager.GetPooledObject("POIIndicators", transform.position).SetActive(false);
        }
    }
}
