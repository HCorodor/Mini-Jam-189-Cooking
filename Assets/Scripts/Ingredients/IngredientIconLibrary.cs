using UnityEngine;
using System.Collections.Generic;

public class IngredientIconLibrary : MonoBehaviour
{
    public static IngredientIconLibrary Instance;

    [SerializeField] private List<IngredientVisual> _visuals;

    private Dictionary<IngredientType, Sprite> _lookUp;

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

        _lookUp = new Dictionary<IngredientType, Sprite>();
        foreach (var visual in _visuals)
        {
            _lookUp[visual.Type] = visual.Icon;
        }
    }

    public Sprite GetIcon(IngredientType type)
    {
        return _lookUp.TryGetValue(type, out var icon) ? icon : null;
    }
}
