using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlateStation : Station
{
    [SerializeField] private List<DishRecipe> _allRecipes;

    private List<Ingredient> _ingredientsOnPlate = new();

    public override void InsertIngredient(Ingredient ingredient)
    {
        if (ingredient.PrepareState != IngredientPrepareState.Prepared) return;

        _ingredientsOnPlate.Add(ingredient);
        Debug.Log($"Added {ingredient.Type} to plate station.");
        CheckForCompletedDish();
    }

    private void CheckForCompletedDish()
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

                ClearPlate();
                break;
            }
        }
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
    }
}
