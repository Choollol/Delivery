using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinfallManager : MonoBehaviour
{
    public static bool hasCompletedTutorial;

    public static bool isInGame { get; private set; }

    [SerializeField] private GameObject coinfallCoin;
    [SerializeField] private GameObject coinfallCanvas;

    [SerializeField] private GameObject coinfallLetter;
    [SerializeField] private GameObject coinfallLetterHolder;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private static float coinSpawnInterval = 3;
    private static float coinSpawnIntervalDecrementAmount = 0.001f;
    public static float coinSpeed = 30f;
    private static float coinSpeedIncrementAmount = 0.1f;

    private int score;
    private int lives = 3;
    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("isRightKeyPressed", false);
        PrimitiveMessenger.AddObject("alphanumerics", "0123456789abcdefghijklmnopqrstuvwxyz");
        EventMessenger.StartListening("LoseLife", LoseLife);
        EventMessenger.StartListening("AddScore", AddScore);

        PrimitiveMessenger.AddObject("NewCoinfallLetter", KeyCode.A);

        EventMessenger.StartListening("BeginCoinfall", BeginGame);
        EventMessenger.StartListening("CloseCoinfall", CloseCoinfall);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("isRightKeyPressed");
        PrimitiveMessenger.RemoveObject("alphanumerics");
        EventMessenger.StopListening("LoseLife", LoseLife);
        EventMessenger.StopListening("AddScore", AddScore);

        PrimitiveMessenger.RemoveObject("NewCoinfallLetter");

        EventMessenger.StopListening("BeginCoinfall", BeginGame);
        EventMessenger.StopListening("CloseCoinfall", CloseCoinfall);
    }
    void Start()
    {
        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        EventMessenger.TriggerEvent("FreezeCamera");
        PrimitiveMessenger.EditObject("cameraFrozenPosition", Vector2.zero);
        EventMessenger.TriggerEvent("DisableScreenUI");

        SceneManager.SetActiveScene(gameObject.scene);

        ObjectPoolManager.AddPool("CoinfallCoins", coinfallCoin, 50, coinfallCanvas.transform.Find("Coins Holder").gameObject);

        if (hasCompletedTutorial)
        {
            EventMessenger.TriggerEvent("SkipTutorial");
        }
    }
    private void FixedUpdate()
    {
        if (isInGame)
        {
            if (coinSpeed < 1000)
            {
                coinSpeed += coinSpeedIncrementAmount;
            }
            if (coinSpawnInterval > 0.2f)
            {
                coinSpawnInterval -= coinSpawnIntervalDecrementAmount;
            }
        }
    }
    private void LateUpdate()
    {
        if (isInGame && Input.anyKeyDown && PrimitiveMessenger.GetObject("alphanumerics").Contains(Input.inputString) && 
            Input.inputString != "")
        {
            if (PrimitiveMessenger.GetObject("isRightKeyPressed"))
            {
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

        ObjectPoolManager.PullFromPool("CoinfallCoins");
        Instantiate(coinfallLetter, coinfallLetterHolder.transform);

        PrimitiveMessenger.EditObject("NewCoinfallLetter",
            (KeyCode)(UnityEngine.Random.Range((int)Enum.Parse<KeyCode>("A"), (int)Enum.Parse<KeyCode>("Z") + 1)));

        if (isInGame)
        {
            StartCoroutine(SpawnCoin());
        }

        yield break;
    }
    private void LoseLife()
    {
        EventMessenger.TriggerEvent("LoseLife" + lives);
        lives--;
        if (lives == 0)
        {
            isInGame = false;
            StartCoroutine(EndGame());
        }
    }
    private void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }
    private void BeginGame()
    {
        StartCoroutine(HandleBeginGame());
    }
    private IEnumerator HandleBeginGame()
    {
        if (!hasCompletedTutorial)
        {
            hasCompletedTutorial = true;
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(3);
        }
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "Go!";
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(false);
        StartCoroutine(SpawnCoin());
        isInGame = true;
        yield break;
    }
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2);
        gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        gameOverText.text = "Final Score: " + score;
        yield return new WaitForSeconds(5);
        GameManager.Instance.CloseSceneWithTransition("Coinfall", "CloseCoinfall");
        yield break;
    }
    private void CloseCoinfall()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
        EventMessenger.TriggerEvent("EnableScreenUI");
        EventMessenger.TriggerEvent("SetPlayerCanActTrue");
        EventMessenger.TriggerEvent("UnfreezeCamera");
    }
}
