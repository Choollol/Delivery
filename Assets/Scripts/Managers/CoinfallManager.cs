using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class CoinfallManager : MonoBehaviour
{
    private static bool isInGame;

    [SerializeField] private GameObject coinfallCoin;
    [SerializeField] private GameObject coinfallCanvas;

    private int score;
    private static float coinSpawnInterval = 3;
    private static float coinSpawnIntervalDecrementAmount = 0.1f;
    public static float coinSpeed = 30f;
    private static float coinSpeedIncrementAmount = 0.1f;

    private int lives;
    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("isRightKeyPressed", false);
        PrimitiveMessenger.AddObject("alphanumerics", "0123456789abcdefghijklmnopqrstuvwxyz");
        EventMessenger.StartListening("LoseLife", LoseLife);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("isRightKeyPressed");
        PrimitiveMessenger.RemoveObject("alphanumerics");
        EventMessenger.StopListening("LoseLife", LoseLife);
    }
    void Start()
    {
        EventMessenger.TriggerEvent("freezeCamera");
        PrimitiveMessenger.EditObject("cameraFrozenPosition", Vector2.zero);
        EventMessenger.TriggerEvent("DisableScreenUI");

        SceneManager.SetActiveScene(gameObject.scene);

        StartCoroutine(BeginCoinfall());
    }
    private void FixedUpdate()
    {
        if (isInGame)
        {
            coinSpeed += coinSpeedIncrementAmount;
        }
    }
    private void LateUpdate()
    {
        if (Input.anyKeyDown && PrimitiveMessenger.GetObject("alphanumerics").Contains(Input.inputString) && Input.inputString != "")
        {
            if (PrimitiveMessenger.GetObject("isRightKeyPressed"))
            {
                score++;
                PrimitiveMessenger.EditObject("isRightKeyPressed", false);
            }
            else
            {
                LoseLife();
            }
        }
    }
    private IEnumerator SpawnCoin()
    {
        yield return new WaitForSeconds(coinSpawnInterval);
        Instantiate(coinfallCoin, coinfallCanvas.transform);
        if (coinSpawnInterval > 0.2f)
        {
            coinSpawnInterval -= coinSpawnIntervalDecrementAmount;
        }
        StartCoroutine(SpawnCoin());
        yield break;
    }
    private void LoseLife()
    {

    }

    private IEnumerator BeginCoinfall()
    {
        StartCoroutine(SpawnCoin());
        isInGame = true;
        yield break;
    }
    private void EndCoinfall()
    {

    }
    private void CloseCoinfall()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
        SceneManager.UnloadSceneAsync(gameObject.scene);
        EventMessenger.TriggerEvent("EnableScreenUI");
    }
}
