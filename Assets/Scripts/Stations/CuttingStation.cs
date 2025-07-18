using UnityEngine;

public class CuttingStation : Station
{
    private void Start()
    {
        InsertIngredient();
    }

    protected override void OnPreparationFinished()
    {
        base.OnPreparationFinished();
        Debug.Log("Cutting done!");
        // Maybe play some cutting sound or spawn particle here
    }
}
