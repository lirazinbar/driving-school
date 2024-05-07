using System.Collections.Generic;
using Cars;
using UnityEngine;

namespace TrafficObjects
{
    public class TrafficLightSurfaceDetector : MonoBehaviour
    {
        [SerializeField] private TrafficLightController _trafficLightController;
        private List<CarDriverAutonomous> _autonomousCars = new List<CarDriverAutonomous>();

        public bool IsEmpty()
        {
            return _autonomousCars.Count == 0;   
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                Debug.Log("Car entered the traffic light surface detector.");
                CarDriverAutonomous autonomousCar = other.GetComponent<CarDriverAutonomous>();
                if (autonomousCar != null)
                {
                    _autonomousCars.Add(autonomousCar);
                    if (_trafficLightController.GetCurrentLightState() == TrafficLightController.LightState.Green)
                    {
                        autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", true);
                        autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", true);
                    }
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                Debug.Log("Car exited the traffic light surface detector.");
                CarDriverAutonomous autonomousCar = other.GetComponent<CarDriverAutonomous>();
                if (autonomousCar != null)
                {
                    // Reset the layer of the raycast
                    _autonomousCars.Remove(autonomousCar);
                    autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", false);
                    autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", false);
                }
            }
        }
        
       public void OnLightChanged(TrafficLightController.LightState lightState)
        {
            foreach (CarDriverAutonomous autonomousCar in _autonomousCars)
            {
                switch (lightState)
                {
                    case TrafficLightController.LightState.Yellow:
                        autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", false);
                        autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", false);
                        break;
                    case TrafficLightController.LightState.Green:
                        autonomousCar.SetLayerOfRaycast(RaycastType.Stop, "StopLine", true);
                        autonomousCar.SetLayerOfRaycast(RaycastType.SlowDown, "StopLine", true);
                        break;
                }
            }
        }
    }
}
