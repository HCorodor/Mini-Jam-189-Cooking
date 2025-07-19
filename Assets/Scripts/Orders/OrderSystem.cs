using UnityEngine;
using System.Collections.Generic;

public class OrderSystem : MonoBehaviour
{
    public static OrderSystem Instance;

    [SerializeField] private OrderUIManager _orderUIManager;
    [SerializeField] private List<DishRecipe> _possibleDishes;
    [SerializeField] private float _orderInterval = 10f;
    [SerializeField] private float _orderTimeLimit = 30f;

    private float _timer;
    private List<Order> _activeOrders = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _orderInterval)
        {
            _timer = 0f;
            AddRandomOrder();
        }

        for (int i = _activeOrders.Count - 1; i >= 0; i--)
        {
            var order = _activeOrders[i];
            if (order.IsCompleted) continue;

            order.TimeRemaining -= Time.deltaTime;
            if (order.TimeRemaining <= 0f)
            {
                Debug.Log($"Order Failed: {order.Recipe.DishName}");
                _activeOrders.RemoveAt(i);
            }
        }
    }

    private void AddRandomOrder()
    {
        var recipe = _possibleDishes[Random.Range(0, _possibleDishes.Count)];
        var order = new Order(recipe, _orderTimeLimit);
        _activeOrders.Add(order);

        _orderUIManager.AddOrderUI(order);

        Debug.Log($"New Order added: {recipe.DishName}");
    }

    public bool SubmitDish(DishRecipe dish)
    {
        foreach (var order in _activeOrders)
        {
            if (!order.IsCompleted && order.Recipe == dish)
            {
                order.IsCompleted = true;
                Debug.Log($"Order Completed: {dish.DishName}");
                return true;
            }
        }
        Debug.Log("No matching order found for submitted dish.");
        return false;
    }

    public List<Order> GetActiveOrders() => _activeOrders;
}
