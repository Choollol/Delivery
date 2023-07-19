using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CoinfallCoin : MonoBehaviour, IPointerDownHandler
{
    private static int lowerBound = -90;
    private static float sideBounds = 60;
    private static int startingY = 97;

    private KeyCode key;

    private float height;

    private RectTransform rt;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();

        height = GetComponent<RectTransform>().rect.height;
    }
    void OnEnable()
    {
        key = PrimitiveMessenger.GetObject("newCoinfallLetter");

        rt.localPosition = new Vector2(UnityEngine.Random.Range(-sideBounds, sideBounds), startingY);
    }
    void Update()
    {
        if (transform.position.y + height / 2 > lowerBound)
        {
            transform.position -= new Vector3(0, CoinfallManager.coinSpeed * Time.deltaTime);
        }
        else if (CoinfallManager.isInGame)
        {
            gameObject.SetActive(false);
            EventMessenger.TriggerEvent("DeleteLetter" + key);
            EventMessenger.TriggerEvent("LoseLife");
        }
        if (Input.GetKeyDown(key) && CoinfallManager.isInGame && !PrimitiveMessenger.GetObject("isRightKeyPressed"))
        {
            PrimitiveMessenger.EditObject("isRightKeyPressed", true);
            StartCoroutine(Disappear());
        }
    }
    private IEnumerator Disappear()
    {
        AudioManager.PlaySound("Coinfall Collect Sound");
        PrimitiveMessenger.EditObject("doDeleteLetter", true);
        EventMessenger.TriggerEvent("DeleteLetter" + key);
        EventMessenger.TriggerEvent("AddScore");
        gameObject.SetActive(false);
        yield break;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (CoinfallManager.isInGame)
        {
            PrimitiveMessenger.EditObject("isCoinClicked", true);
            StartCoroutine(Disappear());
        }
    }
}
