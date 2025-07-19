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
    private Renderer[] _renderers;
    private Collider[] _colliders;

    protected StationState currentState = StationState.Idle;
    protected float progress = 0f;
    protected Ingredient currentIngredient;

    [SerializeField] protected float prepareTime = 3f;
    [SerializeField] private Image _progressBarFill;


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


        PlayerPickUpIngredient player = FindObjectOfType<PlayerPickUpIngredient>();

        if (player != null && !player.IsHoldingIngredient)
        {
            player.ReceiveIngredient(currentIngredient);
            Debug.Log($"{gameObject.name}: Item given to player");

            currentState = StationState.Idle;
            if (_progressBarFill != null) _progressBarFill.fillAmount = 0f;

            currentIngredient = null;
            return;
        }

        currentIngredient.gameObject.SetActive(true);
        currentIngredient.transform.position = transform.position + Vector3.right;

        currentState = StationState.Idle;
        if (_progressBarFill != null) _progressBarFill.fillAmount = 0f;

        Debug.Log($"{gameObject.name}: Item dropped");

        currentIngredient = null;
    }

    public virtual void InsertIngredient(Ingredient ingredient)
    {
        if (currentState != StationState.Idle) return;

        currentIngredient = ingredient;
        ingredient.gameObject.SetActive(false);
        Debug.Log($"{gameObject.name}: {ingredient.Type} inserted");
    }

    public virtual bool IsBeingUsedByPlayer()
    {
        return currentState == StationState.Preparing && PlayerIsNearby();
    }

    protected virtual bool PlayerIsNearby()
    {
        var player = FindObjectOfType<InteractWithStation>();
        if (player == null) return false;

        return Vector2.Distance(player.transform.position, transform.position) < 1.5f;
    }
}
