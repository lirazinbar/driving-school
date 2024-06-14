using System.Collections.Generic;
using Cars;
using Enums;
using Managers;
using UnityEngine;

namespace TrafficObjects
{
    public class TrafficLightSurfaceDetector : MonoBehaviour
    {
        [SerializeField] private TrafficLightController _trafficLightController;
        private List<CarDriverAutonomous> _autonomousCars = new List<CarDriverAutonomous>();
        private int _carsInTrafficLightCounter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                if (GameManager.Instance.IsMainCar( other.gameObject.GetInstanceID()))
                {
                    Debug.Log("Main car entered traffic light surface detector");
                }
                // Debug.Log("Car entered traffic light surface detector");
                CarDriverAutonomous autonomousCar = other.GetComponent<CarDriverAutonomous>();
                if (autonomousCar != null)
                {
                    _autonomousCars.Add(autonomousCar);
                    if (_trafficLightController.GetCurrentLightState() == LightState.Green)
                    {
                        autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", true);
                        autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", true);
                    }
                }
                _carsInTrafficLightCounter++;
                if (_carsInTrafficLightCounter == 1)
                {
                    _trafficLightController.SetIsEmpty(false);
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                if (GameManager.Instance.IsMainCar( other.gameObject.GetInstanceID()))
                {
                    Debug.Log("Main car exited traffic light surface detector");
                }
                // Debug.Log("Car exited traffic light surface detector");
                CarDriverAutonomous autonomousCar = other.GetComponent<CarDriverAutonomous>();
                if (autonomousCar != null)
                {
                    // Reset the layer of the raycast
                    _autonomousCars.Remove(autonomousCar);
                    autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", false);
                    autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", false);
                }

                _carsInTrafficLightCounter--;
                if (_carsInTrafficLightCounter == 0)
                {
                    _trafficLightController.SetIsEmpty(true);
                }
            }
        }
        
       public void OnLightChanged(LightState lightState)
        {
            foreach (CarDriverAutonomous autonomousCar in _autonomousCars)
            {
                switch (lightState)
                {
                    case LightState.Yellow:
                        autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", false);
                        autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", false);
                        break;
                    case LightState.Green:
                        autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", true);
                        autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", true);
                        break;
                }
            }
        }
    }
}
