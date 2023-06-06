using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isUnlocked { get; private set; }
    public bool isInStock { get; private set; }

    [SerializeField] private int price;
    [SerializeField] private ShopManager.ItemType itemType;
    [SerializeField] private ShopManager.VehicleType vehicleType;
    [SerializeField] private int maxUpgradeAmount;
    [SerializeField] private string description;
    [SerializeField] private float descriptionFontSize;

    private int upgradeAmount = 0;

    private string id;

    private Button button;
    private GameObject lockedImage;
    private GameObject outOfStockImage;
    private void Awake()
    {
        button = GetComponent<Button>();

        lockedImage = transform.Find("Locked").gameObject;
        outOfStockImage = transform.Find("Out of Stock").gameObject;

        id = vehicleType.ToString() + itemType;
    }
    void Start()
    {
        button.onClick.AddListener(Purchase);

        outOfStockImage.SetActive(false);
        isInStock = true;

        switch (ShopManager.availabilityCatalogue[id])
        {
            case 0:
                {
                    Unlock();
                    break;
                }
            case 1:
                {
                    Lock();
                    break;
                }
            case 2:
                {
                    OutOfStock();
                    break;
                }
        }
        upgradeAmount = ShopManager.upgradeAmountsCatalogue[id];
    }

    private void OnEnable()
    {
        EventManager.StartListening("unlock" + id, Unlock);
        EventManager.StartListening("lock" + id, Lock);
        EventManager.StartListening("outOfStock" + id, OutOfStock);
    }
    private void OnDisable()
    {
        EventManager.StopListening("unlock" + vehicleType + itemType, Unlock);
        EventManager.StopListening("lock" + vehicleType + itemType, Lock);
        EventManager.StopListening("outOfStock" + vehicleType + itemType, OutOfStock);
    }
    private void Purchase()
    {
        if (CurrencyManager.coins >= price)
        {
            CurrencyManager.Instance.DecreaseCoins(price);
            ShopManager.Instance.BuyItem(itemType);
            upgradeAmount++;
            if (upgradeAmount >= maxUpgradeAmount)
            {
                OutOfStock();
            }
            ShopManager.Instance.UpdateItemAvailabilities();
            ShopManager.upgradeAmountsCatalogue[id] = upgradeAmount;
            AudioManager.PlaySound("Purchase Sound");
        }
        else
        {
            AudioManager.PlaySound("Purchase Fail Sound");
        }
    }
    public void Unlock()
    {
        ShopManager.availabilityCatalogue[id] = 0;
        lockedImage.SetActive(false);
        isUnlocked = true;
        button.enabled = true;
        if (upgradeAmount >= maxUpgradeAmount)
        {
            OutOfStock();
        }
    }
    public void Lock()
    {
        ShopManager.availabilityCatalogue[id] = 1;
        lockedImage.SetActive(true);
        isUnlocked = false;
        button.enabled = false;
    }
    public void OutOfStock()
    {
        ShopManager.availabilityCatalogue[id] = 2;
        outOfStockImage.SetActive(true);
        isInStock = false;
        button.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShopManager.Instance.OpenDescription(description, descriptionFontSize);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ShopManager.Instance.CloseDescription();
    }
}
