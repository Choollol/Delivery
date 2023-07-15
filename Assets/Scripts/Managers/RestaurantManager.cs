using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static int ingredients { get; private set; }
    private static int maxIngredients = 100;
    private void Awake()
    {
        ingredients = 10;
    }
    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("ingredients", ingredients);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("ingredients");
    }
    void Start()
    {
        GameManager.OtherMenuOpened();
    }

    public void DropOffIngredients()
    {
        ingredients += CapacityManager.ingredients;
        EventMessenger.TriggerEvent("DropOffIngredients");
        if (ingredients > maxIngredients)
        {
            ingredients = maxIngredients;
        }
        PrimitiveMessenger.EditObject("ingredients", ingredients);
        EventMessenger.TriggerEvent("UpdateIngredientsText");
    }
    public static int GetIngredients()
    {
        return ingredients;
    }
    public static void UseIngredients(int ingredientsUsed)
    {
        ingredients -= ingredientsUsed;
        if (ingredients < 0)
        {
            ingredients = 0;
        }
        PrimitiveMessenger.EditObject("ingredients", ingredients);
        EventMessenger.TriggerEvent("UpdateIngredientsText");
    }
}
