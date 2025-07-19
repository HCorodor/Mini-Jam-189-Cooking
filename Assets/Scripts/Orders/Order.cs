using System;
public class Order
{
    public DishRecipe Recipe;
    public float TimeRemaining;
    public bool IsCompleted;

    public Order(DishRecipe recipe, float timeLimit)
    {
        Recipe = recipe;
        TimeRemaining = timeLimit;
        IsCompleted = false;
    }
}
