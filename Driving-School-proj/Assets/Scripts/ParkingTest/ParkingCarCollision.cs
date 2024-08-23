using System;
using UnityEngine;

namespace ParkingTest
{
    public class ParkingCarCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Car"))
            {
                ParkingTestGameManager.Instance.OnCarHitOtherCar();
            }
        }
    }
}