using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    private static float angle = 0;

    private static float yOffset = -0.1f;

    public bool doRotate = false;

    private float currentAngle;
    private float radius = 1;
    private float radiusDecrement = 0.0001f;
    private float speed = 1;
    private float acceleration = 0.005f;
    private void Start()
    {
        currentAngle = angle + Mathf.PI / 2;
        angle += Mathf.PI / 5 * 2;
        transform.localPosition = new Vector3(radius * Mathf.Cos(currentAngle), radius * Mathf.Sin(currentAngle) + yOffset);
    }
    private void Update()
    {
        if (doRotate)
        {
            currentAngle += Time.deltaTime * speed;
            if (speed < 100)
            {
                speed += acceleration;
            }

            float newX = radius * Mathf.Cos(currentAngle);
            float newY = radius * Mathf.Sin(currentAngle);
            radius -= radiusDecrement;
            if (radiusDecrement > -100)
            {
                radiusDecrement += radiusDecrement / 3000;
            }

            transform.localPosition = new Vector3(newX, newY + yOffset, 0);

            if (Mathf.Abs(transform.position.x) > 10 && Mathf.Abs(transform.position.y) > 10)
            {
                doRotate = false;
                gameObject.SetActive(false);
                EventMessenger.TriggerEvent("RotatePillarsEnd");
            }
        }
    }
    
}
