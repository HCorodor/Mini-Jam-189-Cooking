using UnityEngine;

public class PlayerPickUpIngredient : MonoBehaviour
{
    public bool IsHoldingIngredient;
    public Ingredient HeldIngredient;

    private Ingredient _currentNearbyIngredient;

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient != null && ingredient.PickupState == IngredientPickupState.Pickupable)
        {
            _currentNearbyIngredient = ingredient;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient != null && ingredient == _currentNearbyIngredient)
        {
            _currentNearbyIngredient = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsHoldingIngredient && _currentNearbyIngredient != null)
        {
            PickUpIngredient();
        }

        if (Input.GetKeyDown(KeyCode.Q) && IsHoldingIngredient)
        {
            DropIngredient();
        }
    }

    private void PickUpIngredient()
    {
        HeldIngredient = _currentNearbyIngredient;
        Debug.Log(HeldIngredient.Type.ToString() + " picked up!");
        HeldIngredient.PickUp();
        IsHoldingIngredient = true;
        _currentNearbyIngredient = null;
    }

    private void DropIngredient()
    {
        if (HeldIngredient == null) return;

        Vector3 dropPosition = transform.position + transform.forward;
        HeldIngredient.Drop(dropPosition);
        HeldIngredient = null;
        IsHoldingIngredient = false;
    }
}