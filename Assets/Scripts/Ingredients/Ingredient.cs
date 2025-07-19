using UnityEngine;
using System.Collections.Generic;

public enum IngredientPrepareState { Unprepared, Prepared, Misprepared }
public enum IngredientPickupState { Pickupable, PickedUp }
public enum IngredientType { Lettuce, Meat }

public class Ingredient : MonoBehaviour
{
    [Header("States")]
    public IngredientPrepareState PrepareState = IngredientPrepareState.Unprepared;
    public IngredientPickupState PickupState = IngredientPickupState.Pickupable;

    [Header("Type")]
    public IngredientType Type;

    private Renderer[] _renderers;
    private Collider[] _colliders;


    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _colliders = GetComponentsInChildren<Collider>();
    }


    public void PickUp()
    {
        if (PickupState != IngredientPickupState.Pickupable) return;

        PickupState = IngredientPickupState.PickedUp;


        foreach (var r in _renderers) r.enabled = false;
        foreach (var c in _colliders) c.enabled = false;
    }

    public void Drop(Vector3 worldPosition)
    {
        PickupState = IngredientPickupState.Pickupable;
        transform.position = worldPosition;
        gameObject.SetActive(true);

        foreach (var r in _renderers) r.enabled = true;
        foreach (var c in _colliders) c.enabled = true;
    }

    public void Prepare()
    { 
        switch (PrepareState)
        {
            case IngredientPrepareState.Unprepared:
                PrepareState = IngredientPrepareState.Prepared;
                break;
            case IngredientPrepareState.Prepared:
                PrepareState = IngredientPrepareState.Misprepared;
                break;
        }
    }
}
