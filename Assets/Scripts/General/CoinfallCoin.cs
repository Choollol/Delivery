using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinfallCoin : MonoBehaviour
{
    private static int lowerBound = -90;
    private static float sideBounds = 60;

    private KeyCode key;

    private float height;

    private RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();

        key = (KeyCode)(UnityEngine.Random.Range((int)Enum.Parse<KeyCode>("A"), (int)Enum.Parse<KeyCode>("Z") + 1));
        Debug.Log(key);

        height = GetComponent<RectTransform>().rect.height;

        rt.localPosition = new Vector2(UnityEngine.Random.Range(-sideBounds, sideBounds), rt.localPosition.y);
    }
    private void FixedUpdate()
    {
        if (transform.position.y + height / 2 > lowerBound)
        {
            transform.position -= new Vector3(0, CoinfallManager.coinSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            PrimitiveMessenger.EditObject("isRightKeyPressed", true);
            StartCoroutine(OnThisKeyPressed());
        }
    }
    private IEnumerator OnThisKeyPressed()
    {
        Destroy(gameObject);
        yield break;
    }
}
