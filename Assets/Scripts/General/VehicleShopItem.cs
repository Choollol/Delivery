using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VehicleShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isUnlocked { get; protected set; }
    public bool isInStock { get; protected set; }

    [SerializeField] protected int price;
    [SerializeField] protected int priceIncreaseAmount;
    [SerializeField] protected VehicleShopManager.ItemType itemType;
    [SerializeField] protected VehicleManager.VehicleType vehicleType;
    [SerializeField] protected int maxUpgradeAmount;
    [SerializeField] protected string description;
    [SerializeField] protected float descriptionFontSize;

    protected int upgradeAmount = 0;

    protected string id;

    protected Button button;
    protected GameObject lockedImage;
    protected GameObject outOfStockImage;
    private void Awake()
    {
        button = GetComponent<Button>();

        lockedImage = transform.Find("Locked").gameObject;
        outOfStockImage = transform.Find("Out of Stock").gameObject;

        id = vehicleType.ToString() + itemType;
    }
    public virtual void Start()
    {
        button.onClick.AddListener(Purchase);

        lockedImage.SetActive(false);
        outOfStockImage.SetActive(false);
        isInStock = true;

        switch (VehicleShopManager.availabilityCatalogue[id])
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
        upgradeAmount = VehicleShopManager.upgradeAmountsCatalogue[id];
        price += upgradeAmount * priceIncreaseAmount;
    }

    private void OnEnable()
    {
        EventMessenger.StartListening("Unlock" + id, Unlock);
        EventMessenger.StartListening("Lock" + id, Lock);
        EventMessenger.StartListening("OutOfStock" + id, OutOfStock);
    }
    private void OnDisable()
    {
        EventMessenger.StopListening("Unlock" + vehicleType + itemType, Unlock);
        EventMessenger.StopListening("Lock" + vehicleType + itemType, Lock);
        EventMessenger.StopListening("OutOfStock" + vehicleType + itemType, OutOfStock);
    }
    private void Purchase()
    {
        if (CurrencyManager.coins >= price)
        {
            CurrencyManager.Instance.DecreaseCoins(price);
            VehicleShopManager.Instance.BuyItem(itemType);
            upgradeAmount++;
            if (upgradeAmount >= maxUpgradeAmount)
            {
                OutOfStock();
            }
            VehicleShopManager.Instance.UpdateItemAvailabilities();
            VehicleShopManager.upgradeAmountsCatalogue[id] = upgradeAmount;
            AudioManager.PlaySound("Purchase Sound");
            price += priceIncreaseAmount;
            VehicleShopManager.Instance.OpenPopup(description, descriptionFontSize, VehicleShopManager.availabilityCatalogue[id] == 0,
                transform.position, price);
        }
        else
        {
            AudioManager.PlaySound("Purchase Fail Sound");
        }
    }
    public void Unlock()
    {
        VehicleShopManager.availabilityCatalogue[id] = 0;
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
        VehicleShopManager.availabilityCatalogue[id] = 1;
        lockedImage.SetActive(true);
        outOfStockImage.SetActive(false);
        isUnlocked = false;
        button.enabled = false;
    }
    public void OutOfStock()
    {
        VehicleShopManager.availabilityCatalogue[id] = 2;
        outOfStockImage.SetActive(true);
        lockedImage.SetActive(false);
        isInStock = false;
        button.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        VehicleShopManager.Instance.OpenPopup(description, descriptionFontSize, VehicleShopManager.availabilityCatalogue[id] == 0, 
            transform.position, price);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        VehicleShopManager.Instance.ClosePopup();
    }
}
