using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishManager : MonoBehaviour
{
    private static int numOfDishes = 3;
    private static int maxDishes = 100;

    private static int minCookDishes = 1;
    private static int maxCookDishes = 5;

    private static float cookCooldown = 5;
    private static float minCookCooldown = 60f;
    private static float maxCookCooldown = 200f;

    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("numOfDishes", numOfDishes);

        EventMessenger.StartListening("PickUpDishes", PickUpDishes);
        PrimitiveMessenger.AddObject("dishesToPickUp", 0);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("numOfDishes");

        EventMessenger.StopListening("PickUpDishes", PickUpDishes);
        PrimitiveMessenger.RemoveObject("dishesToPickUp");
    }
    void Start()
    {
        StartCoroutine(Cook());
    }
    private IEnumerator Cook()
    {
        yield return new WaitForSeconds(cookCooldown);
        int dishesToCook = Random.Range(minCookDishes, maxCookDishes);
        if (RestaurantManager.ingredients > 0)
        {
            numOfDishes += dishesToCook;
            if (numOfDishes > maxDishes)
            {
                numOfDishes = maxDishes;
            }
        }
        UpdateDishes();
        RestaurantManager.UseIngredients(dishesToCook);
        cookCooldown = Random.Range(minCookCooldown, maxCookCooldown);
        while (!GameManager.isGameActive)
        {
            yield return null;
        }
        StartCoroutine(Cook());
        yield break;
    }
    private void PickUpDishes()
    {
        int dishesToPickUp = 0;
        if (numOfDishes + PrimitiveMessenger.GetObject("capacityInUse") <= PrimitiveMessenger.GetObject("maxCapacity"))
        {
            dishesToPickUp = numOfDishes;
        }
        else if (numOfDishes + PrimitiveMessenger.GetObject("capacityInUse") > PrimitiveMessenger.GetObject("maxCapacity"))
        {
            dishesToPickUp = PrimitiveMessenger.GetObject("maxCapacity") - PrimitiveMessenger.GetObject("capacityInUse");
        }
        PrimitiveMessenger.EditObject("dishesToPickUp", dishesToPickUp);
        EventMessenger.TriggerEvent("PickUpDishesCapacity");
        for (int i = 0; i < dishesToPickUp; i++)
        {
            POIManager.AddCustomerPOI(1);
        }
        numOfDishes -= dishesToPickUp;
        UpdateDishes();
    }
    public static void SetDishes(int dishes)
    {
        numOfDishes = dishes;
        UpdateDishes();
    }
    private static void UpdateDishes()
    {
        PrimitiveMessenger.EditObject("numOfDishes", numOfDishes);
        EventMessenger.TriggerEvent("UpdateDishesText");
    }
}
