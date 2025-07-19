using UnityEngine;

public class InteractWithStation : MonoBehaviour
{
    private Station _currentNearbyStation;
    public Station CurrentStation => _currentNearbyStation;

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
        if (Input.GetKeyDown(KeyCode.Space) && _currentNearbyStation != null)
        {
            _currentNearbyStation.Interact();
        }

        if (_currentNearbyStation != null)
        {
            _currentNearbyStation.ManualUpdate();
        }
    }
}
