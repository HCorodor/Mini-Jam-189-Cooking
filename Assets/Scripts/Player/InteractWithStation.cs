using UnityEngine;

public class InteractWithStation : MonoBehaviour
{
    private Animator anim;
    private bool isInteracting = false;
    private Station _currentNearbyStation;
    public Station CurrentStation => _currentNearbyStation;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Station station = other.GetComponentInParent<Station>();

        if (station != null)
        {
            _currentNearbyStation = station;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Station station = other.GetComponentInParent<Station>();

        if (station != null && station == _currentNearbyStation)
        {
            _currentNearbyStation = null;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _currentNearbyStation != null)
        {
            if (!isInteracting)
            {
                isInteracting = true;
                anim.SetBool("isInteracting", true);
            }
            _currentNearbyStation.Interact();
        }

        if (_currentNearbyStation != null)
        {
            _currentNearbyStation.ManualUpdate();
        }
    }

    public void InteractionFinished()
    {
        isInteracting = false;
        anim.SetBool("isInteracting", false);
    }
}
