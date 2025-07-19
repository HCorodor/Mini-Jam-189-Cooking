using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStationTester : MonoBehaviour
{
    [SerializeField] private PlateStation plateStation;
    [SerializeField] private List<IngredientType> testIngredients;
    [SerializeField] private float delayBeforeChecking = 0.5f;

    private void Start()
    {
        StartCoroutine(RunTestAfterOrder());
    }

    private IEnumerator RunTestAfterOrder()
    {
        // Wait for orders to exist
        while (OrderSystem.Instance == null || OrderSystem.Instance.GetActiveOrders().Count == 0)
        {
            yield return null;
        }

        foreach (var type in testIngredients)
        {
            Ingredient mock = CreateMockIngredient(type);
            plateStation.InsertIngredient(mock);  // Use automatic insertion
            yield return new WaitForSeconds(0.1f);
        }

        // No need for manual check here since InsertIngredient auto checks
    }

    private Ingredient CreateMockIngredient(IngredientType type)
    {
        GameObject obj = new GameObject("Mock " + type);
        Ingredient ing = obj.AddComponent<Ingredient>();
        ing.Type = type;
        ing.PrepareState = IngredientPrepareState.Prepared;
        return ing;
    }
}