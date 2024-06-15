using Cars;
using Enums;
using Managers;
using TrafficObjects;
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
                if (GameManager.Instance.IsMainCar(_car.gameObject.GetInstanceID()))
                {
                    Debug.Log("Main car stopped before stop sign");
                }
                
                _carStopped = true;
                EventsManager.Instance.TriggerCarStoppedBeforeStopSignEvent(stopSign.GetInstanceID());
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
                EventsManager.Instance.TriggerCarReachedStopSignEvent(other.gameObject.GetInstanceID(), stopSign.GetInstanceID());
                _car = other.gameObject.GetComponent<CarController>();
                _carReachedSign = true;
            }            
        }
    }
    
    public bool IsCarStopped()
    {
        return _carStopped;
    }
}