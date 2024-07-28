using Cars;
using Enums;
using Managers;
using UnityEngine;

namespace TrafficObjects.GiveWay
{
    public class StopLine : MonoBehaviour
    {
        [SerializeField] private GameObject stopSignObject;
        [SerializeField] private StopSurfaceDetector stopSurfaceDetector;
        private CarDriverAutonomous _autonomousCar;
        private bool _carPassed;
        
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                if (!stopSurfaceDetector.IsCarStopped())
                {
                    _carPassed = true;
                    if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                    {
                        Debug.Log("Main car passed StopLine");
                    }
                    
                    string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                    if (hitSide.Equals("Front"))
                    {
                        EventsManager.Instance.TriggerCarPassedStopSignEvent(stopSignObject.GetInstanceID());
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
                    // Check if main car entered the junction without giving way
                    StopSign stopSign = stopSignObject.GetComponent<StopSign>();
                    if (!stopSign.HasLock() && !stopSign.IsLockAvailable())
                    {
                        EventsManager.Instance.carDidNotGiveWayEvent.Invoke();
                    }
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
}