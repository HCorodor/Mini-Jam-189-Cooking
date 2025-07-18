using UnityEngine;
using UnityEngine.UI;

public enum StationState
{
    Idle,
    Preparing,
    Finished
}

public abstract class Station : MonoBehaviour
{
    protected StationState currentState = StationState.Idle;
    protected float progress = 0f;
    protected bool hasIngredient = false;

    [SerializeField] protected float prepareTime = 3f;
    [SerializeField] private Image _progressBarFill;

    protected virtual void Update()
    {
        if (currentState == StationState.Preparing)
        {
            progress += Time.deltaTime;

            // Clamp progress
            if (progress > prepareTime)
            {
                progress = prepareTime;
            }

            if (_progressBarFill != null)
            {
                _progressBarFill.fillAmount = progress / prepareTime;
            }

            if (progress >= prepareTime)
            {
                OnPreparationFinished();
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Space is down");
            Interact();
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

            default:
                break;
        }
    }

    protected virtual void TryStartPreparation()
    {
        if (!hasIngredient) return;

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
        Debug.Log($"{gameObject.name}: Finished preparing");
    }

    protected virtual void TryTakePreparedItem()
    {
        // Simulate taking the item away
        hasIngredient = false;
        currentState = StationState.Idle;
        if (_progressBarFill != null) _progressBarFill.fillAmount = 0f;
        Debug.Log($"{gameObject.name}: Item taken");
    }

    // For testing, you can simulate inserting an ingredient
    public virtual void InsertIngredient()
    {
        if (currentState != StationState.Idle) return;

        hasIngredient = true;
        Debug.Log($"{gameObject.name}: Ingredient inserted");
    }
}
