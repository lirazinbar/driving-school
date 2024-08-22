using UnityEngine;

namespace ParkingTest
{
    public class ParkingSlotTile : MonoBehaviour
    {
        [SerializeField] private ParkingSlot parkingSlot;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                parkingSlot.DeactivateParkingBeam();
            }
        }
    }
}