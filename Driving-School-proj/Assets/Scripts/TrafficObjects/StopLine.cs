using Cars;
using Enums;
using Managers;
using TrafficObjects;
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
            if (!_stopSurfaceDetector.IsCarStopped())
            {
                _carPassed = true;
                if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                {
                    Debug.Log("Main car passed StopLine");
                }
                
                string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                if (hitSide.Equals("Front"))
                {
                    EventsManager.Instance.TriggerCarPassedStopSignEvent(_stopSign.GetInstanceID());
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
            {
                Debug.Log("Main car completely passed StopLine");
            }
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