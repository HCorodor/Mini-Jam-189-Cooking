using UnityEngine;
using System.Collections.Generic;

public class StationManager : MonoBehaviour
{
    private List<Station> _stations = new List<Station>();

    private void Start()
    {
        Station[] foundStations = FindObjectsOfType<Station>();
        _stations.AddRange(foundStations);
    }

    private void Update()
    {
        foreach (Station station in _stations)
        {
            station.ManualUpdate();
        }
    }
}
