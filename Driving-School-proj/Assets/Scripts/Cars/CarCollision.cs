using Managers;
using UnityEngine;

namespace Cars
{
    public class CarCollision: MonoBehaviour
    {
        [SerializeField] private bool isMainCar;
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Car"))
            {
                if (isMainCar)
                {
                    EventsManager.Instance.TriggerCarHitOtherCarEvent();
                }
                else
                {
                    if (!GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                    {
                        // If neither car is the main car - Destroy both cars
                        Destroy(other.gameObject);
                        Destroy(gameObject);
                    }
                }
            }
            if (other.gameObject.CompareTag("Pedestrian"))
            {
                if (isMainCar)
                {
                    EventsManager.Instance.TriggerCarHitPedestrianEvent();
                }
                else
                {
                    // If not the main car hits a pedestrian - Destroy the pedestrian and the car
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
            }
            if (other.gameObject.CompareTag("Obstacle"))
            {
                if (isMainCar)
                {
                    EventsManager.Instance.TriggerCarHitObstacleEvent();
                }
            }
        }
    }
}