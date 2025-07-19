using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SatisfactionManager : MonoBehaviour
{
    public static SatisfactionManager Instance { get; private set; }

    [SerializeField] private Image _smileyUI;
    [SerializeField] private List<Sprite> _smileyFaces;
    [SerializeField] private int _maxLives;

    private int _currentLives;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _currentLives = _maxLives;
        UpdateSmiley();
    }

    public void LoseLife()
    {
        _currentLives--;
        _currentLives = Mathf.Max(0, _currentLives);

        UpdateSmiley();

        if (_currentLives <= 0)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }

    private void UpdateSmiley()
    {
        int spriteIndex = Mathf.Clamp(_maxLives - _currentLives, 0, _smileyFaces.Count - 1);
        _smileyUI.sprite = _smileyFaces[spriteIndex];
    }
}
