using System;
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
        private bool _isVisible;

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                if (!stopSurfaceDetector.IsCarStopped())
                {
                    _carPassed = true;
                    if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()) && _isVisible)
                    {
                        string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                        if (hitSide.Equals("Front"))
                        {
                            Debug.Log("Main car passed StopLine");
                            EventsManager.Instance.TriggerCarPassedStopSignEvent(stopSignObject.GetInstanceID());
                        }
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
                    // Check if main car entered the junction without giving way
                    StopSign stopSign = stopSignObject.GetComponent<StopSign>();
                    if (!stopSign.HasLock() && !stopSign.IsLockAvailable())
                    {
                        // If other car has way
                        EventsManager.Instance.TriggerCarDidNotGiveWayEvent();
                    }
                    if (stopSign.IsPedestrianCrossing())
                    {
                        EventsManager.Instance.TriggerCarDidNotGiveWayToPedestrianEvent();
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
        
        public void SetIsVisible(bool isSignVisible)
        {
            _isVisible = isSignVisible;
        }
    }
}
