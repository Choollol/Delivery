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
        EventMessenger.StartListening("DropOffDishesCapacity", DropOffDishes);
        EventMessenger.StartListening("PickUpIngredients", PickUpIngredients);
        EventMessenger.StartListening("DropOffIngredients", DropOffIngredients);

        PrimitiveMessenger.AddObject("ingredientsToPickUp", 0);
        PrimitiveMessenger.AddObject("ingredientsToDropOff", 0);
        PrimitiveMessenger.AddObject("dishesToPickUp", 0);
        PrimitiveMessenger.AddObject("dishesToDropOff", 0);
    }
    private void OnDisable()
    {
        PrimitiveMessenger.RemoveObject("maxCapacity");
        PrimitiveMessenger.RemoveObject("capacityInUse");

        EventMessenger.StopListening("PickUpDishesCapacity", PickUpDishes);
        EventMessenger.StopListening("DropOffDishesCapacity", DropOffDishes);
        EventMessenger.StopListening("PickUpIngredients", PickUpIngredients);
        EventMessenger.StopListening("DropOffIngredients", DropOffIngredients);

        PrimitiveMessenger.RemoveObject("ingredientsToPickUp");
        PrimitiveMessenger.RemoveObject("ingredientsToDropOff");
        PrimitiveMessenger.RemoveObject("dishesToPickUp");
        PrimitiveMessenger.RemoveObject("dishesToDropOff");
    }
    private void Awake()
    {
        maxCapacity = 1;
    }
    private void PickUpDishes()
    {
        capacityInUse += PrimitiveMessenger.GetObject("dishesToPickUp");
        dishes += PrimitiveMessenger.GetObject("dishesToPickUp");
        if (capacityInUse > maxCapacity)
        {
            dishes -= capacityInUse - maxCapacity;
            capacityInUse = maxCapacity;
        }
        UpdateCapacity();
    }
    private void PickUpIngredients()
    {
        capacityInUse += PrimitiveMessenger.GetObject("ingredientsToPickUp");
        ingredients += PrimitiveMessenger.GetObject("ingredientsToPickUp");
        if (capacityInUse > maxCapacity)
        {
            ingredients -= capacityInUse - maxCapacity;
            capacityInUse = maxCapacity;
        }
        UpdateCapacity();
    }
    private void DropOffDishes()
    {
        capacityInUse -= PrimitiveMessenger.GetObject("dishesToDropOff");
        dishes -= PrimitiveMessenger.GetObject("dishesToDropOff");
        UpdateCapacity();
    }
    private void DropOffIngredients()
    {
        capacityInUse -= PrimitiveMessenger.GetObject("ingredientsToDropOff");
        ingredients -= PrimitiveMessenger.GetObject("ingredientsToDropOff");
        UpdateCapacity();
    }
    private void UpdateCapacity()
    {
        PrimitiveMessenger.EditObject("capacityInUse", capacityInUse);
        EventMessenger.TriggerEvent("UpdateCapacityText");
    }

}
