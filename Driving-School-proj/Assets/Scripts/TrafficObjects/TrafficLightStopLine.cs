using UnityEngine;

namespace TrafficObjects
{
    public class TrafficLightStopLine : MonoBehaviour
    {
        [SerializeField] private TrafficLightController trafficLightController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                Debug.Log("Car entered the traffic light stop line.");
                trafficLightController.OnCarPassedStopLine(other.gameObject.GetInstanceID());
            }
        }
    }
}
