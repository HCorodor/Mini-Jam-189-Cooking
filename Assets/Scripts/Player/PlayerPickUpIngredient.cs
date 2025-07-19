using UnityEngine;

public class PlayerPickUpIngredient : MonoBehaviour
{
    public bool IsHoldingIngredient;
    public Ingredient HeldIngredient;

    private Ingredient _currentNearbyIngredient;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ingredient ingredient = other.GetComponent<Ingredient>();
        if (ingredient != null && ingredient.PickupState == IngredientPickupState.Pickupable)
        {
            _currentNearbyIngredient = ingredient;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
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

        Station nearbyStation = GetComponent<InteractWithStation>()?.CurrentStation;

        if (nearbyStation != null)
        {
            bool succes = nearbyStation.InsertIngredient(HeldIngredient);

            if (nearbyStation.InsertIngredient(HeldIngredient))
                {
                    HeldIngredient = null;
                    IsHoldingIngredient = false;
                    return;
                }

            Debug.Log("Station rejected the ingredient. You are still holding it.");
        }
        else
        {
            Vector3 dropPosition = transform.position + transform.right;
            HeldIngredient.Drop(dropPosition);
            HeldIngredient = null;
            IsHoldingIngredient = false;
        }
    }

    public void ReceiveIngredient(Ingredient ingredient)
    {
        HeldIngredient = ingredient;
        IsHoldingIngredient = true;
        ingredient.PickUp();
        Debug.Log($"[ReceiveIngredient] Received: {ingredient.Type}, PrepareState: {ingredient.PrepareState}");
        
    }
}