using UnityEngine;

public class CuttingStation : Station
{
    protected override void OnPreparationFinished()
    {
        base.OnPreparationFinished();
        Debug.Log("Cutting done!");
    }
}
