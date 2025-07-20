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

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

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

        UpdateVisual();
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

        UpdateVisual(); // Change sprite on prepare
    }

    private void UpdateVisual()
    {
        if (_spriteRenderer == null || IngredientIconLibrary.Instance == null) return;

        var newSprite = IngredientIconLibrary.Instance.GetWorldSprite(Type, PrepareState);
        if (newSprite != null)
        {
            _spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning($"Missing world sprite for {Type} in state {PrepareState}");
        }
    }
}
