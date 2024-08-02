using Cars;
using Enums;
using Managers;
using UnityEngine;

namespace TrafficObjects.GiveWay
{
    public class StopSign: MonoBehaviour
    {
        [SerializeField] private JunctionGiveWayManager junctionGiveWayManager;
        [SerializeField] private StopLine stopLine;
        [SerializeField] private StopSurfaceDetector stopSurfaceDetector;
        [SerializeField] private MeshRenderer stopSignRenderer;
        [SerializeField] private MeshRenderer stopLineRenderer;
        
        private CarController _currentCar;
        private bool _isOccupied;
        private bool _hasLock;
        bool isVisible;
        
        void Start()
        {
            EventsManager.Instance.carExitedJunctionEvent.AddListener(OnCarExitedJunction);
        }

        private void OnDestroy()
        {
            EventsManager.Instance.carExitedJunctionEvent.RemoveListener(OnCarExitedJunction);
        }
        
        // The lock is used to prevent multiple cars from entering the junction at the same time
        // When a stopSign successfully takes the lock, it will yield it when the car exits the junction
        public void Update()
        {
            if (_isOccupied)
            {
                if (!_hasLock && TryTakeLock())
                {
                    _hasLock = true;
                    _isOccupied = false;
                    SetCurrentCar();
                    
                    // Let the autonomous car cross the junction
                    CarDriverAutonomous autonomousCar = _currentCar.GetComponent<CarDriverAutonomous>();
                    if (autonomousCar != null)
                    {
                        // Ignore stop line with Stop raycast
                        autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", true);
                    }
                }
            }
        }
        
        public void SetIsVisible(bool isSignVisible)
        {
            isVisible = isSignVisible;
            stopSignRenderer.enabled = isVisible;
            stopLineRenderer.enabled = isVisible;
        }
        
        public bool IsVisible()
        {
            return isVisible;
        }
        
        public bool HasLock()
        {
            return _hasLock;
        }
        
        public void SetOccupied(bool isOccupied)
        {
            _isOccupied = isOccupied;
        }
        
        private void SetCurrentCar()
        {
            _currentCar = stopSurfaceDetector.GetCar();
        }
        
        private bool TryTakeLock()
        {
            return junctionGiveWayManager.TryTakeLock();
        }
        
        private void OnCarExitedJunction(int carId)
        {
            // Yield the lock when the car exits the junction
            if (_hasLock && carId == _currentCar.GetInstanceID())
            {
                _hasLock = false;
                junctionGiveWayManager.YieldLock();
            }
        }

        public bool IsLockAvailable()
        {
            return junctionGiveWayManager.IsLockAvailable();
        }
    }
}