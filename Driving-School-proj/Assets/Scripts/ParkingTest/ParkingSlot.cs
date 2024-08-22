using UnityEngine;

namespace ParkingTest
{
    public class ParkingSlot : MonoBehaviour
    {
        [SerializeField] private GameObject parkingBeam;
        private bool isOccupied;
        

        private void Start()
        {
        }

        public void ActivateParkingBeam()
        {
            parkingBeam.SetActive(true);
        }

        public void DeactivateParkingBeam()
        {
            parkingBeam.SetActive(false);
        }
        
        public bool IsOccupied()
        {
            return isOccupied;
        }
    }
}