using UnityEngine;

public class PlayerPickUpIngredient : MonoBehaviour
{
    public bool IsHoldingIngredient;
    public Ingredient HeldIngredient;
    float pickUpRange = 2f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsHoldingIngredient = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)&& !IsHoldingIngredient)
        {
            PickUpIngredient();
        }
        if (Input.GetKey(KeyCode.Q) && IsHoldingIngredient)
        {
            DropIngredient();
        }


    }

    void PickUpIngredient()
    {
        Debug.Log("Pickup Ingredient");
        Collider[] hits = Physics.OverlapSphere(transform.position, pickUpRange);
        Debug.Log(hits.Length);
        
        foreach(var hit in hits)
        {
            Debug.Log($"Hit: {hit.name}");
            Ingredient ingredient = hit.GetComponent<Ingredient>();
            if (ingredient != null && ingredient.PickupState == IngredientPickupState.Pickupable)
            {
                ingredient.PickUp();
                HeldIngredient = ingredient;
                IsHoldingIngredient = true;
                break;
            }

        }

    }


    void DropIngredient()
    {
        if(HeldIngredient != null)
        {
            Vector3 dropPosition = transform.position + transform.forward;
            HeldIngredient.Drop(dropPosition);
            HeldIngredient = null;
            IsHoldingIngredient = false;
        }
    }

}
