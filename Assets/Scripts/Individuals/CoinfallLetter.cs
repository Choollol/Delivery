using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinfallLetter : MonoBehaviour
{
    private KeyCode id;
    private void Awake()
    {
        id = PrimitiveMessenger.GetObject("NewCoinfallLetter");
        GetComponent<TextMeshProUGUI>().text = id.ToString();
    }

    private void OnEnable()
    {
        EventMessenger.StartListening("DeleteLetter" + id, DeleteLetter);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("DeleteLetter" + id, DeleteLetter);
    }
    private void DeleteLetter()
    {
        Destroy(gameObject);
    }
}
