using UnityEngine;

public class CookingStation : Station
{
    protected override void OnPreparationFinished()
    {
        base.OnPreparationFinished();
        Debug.Log("Cooking done!");
    }
}
