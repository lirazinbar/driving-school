using UnityEngine;

namespace TrafficObjects
{
    public class TrafficLightSurfaceDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                other.GetComponent<CarController>().SetInputs(0, 0, true);
            }
        }
    }
}
