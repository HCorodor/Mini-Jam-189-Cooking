using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _orderText;
    [SerializeField] private Image _timerFill;
    [SerializeField] private Transform _iconContainer;
    [SerializeField] private GameObject _iconPrefab;

    private Order _linkedOrder;

    public void Setup(Order order)
    {
        _linkedOrder = order;
        _orderText.text = order.Recipe.DishName;

        foreach (var ingredient in order.Recipe.RequiredIngredients)
        {
            var iconGO = Instantiate(_iconPrefab, _iconContainer);
            var image = iconGO.GetComponent<Image>();
            var sprite = IngredientIconLibrary.Instance.GetIcon(ingredient);

            if (image != null && sprite != null)
            {
                image.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"Missing icon for ingredient: {ingredient}");
            }
        }
    }

    public void ManualUpdate(float deltaTime)
    {
        if (_linkedOrder == null) return;

        float t = Mathf.Clamp01(_linkedOrder.TimeRemaining / _linkedOrder.InitialTime);
        _timerFill.fillAmount = t;

        if (_linkedOrder.IsCompleted || _linkedOrder.TimeRemaining <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
