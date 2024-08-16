using Managers;
using UnityEngine;

namespace TrafficObjects.TrafficLight
{
    public class TrafficLightStopLine : MonoBehaviour
    {
        [SerializeField] private TrafficLightController trafficLightController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                Debug.Log("hitSide: " + hitSide);
                if (hitSide.Equals("Back") || hitSide.Equals("Left"))
                {
                    if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                    {
                        Debug.Log("Main car passed traffic light stop line");
                    }
                    trafficLightController.OnCarPassedStopLine(other.gameObject.GetInstanceID());
                }
            }
        }
    }
}
