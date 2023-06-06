using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinParticle : MonoBehaviour
{
    [SerializeField] private float initialSpeed;
    [SerializeField] private float initialSpeedRange;
    [SerializeField] private float floatingTime;
    [SerializeField] private float floatingTimeRange;
    [SerializeField] private float movingTime;

    private Camera mainCamera;
    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void OnEnable()
    {
        StartCoroutine(InitialForce());
    }
    private IEnumerator InitialForce()
    {
        Vector3 startingForce = new Vector2(Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized *
            Random.Range(initialSpeed - initialSpeedRange, initialSpeed + initialSpeedRange) / 100;
        float floatTime = Random.Range(floatingTime - floatingTimeRange, floatingTime + floatingTimeRange);
        float timer = floatTime;
        float maxDeltaTime = 0.002f;

        transform.position = mainCamera.transform.position.ToVector2();

        while (timer > 0.2f)
        {
            if (GameManager.isGameActive)
            {
                transform.position += startingForce * timer / floatTime;
                if (Time.deltaTime < maxDeltaTime)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer -= maxDeltaTime;
                }
            }
            yield return null;
        }
        StartCoroutine(MoveToTopLeft());
        yield break;
    }
    private IEnumerator MoveToTopLeft()
    {
        Vector2 targetPos = new Vector2(-mainCamera.orthographicSize * mainCamera.aspect + 0.1f, mainCamera.orthographicSize - 0.1f);
        Vector2 startPos = transform.localPosition;
        float timer = 0;
        while (timer < movingTime)
        {
            if (GameManager.isGameActive)
            {
                transform.localPosition = Vector2.Lerp(startPos, mainCamera.transform.position + targetPos.ToVector3(0), timer / movingTime);
                timer += Time.deltaTime;
            }
            yield return null;
        }
        CurrencyManager.Instance.IncrementCoins();
        gameObject.SetActive(false);
        yield break;
    }
}
