using Managers;
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
                if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                {
                    Debug.Log("Main car passed traffic light stop line");
                }
                string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                if (hitSide.Equals("Back"))
                {
                    trafficLightController.OnCarPassedStopLine(other.gameObject.GetInstanceID());
                }
            }
        }
    }
}
