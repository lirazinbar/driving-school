using UnityEngine;

public class StopSurfaceDetector: MonoBehaviour
{
    [SerializeField] private GameObject stopSign;
    [SerializeField] private StopLine _stopLine;
    private CarController _car;
    private bool _carStopped;
    private bool _carReachedSign;
    

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
                    // Ignore stop line with Stop raycast
                    autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", true);
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
            EventsManager.Instance.TriggerCarReachedStopSignEvent(other.gameObject.GetInstanceID(), stopSign.GetInstanceID());
            _car = other.gameObject.GetComponent<CarController>();
            _carReachedSign = true;
        }
    }
    
    public bool IsCarStopped()
    {
        return _carStopped;
    }
}