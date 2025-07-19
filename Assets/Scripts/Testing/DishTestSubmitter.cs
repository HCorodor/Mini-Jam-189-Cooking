using UnityEngine;

using UnityEngine;
using System.Collections;

public class DishTestSubmitter : MonoBehaviour
{
    [SerializeField] private DishRecipe testRecipe;
    [SerializeField] private float submissionInterval = 3f; // Try every 5 seconds

    private void Start()
    {
        StartCoroutine(SubmitRoutine());
    }

    private IEnumerator SubmitRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(submissionInterval);

            if (OrderSystem.Instance == null)
            {
                Debug.LogWarning("OrderSystem not found.");
                continue;
            }

            bool success = OrderSystem.Instance.SubmitDish(testRecipe);
            Debug.Log(success ? $"✔️ Dish submitted: {testRecipe.DishName}" : $"❌ No matching order for: {testRecipe.DishName}");
        }
    }
}
