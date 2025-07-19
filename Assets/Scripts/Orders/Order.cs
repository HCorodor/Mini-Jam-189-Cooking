using System;
public class Order
{
    public DishRecipe Recipe;
    public float TimeRemaining;
    public float InitialTime { get; private set; }
    public bool IsCompleted;

    public Order(DishRecipe recipe, float timeLimit)
    {
        Recipe = recipe;
        TimeRemaining = timeLimit;
        InitialTime = timeLimit;
        IsCompleted = false;
    }
}
