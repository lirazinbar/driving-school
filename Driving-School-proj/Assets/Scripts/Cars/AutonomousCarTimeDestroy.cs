using UnityEngine;

namespace Cars
{
    public class AutonomousCarTimeDestroy: MonoBehaviour
    {
        [SerializeField] private float timeToLiveSeconds = 120f;
        
        // Destroy the car after 120 seconds
        private void Update()
        {
            timeToLiveSeconds -= Time.deltaTime;
            if (timeToLiveSeconds <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}