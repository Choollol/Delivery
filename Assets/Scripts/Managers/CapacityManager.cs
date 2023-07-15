using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityManager : MonoBehaviour
{
    public static int maxCapacity;
    public static int capacityInUse;
    public static int ingredients;
    public static int dishes;
    private void OnEnable()
    {
        PrimitiveMessenger.AddObject("maxCapacity", maxCapacity);
        PrimitiveMessenger.AddObject("capacityInUse", capacityInUse);

        EventMessenger.StartListening("PickUpDishesCapacity", PickUpDishes);
        EventMessenger.StartListening("PickUpIngredients", PickUpIngredients);
        EventMessenger.StartListening("DropOffIngredients", DropOffIngredients);

        PrimitiveMessenger.AddObject("ingredientsToPickUp", 0);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("maxCapacity");
        PrimitiveMessenger.RemoveObject("capacityInUse");

        EventMessenger.StopListening("PickUpDishesCapacity", PickUpDishes);
        EventMessenger.StopListening("PickUpIngredients", PickUpIngredients);
        EventMessenger.StopListening("DropOffIngredients", DropOffIngredients);
        PrimitiveMessenger.RemoveObject("ingredientsToPickUp");
    }
    private void Awake()
    {
        maxCapacity = 1;
    }
    private void PickUpDishes()
    {
        capacityInUse += PrimitiveMessenger.GetObject("numOfDishes");
        dishes += PrimitiveMessenger.GetObject("numOfDishes");
        if (capacityInUse > maxCapacity)
        {
            dishes -= capacityInUse - maxCapacity;
            capacityInUse = maxCapacity;
        }
        UpdateCapacity();
    }
    private void PickUpIngredients()
    {
        capacityInUse += 2;//PrimitiveMessenger.GetObject("ingredientsToPickUp");
        ingredients += 2;//PrimitiveMessenger.GetObject("ingredientsToPickUp");
        if (capacityInUse > maxCapacity)
        {
            ingredients -= capacityInUse - maxCapacity;
            capacityInUse = maxCapacity;
        }
        UpdateCapacity();
    }
    private void DropOffDishes()
    {
        capacityInUse -= dishes;
        dishes = 0;
        UpdateCapacity();
    }
    private void DropOffIngredients()
    {
        capacityInUse -= ingredients;
        ingredients = 0;
        UpdateCapacity();
    }
    private void UpdateCapacity()
    {
        PrimitiveMessenger.EditObject("capacityInUse", capacityInUse);
        EventMessenger.TriggerEvent("UpdateCapacityText");
    }

}
