using Cars;
using Managers;
using UnityEngine;


namespace TrafficObjects.GiveWay
{
    public class StopSurfaceDetector: MonoBehaviour
    {
        [SerializeField] private GameObject stopSignObject;
        [SerializeField] private StopLine stopLine;
        private CarController _car;
        private bool _carStopped;
        private bool _carReachedSign;
        private bool _isVisible;

        private void Start()
        {
            _isVisible = stopSignObject.GetComponent<StopSign>().IsVisible();
        }

        private void Update()
        {
            if (_carReachedSign)
            {
                if (_car.IsStopped())
                {
                    _carStopped = true;
                    if (_isVisible)
                    {
                        EventsManager.Instance.TriggerCarStoppedBeforeStopSignEvent(stopSignObject.GetInstanceID());
                    }
                    stopSignObject.GetComponent<StopSign>().SetOccupied(true);
                    _carReachedSign = false;
                }
                if (stopLine.IsCarPassed())
                {
                    _carReachedSign = false;
                }
            }
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                _carStopped = false;
                if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                {
                    Debug.Log("Main car reached stop sign");
                }
                
                string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                if (hitSide.Equals("Front"))
                {
                    if (_isVisible)
                    {
                        EventsManager.Instance.TriggerCarReachedStopSignEvent(other.gameObject.GetInstanceID(), stopSignObject.GetInstanceID());
                    }
                    _car = other.gameObject.GetComponent<CarController>();
                    _carReachedSign = true;
                }            
            }
        }
        
        public bool IsCarStopped()
        {
            return _carStopped;
        }
        
        public CarController GetCar()
        {
            return _car;
        }
    }
}
