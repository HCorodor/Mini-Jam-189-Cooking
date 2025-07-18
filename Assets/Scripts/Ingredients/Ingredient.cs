using UnityEngine;

public enum IngredientPrepareState { Unprepared, Prepared, Misprepared }
public enum IngredientPickupState { Pickupable, PickedUp }
public enum IngredientType { Lettuce, Meat}

public class Ingredient : MonoBehaviour
{
    [Header("States")]
    public IngredientPrepareState PrepareState = IngredientPrepareState.Unprepared;
    public IngredientPickupState PickupState = IngredientPickupState.Pickupable;

    [Header("Type")]
    public IngredientType Type;

    public void PickUp()
    {
        if (PickupState != IngredientPickupState.Pickupable) return;

        PickupState = IngredientPickupState.PickedUp;
        gameObject.SetActive(false);
    }

    public void Drop(Vector3 worldPosition)
    {
        PickupState = IngredientPickupState.Pickupable;
        gameObject.SetActive(true);
        transform.position = worldPosition;
    }

    public void Prepare()
    {
        if (PrepareState == IngredientPrepareState.Unprepared)
        {
            PrepareState = IngredientPrepareState.Prepared;
        }

        if (PrepareState == IngredientPrepareState.Prepared)
        {
            PrepareState = IngredientPrepareState.Misprepared;
        }
    }

    public bool CanBePreparedAt(Station station)
    {
        if (station is CuttingStation && Type == IngredientType.Lettuce) return true;
        return false;
    }
}
