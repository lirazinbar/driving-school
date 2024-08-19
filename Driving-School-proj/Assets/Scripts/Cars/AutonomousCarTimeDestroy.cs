using Managers;
using UnityEngine;

namespace Cars
{
    public class AutonomousCarTimeDestroy: MonoBehaviour
    {
        [SerializeField] private float timeToLiveSeconds = 120f;

        private void Start()
        {
            // Increase TTL if it's not default route
            if (!GameManager.Instance.IsDefaultRoute())
            {
                timeToLiveSeconds += 120f;
            }
        }

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