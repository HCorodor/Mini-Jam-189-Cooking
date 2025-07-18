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
    protected Ingredient currentIngredient;

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
        if (currentIngredient == null) return;
        if (!currentIngredient.CanBePreparedAt(this))
        {
            Debug.Log($"{currentIngredient.Type} cannot be prepared at {gameObject.name}");
            return;
        }

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

        currentIngredient.gameObject.SetActive(true);
        currentIngredient.transform.position = transform.position + Vector3.right; // Drop beside

        currentState = StationState.Idle;

        if (_progressBarFill != null) _progressBarFill.fillAmount = 0f;

        Debug.Log($"{gameObject.name}: Item taken");

        currentIngredient = null;
    }

    public virtual void InsertIngredient(Ingredient ingredient)
    {
        if (currentState != StationState.Idle) return;

        currentIngredient = ingredient;
        ingredient.gameObject.SetActive(false);
        Debug.Log($"{gameObject.name}: {ingredient.Type} inserted");
    }
}
