using UnityEngine;

public class IngredientStation : Station
{
    [SerializeField] private IngredientType _ingredientType;
    [SerializeField] private GameObject _ingredientPrefab;

    public override void Interact()
    {
        PlayerPickUpIngredient player = FindFirstObjectByType<PlayerPickUpIngredient>();

        if (player != null && !player.IsHoldingIngredient)
        {
            Ingredient newIngredient = Instantiate(_ingredientPrefab, transform.position + Vector3.up, Quaternion.identity).GetComponent<Ingredient>();
            newIngredient.Type = _ingredientType;
            newIngredient.PrepareState = IngredientPrepareState.Unprepared;
            newIngredient.PickupState = IngredientPickupState.Pickupable;

            player.ReceiveIngredient(newIngredient);
            Debug.Log($"{gameObject.name}: Gave player a fresh {_ingredientType}");
        }
        else
        {
            Debug.Log($"{gameObject.name}: Player is already holding something.");
        }
    }

    public override bool InsertIngredient(Ingredient ingredient)
    {
        Debug.LogWarning("IngredientDispenserStation does not accept ingredients.");
        return false;
    }
}
