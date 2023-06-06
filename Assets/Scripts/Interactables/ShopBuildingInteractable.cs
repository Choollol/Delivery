using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuildingInteractable : MonoBehaviour, IInteractable
{
    public float interactRange { get; private set; }
    void Start()
    {
        interactRange = 0.1f;
    }

    void Update()
    {
        
    }
    public void OnInteract()
    {
        GameManager.OpenShop();
    }
}
