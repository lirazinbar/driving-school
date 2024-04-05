using UnityEngine;

public class StopSurfaceDetector: MonoBehaviour
{
    private CarController _car;
    private StopLine _stopLine;
    private bool _carStopped;
    private bool _carReachedSign;
    
    void Start()
    {
        _stopLine = transform.parent.gameObject.transform.Find("StopLine").GetComponentInChildren<StopLine>();
    }

    private void Update()
    {
        if (_carReachedSign)
        {
            if (_car.IsStopped())
            {
                _carStopped = true;
                EventsManager.Instance.TriggerCarStoppedBeforeStopSignEvent(GetInstanceID());
                CarDriverAutonomous autonomousCar = _car.GetComponent<CarDriverAutonomous>();
                if (autonomousCar != null)
                {
                    // Stop detecting the stop line with raycast
                    autonomousCar.SetDetectStopLine(false);
                }
                _carReachedSign = false;
            }
            if (_stopLine.IsCarPassed())
            {
                _carReachedSign = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("StopSurfaceDetector triggered.");
        _carStopped = false;
        if (other.CompareTag("Car"))
        {
            EventsManager.Instance.TriggerCarReachedStopSignEvent(other.gameObject, GetInstanceID());
            _car = other.gameObject.GetComponent<CarController>();
            _carReachedSign = true;
        }
    }
    
    public bool IsCarStopped()
    {
        return _carStopped;
    }
}
