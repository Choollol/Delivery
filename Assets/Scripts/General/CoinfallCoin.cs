using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinfallCoin : MonoBehaviour
{
    private static int lowerBound = -90;
    private static float sideBounds = 60;
    private static int startingY = 97;

    private int id;

    private KeyCode key;

    private float height;

    private RectTransform rt;
    private Image image;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        height = GetComponent<RectTransform>().rect.height;
    }
    void OnEnable()
    {
        key = PrimitiveMessenger.GetObject("NewCoinfallLetter");

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
        if (Input.GetKeyDown(key) && CoinfallManager.isInGame)
        {
            PrimitiveMessenger.EditObject("isRightKeyPressed", true);
            StartCoroutine(OnThisKeyPressed());
        }
    }
    private IEnumerator OnThisKeyPressed()
    {
        AudioManager.PlaySound("Coinfall Collect Sound");
        EventMessenger.TriggerEvent("DeleteLetter" + key);
        EventMessenger.TriggerEvent("AddScore");
        gameObject.SetActive(false);
        yield break;
    }
}
