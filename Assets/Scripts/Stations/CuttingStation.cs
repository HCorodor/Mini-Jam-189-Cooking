using UnityEngine;

public class CuttingStation : Station
{
    private void Start()
    {
        Ingredient testIngredient = FindObjectOfType<Ingredient>();
        InsertIngredient(testIngredient);
    }

    protected override void OnPreparationFinished()
    {
        base.OnPreparationFinished();
        Debug.Log("Cutting done!");
    }
}
