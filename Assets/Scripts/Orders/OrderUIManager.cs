using UnityEngine;
using System.Collections.Generic;

public class OrderUIManager : MonoBehaviour
{
    [SerializeField] private Transform _orderUIParent;
    [SerializeField] private GameObject _orderUIPrefab;

    private List<OrderUIController> _activeUIs = new();

    public void AddOrderUI(Order order)
    {
        GameObject uiObject = Instantiate(_orderUIPrefab, _orderUIParent);
        OrderUIController controller = uiObject.GetComponent<OrderUIController>();
        controller.Setup(order);
        _activeUIs.Add(controller);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        for (int i = _activeUIs.Count - 1; i >= 0; i--)
        {
            var ui = _activeUIs[i];

            if (ui == null)
            {
                _activeUIs.RemoveAt(i);
                continue;
            }

            if (ui.gameObject == null)
            {
                _activeUIs.RemoveAt(i);
                continue;
            }

            ui.ManualUpdate(dt);

            if (ui == null || ui.gameObject == null)
            {
                _activeUIs.RemoveAt(i);
            }
        }
    }
}
