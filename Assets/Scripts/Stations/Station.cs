using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum StationState
{
    Idle,
    Preparing,
    Finished
}

public enum StationType
{
    Cutting,
    Cooking,
    Plate,
    Ingredient
}

public abstract class Station : MonoBehaviour
{
    private Renderer[] _renderers;
    private Collider[] _colliders;

    protected StationState currentState = StationState.Idle;
    protected float progress = 0f;
    protected Ingredient currentIngredient;

    [SerializeField] private StationType _stationType;
    [SerializeField] private List<IngredientType> _acceptedIngredients;
    [SerializeField] protected float prepareTime = 3f;
    [SerializeField] private Image _progressBarFill;

    public StationType StationType => _stationType;


    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _colliders = GetComponentsInChildren<Collider>();
    }


    public void ManualUpdate()
    {
        if (currentState == StationState.Preparing)
        {
            progress += Time.deltaTime;
            progress = Mathf.Min(progress, prepareTime);

            if (_progressBarFill != null)
            {
                _progressBarFill.fillAmount = progress / prepareTime;
            }

            if (progress >= prepareTime)
            {
                OnPreparationFinished();
            }
        }
    }

    public virtual void Interact()
    {
        switch (currentState)
        {
            case StationState.Idle:
                TryStartPreparation();
                break;

            case StationState.Finished:
                TryTakePreparedItem();
                break;
        }
    }

    protected virtual void TryStartPreparation()
    {
        if (currentIngredient == null) return;

        currentState = StationState.Preparing;
        progress = 0f;

        if (_progressBarFill != null)
        {
            _progressBarFill.fillAmount = 0f;
        }
    }

    protected virtual void OnPreparationFinished()
    {
        currentState = StationState.Finished;
        currentIngredient.Prepare();
        Debug.Log($"{gameObject.name}: Finished preparing");
    }

    protected virtual void TryTakePreparedItem()
    {
        if (currentIngredient == null) return;

        PlayerPickUpIngredient player = FindAnyObjectByType<PlayerPickUpIngredient>();

        if (player != null && !player.IsHoldingIngredient)
        {
            player.ReceiveIngredient(currentIngredient);
            Debug.Log($"{gameObject.name}: Item given to player");
        }
        else
        {
            currentIngredient.gameObject.SetActive(true);
            currentIngredient.transform.position = transform.position + Vector3.right;
            Debug.Log($"{gameObject.name}: Item dropped");
        }

        currentState = StationState.Idle;
        if (_progressBarFill != null) _progressBarFill.fillAmount = 0f;
        currentIngredient = null;
    }

    public virtual bool InsertIngredient(Ingredient ingredient)
    {
        if (currentState != StationState.Idle) return false;

        if (!_acceptedIngredients.Contains(ingredient.Type))
        {
            Debug.Log($"{ingredient.Type} not accepted at {gameObject.name}. Insertion blocked.");
            return false;
        }

        currentIngredient = ingredient;
        ingredient.gameObject.SetActive(false);
        Debug.Log($"{gameObject.name}: {ingredient.Type} inserted");
        return true;
    }
}
