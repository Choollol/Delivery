using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager instance;
    public static CurrencyManager Instance
    {
        get { return instance; }
    }

    public static int coins { get; private set; }

    [SerializeField] private GameObject amountFrames;
    [SerializeField] private TextMeshProUGUI amountText;

    [SerializeField] private GameObject coinParticle;

    [SerializeField] private GameObject coinParticleParent;

    [SerializeField] private int maxCoinParticles;

    private int maxAmount = 9999999;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }
    private void Start()
    {
        UpdateAmountUI();
        ObjectPoolManager.AddPool("CoinParticles", coinParticle, maxCoinParticles, coinParticleParent);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            SpawnCoins(50, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            DecreaseCoins(50);  
        }
    }
    public void SetCoins(int newCoins)
    {
        coins = newCoins;
        UpdateAmountUI();
    }
    public void SpawnCoins(int amount, float delay)
    {
        StartCoroutine(HandleSpawnCoins(amount, delay));
    }
    private IEnumerator HandleSpawnCoins(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < amount; i++)
        {
            if (ObjectPoolManager.IsObjectAvailable("CoinParticles"))
            {
                ObjectPoolManager.PullFromPool("CoinParticles");
                AudioManager.PlaySound("Coin Spawn Sound");
            }
        }
        yield break;
    }
    public void IncrementCoins()
    {
        coins++;
        if (coins > maxAmount)
        {
            coins = maxAmount;
        }
        AudioSource sound = AudioManager.GetSound("Coin Collect Sound");
        if (!sound.isPlaying || sound.time / sound.clip.length > 0.2f)
        {
            AudioManager.PlaySound("Coin Collect Sound", 0.98f, 1.02f);
        }
        UpdateAmountUI();
    }
    public void DecreaseCoins(int amount)
    {
        StartCoroutine(HandleDecreaseCoins(amount));
    }
    private IEnumerator HandleDecreaseCoins(int amount)
    {
        int counter = Mathf.Abs(amount);
        while (counter > 0)
        {
            coins--;
            counter--;
            if (coins < 0)
            {
                coins = 0;
                yield break;
            }
            UpdateAmountUI();
            yield return null;
        }
        yield break;
    }
    private void UpdateAmountUI()
    {
        int digitCount = coins.ToString().Length;
        for (int i = 0; i < digitCount; i++)
        {
            amountFrames.transform.GetChild(i).gameObject.SetActive(true);
        }
        if (digitCount < amountFrames.transform.childCount)
        {
            for (int i = digitCount; i < amountFrames.transform.childCount; i++)
            {
                amountFrames.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        amountText.text = coins.ToString();
    }
}
