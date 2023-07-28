using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static int ingredients = 10;
    private static int maxIngredients = 100;

    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("ingredients", ingredients);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("ingredients");
    }

    public void DropOffIngredients()
    {
        int ingredientsToDropOff;
        if (ingredients + CapacityManager.ingredients > maxIngredients)
        {
            ingredientsToDropOff = maxIngredients - ingredients;
        }
        else
        {
            ingredientsToDropOff = CapacityManager.ingredients;
        }
        ingredients += ingredientsToDropOff;
        PrimitiveMessenger.EditObject("ingredientsToDropOff", ingredientsToDropOff);
        EventMessenger.TriggerEvent("DropOffIngredients");
        if (ingredients > maxIngredients)
        {
            ingredients = maxIngredients;
        }
        PrimitiveMessenger.EditObject("ingredients", ingredients);
        EventMessenger.TriggerEvent("UpdateIngredientsText");
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
