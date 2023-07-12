using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishManager : MonoBehaviour
{
    private int numOfDishes = 0;
    private int maxDishes = 100;

    private int minCookDishes = 1;
    private int maxCookDishes = 5;

    private float cookCooldown = 5;
    private float minCookCooldown = 2f; //5f
    private float maxCookCooldown = 3f; //20f

    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("numOfDishes", numOfDishes);

        EventMessenger.StartListening("PickUpDishes", PickUpDishes);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("numOfDishes");

        EventMessenger.StopListening("PickUpDishes", PickUpDishes);
    }
    void Start()
    {
        StartCoroutine(Cook());
    }
    private IEnumerator Cook()
    {
        yield return new WaitForSeconds(cookCooldown);
        int dishesToCook = Random.Range(minCookDishes, maxCookDishes);
        if (RestaurantManager.GetIngredients() > 0)
        {
            numOfDishes += dishesToCook;
            if (numOfDishes > maxDishes)
            {
                numOfDishes = maxDishes;
            }
        }
        PrimitiveMessenger.EditObject("numOfDishes", numOfDishes);
        EventMessenger.TriggerEvent("UpdateDishesText");
        RestaurantManager.UseIngredients(dishesToCook);
        cookCooldown = Random.Range(minCookCooldown, maxCookCooldown);
        StartCoroutine(Cook());
        yield break;
    }
    private void PickUpDishes()
    {
        int temp = PrimitiveMessenger.GetObject("maxCapacity") - PrimitiveMessenger.GetObject("capacityInUse");
        EventMessenger.TriggerEvent("PickUpDishesCapacity");
        numOfDishes -= temp;
        if (numOfDishes < 0)
        {
            numOfDishes = 0;
        }
        PrimitiveMessenger.EditObject("numOfDishes", numOfDishes);
        EventMessenger.TriggerEvent("UpdateDishesText");
    }
}
