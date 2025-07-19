using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlateStation : Station
{
    [SerializeField] private List<DishRecipe> _allRecipes;

    private List<Ingredient> _ingredientsOnPlate = new();

    public override void Interact()
    {
        PlayerPickUpIngredient player = FindObjectOfType<PlayerPickUpIngredient>();

        if (player != null && player.IsHoldingIngredient)
        {
            Ingredient held = player.HeldIngredient;

            if (held.PrepareState == IngredientPrepareState.Prepared)
            {
                InsertIngredient(held);

                player.HeldIngredient = null;
                player.IsHoldingIngredient = false;

                Debug.Log("Ingredient added to plate via Interact.");
            }
            else
            {
                Debug.Log("Cannot plate wrongly prepared ingredient");
            }
        }
        else
        {
            Debug.Log("Player is not holding an ingredient.");
        }
    }

    public override void InsertIngredient(Ingredient ingredient)
    {
        if (ingredient.PrepareState != IngredientPrepareState.Prepared) return;

        _ingredientsOnPlate.Add(ingredient);
        Debug.Log($"Added {ingredient.Type} to plate station.");

        bool isCompleteDish = TrySubmitDish();

        if (isCompleteDish)
        {
            ClearPlate();
        }
        else
        {
            Debug.Log("Current Plate does not match any recipe yet");
        }
    }

    private bool TrySubmitDish()
    {
        foreach (var recipe in _allRecipes)
        {
            if (RecipeMatches(recipe))
            {
                Debug.Log($"Dish Completed: {recipe.DishName}");

                // Submit dish to the order system
                bool success = OrderSystem.Instance.SubmitDish(recipe);

                if (success)
                {
                    Debug.Log("Order matched and completed!");
                }
                else
                {
                    Debug.Log("Dish completed, but no matching order found.");
                }

                return success;
            }
        }
        return false;
    }

    private bool RecipeMatches(DishRecipe recipe)
    {
        var typesOnPlate = _ingredientsOnPlate.Select(i => i.Type).ToList();
        return recipe.RequiredIngredients.All(req => typesOnPlate.Contains(req)) && typesOnPlate.Count == recipe.RequiredIngredients.Count;
    }

    private void ClearPlate()
    {
        foreach (var ing in _ingredientsOnPlate)
        {
            Destroy(ing.gameObject);
        }
        _ingredientsOnPlate.Clear();
        Debug.Log("Plate cleared.");
    }
}
