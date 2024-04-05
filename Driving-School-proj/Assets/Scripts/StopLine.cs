using UnityEngine;

public class StopLine : MonoBehaviour
{
    private GameObject _stopSign;
    private StopSurfaceDetector _stopSurfaceDetector;
    private CarDriverAutonomous _autonomousCar;
    private bool _carPassed;
    
    private void Start()
    {
        // Find the Stop Sign parent GameObject in the scene
        _stopSign = transform.parent.parent.gameObject;
        _stopSurfaceDetector = _stopSign.transform.Find("StopSurfaceDetector").GetComponent<StopSurfaceDetector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Debug.Log("StopLine triggered.");
            if (!_stopSurfaceDetector.IsCarStopped())
            {
                _carPassed = true;
                EventsManager.Instance.TriggerCarPassedStopSignEvent(_stopSign.GetInstanceID());
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Debug.Log("Car completely passed the StopLine.");
            _autonomousCar = other.gameObject.GetComponent<CarDriverAutonomous>();
            if (_autonomousCar != null)
            {
                // Stop detecting the stop line with raycast
                _autonomousCar.SetDetectStopLine(true);
            }
        }
    }
    
    public bool IsCarPassed()
    {
        return _carPassed;
    }
}
