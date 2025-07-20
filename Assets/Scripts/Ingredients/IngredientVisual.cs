using UnityEngine;

[CreateAssetMenu(menuName = "Cooking/Ingredient Visual")]
public class IngredientVisual : ScriptableObject
{
    public IngredientType Type;
    public IngredientPrepareState State;
    public Sprite WorldSprite;
    public Sprite Icon;
}
