using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Cooking/Dish Recipe")]
public class DishRecipe : ScriptableObject
{
    public string DishName;
    public List<IngredientType> RequiredIngredients;
}
