using UnityEngine;
using System.Collections.Generic;

public class IngredientIconLibrary : MonoBehaviour
{
    public static IngredientIconLibrary Instance;

    [SerializeField] private List<IngredientVisual> _visuals;

    private Dictionary<(IngredientType, IngredientPrepareState), IngredientVisual> _lookUp;

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

        _lookUp = new Dictionary<(IngredientType, IngredientPrepareState), IngredientVisual>();

        foreach (var visual in _visuals)
        {
            var key = (visual.Type, visual.State);
            if (!_lookUp.ContainsKey(key))
            {
                _lookUp.Add(key, visual);
            }
        }
    }

    public Sprite GetIcon(IngredientType type, IngredientPrepareState state)
    {
        var key = (type, state);
        return _lookUp.TryGetValue(key, out var visual) ? visual.Icon : null;
    }

    public Sprite GetWorldSprite(IngredientType type, IngredientPrepareState state)
    {
        var key = (type, state);
        return _lookUp.TryGetValue(key, out var visual) ? visual.WorldSprite : null;
    }
}
