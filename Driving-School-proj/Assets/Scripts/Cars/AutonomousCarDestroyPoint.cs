using Managers;
using UnityEngine;

namespace Cars
{
    public class AutonomousCarDestroyPoint: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Car"))
            {
                if (!GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                {
                    Destroy(other.gameObject);
                }
            }
        }
    }
}