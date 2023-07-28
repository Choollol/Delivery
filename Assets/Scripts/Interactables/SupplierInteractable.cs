using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SupplierInteractable : MonoBehaviour, IInteractable
{
    private int id;
    public float interactRange { get; private set; }
    private void OnEnable()
    {
        EventMessenger.StartListening("UpdatePOIIndicators", UpdatePOIIndicator);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("UpdatePOIIndicators", UpdatePOIIndicator);
    }
    private void Awake()
    {
        id = POIManager.ID;
        POIManager.ID++;

        interactRange = 0.15f;
    }
    public void OnInteract()
    {
        if (POIManager.poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, id)] > 0)
        {
            POIManager.PickUpIngredients(GameManager.currentArea, id);
        }
    }
    private void UpdatePOIIndicator()
    {
        if (POIManager.poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, id)] > 0 &&
            ObjectPoolManager.GetPooledObject("POIIndicators", transform.position) == null)
        {
            POIManager.AddPOIIndicator(transform.position);
        }
        else if (POIManager.poiOrders[new KeyValuePair<GameManager.Area, int>(GameManager.currentArea, id)] == 0 &&
            ObjectPoolManager.GetPooledObject("POIIndicators", transform.position))
        {
            ObjectPoolManager.GetPooledObject("POIIndicators", transform.position).SetActive(false);
        }
    }
}
