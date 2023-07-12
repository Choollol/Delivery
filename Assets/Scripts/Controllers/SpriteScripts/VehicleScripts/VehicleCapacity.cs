using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCapacity : MonoBehaviour
{
    [SerializeField] private int maxCapacity;
    private void Start()
    {
        CapacityManager.maxCapacity = maxCapacity;
        PrimitiveMessenger.EditObject("maxCapacity", maxCapacity);
    }
}
