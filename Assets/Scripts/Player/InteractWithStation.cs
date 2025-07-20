using UnityEngine;

public class InteractWithStation : MonoBehaviour
{
    private Animator anim;
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
        // ✅ Trigger interaction ONCE
        if (Input.GetKeyDown(KeyCode.Space) && _currentNearbyStation != null)
        {
            _currentNearbyStation.Interact();
        }

        // ✅ Only animate if station is actively being used AND player is nearby
        bool shouldAnimateInteraction = _currentNearbyStation != null &&
                                        _currentNearbyStation.IsBeingUsedByPlayer();

        anim.SetBool("isInteracting", shouldAnimateInteraction);

        // ✅ Station logic update
        // if (_currentNearbyStation != null)
        // {
        //     _currentNearbyStation.ManualUpdate();
        // }
    }
}
