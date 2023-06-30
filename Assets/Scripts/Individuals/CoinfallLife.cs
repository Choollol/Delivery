using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinfallLife : MonoBehaviour
{
    [SerializeField] private int lifeNumber;
    private void OnEnable()
    {
        EventMessenger.StartListening("LoseLife" + lifeNumber, LifeLost);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("LoseLife" + lifeNumber, LifeLost);
    }
    private void LifeLost()
    {
        gameObject.SetActive(false);
    }
}
