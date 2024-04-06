using UnityEngine;

public class StopLine : MonoBehaviour
{
    [SerializeField] private GameObject _stopSign;
    [SerializeField] private StopSurfaceDetector _stopSurfaceDetector;
    private CarDriverAutonomous _autonomousCar;
    private bool _carPassed;
    

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
                // Don't ignore stop line with Stop raycast
                _autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", false);
            }
        }
    }
    
    public bool IsCarPassed()
    {
        return _carPassed;
    }
}
