using UnityEngine;

namespace ParkingTest
{
    public class ParkingSuccessIndicator : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                Debug.Log("Parking Success");
            }
        }
    }
}