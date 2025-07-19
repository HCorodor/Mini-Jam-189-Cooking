using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _orderText;
    [SerializeField] private Image _timerFill;

    private Order _linkedOrder;

    public void Setup(Order order)
    {
        _linkedOrder = order;
        _orderText.text = order.Recipe.DishName;
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
