using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinfallManager : MonoBehaviour
{
    private static string coinfallKeys = "abcdefghijklmnopqrstuvwxyz";

    public static bool hasCompletedTutorial;
    public static bool isInGame { get; private set; }

    [SerializeField] private GameObject coinfallCoin;
    [SerializeField] private GameObject coinfallCanvas;

    [SerializeField] private GameObject coinfallLetter;
    [SerializeField] private GameObject coinfallLetterHolder;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private static float coinSpawnInterval;
    private static float coinSpawnIntervalDecrementAmount = 0.001f;
    public static float coinSpeed;
    private static float coinSpeedIncrementAmount = 0.1f;

    private int score;
    private int lives = 3;
    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("isRightKeyPressed", false);
        PrimitiveMessenger.AddObject("isCoinClicked", false);

        EventMessenger.StartListening("LoseLife", LoseLife);
        EventMessenger.StartListening("AddScore", AddScore);

        PrimitiveMessenger.AddObject("newCoinfallLetter", KeyCode.A);

        PrimitiveMessenger.AddObject("doDeleteLetter", false);

        EventMessenger.StartListening("BeginCoinfall", BeginGame);
        EventMessenger.StartListening("CloseCoinfall", CloseCoinfall);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("isRightKeyPressed");
        PrimitiveMessenger.RemoveObject("isCoinClicked");

        EventMessenger.StopListening("LoseLife", LoseLife);
        EventMessenger.StopListening("AddScore", AddScore);

        PrimitiveMessenger.RemoveObject("newCoinfallLetter");

        PrimitiveMessenger.RemoveObject("doDeleteLetter");

        EventMessenger.StopListening("BeginCoinfall", BeginGame);
        EventMessenger.StopListening("CloseCoinfall", CloseCoinfall);
    }
    void Start()
    {
        GameManager.isInWorld = false;

        EventMessenger.TriggerEvent("SetPlayerCanActFalse");
        EventMessenger.TriggerEvent("FreezeCamera");
        PrimitiveMessenger.EditObject("cameraFrozenPosition", Vector2.zero);
        EventMessenger.TriggerEvent("DisableScreenUI");

        SceneManager.SetActiveScene(gameObject.scene);

        ObjectPoolManager.AddPool("CoinfallCoins", coinfallCoin, 50, coinfallCanvas.transform.Find("Coins Holder").gameObject);

        coinSpawnInterval = 3;
        coinSpeed = 30f;

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
            if (coinSpawnInterval > 0.5f)
            {
                coinSpawnInterval -= coinSpawnIntervalDecrementAmount;
            }
            else if (coinSpawnInterval > 0.2f)
            {
                coinSpawnInterval -= coinSpawnIntervalDecrementAmount / 5;
            }
        }
    }
    private void LateUpdate()
    {
        if (isInGame && Input.anyKeyDown && GameManager.isGameActive)
        {
            if (coinfallKeys.Contains(Input.inputString) && Input.inputString != "")
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
            else if (Input.GetMouseButtonDown(0))
            {
                if (PrimitiveMessenger.GetObject("isCoinClicked"))
                {
                    PrimitiveMessenger.EditObject("isCoinClicked", false);
                }
                else
                {
                    LoseLife();
                }
            }
        }
    }
    private IEnumerator SpawnCoin()
    {
        yield return new WaitForSeconds(coinSpawnInterval);

        ObjectPoolManager.PullFromPool("CoinfallCoins");
        Instantiate(coinfallLetter, coinfallLetterHolder.transform);

        PrimitiveMessenger.EditObject("newCoinfallLetter",
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
        AudioManager.PlaySound("Coinfall Life Lost Sound");
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
        AudioManager.PlaySound("Coinfall Countdown Sound");
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        AudioManager.PlaySound("Coinfall Countdown Sound");
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        AudioManager.PlaySound("Coinfall Countdown Sound");
        yield return new WaitForSeconds(1);
        countdownText.text = "Go!";
        AudioManager.PlaySound("Coinfall Countdown End Sound");
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(false);
        StartCoroutine(SpawnCoin());
        isInGame = true;
        AudioManager.PlaySound("Coinfall Theme");
        yield break;
    }
    private IEnumerator EndGame()
    {
        StartCoroutine(AudioManager.FadeAudio("Coinfall Theme", 0.5f, 0));
        yield return new WaitForSeconds(2);
        gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        gameOverText.text = "Final Score: " + score;
        yield return new WaitForSeconds(2);
        ObjectPoolManager.RemovePoolKey("CoinfallCoins");
        GameManager.Instance.CloseSceneWithTransition("Coinfall", "CloseCoinfall");
        yield break;
    }
    private void CloseCoinfall()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
        EventMessenger.TriggerEvent("EnableScreenUI");
        EventMessenger.TriggerEvent("SetPlayerCanActTrue");
        EventMessenger.TriggerEvent("UnfreezeCamera");

        int coins = 0;
        if (score < 20)
        {
            coins = (int)(PrimitiveMessenger.GetObject("CoinfallBaseAmount") * ((float)score / 6 + 2));
        }
        else if (score < 60)
        {
            coins = (int)(PrimitiveMessenger.GetObject("CoinfallBaseAmount") * ((float)score / 8 + 4));
        }
        else
        {
            coins = (int)(PrimitiveMessenger.GetObject("CoinfallBaseAmount") * ((float)score / 12 + 7));
        }
        CurrencyManager.Instance.SpawnCoins(coins, 1.5f);

        GameManager.isInWorld = true;
    }
}
