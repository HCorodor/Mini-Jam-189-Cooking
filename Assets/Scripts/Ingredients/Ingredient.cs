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

    private Renderer _renderer;
    private Collider _collider;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    public void PickUp()
    {
        if (PickupState != IngredientPickupState.Pickupable) return;

        PickupState = IngredientPickupState.PickedUp;

        if (_renderer != null) _renderer.enabled = false;
        if (_collider != null) _collider.enabled = false;
    }

    public void Drop(Vector3 worldPosition)
    {
        PickupState = IngredientPickupState.Pickupable;
        transform.position = worldPosition;

        if (_renderer != null) _renderer.enabled = true;
        if (_collider != null) _collider.enabled = true;
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
