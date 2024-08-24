using System;
using System.Collections;
using UnityEngine;

namespace ParkingTest
{
    public class ParkingSlot : MonoBehaviour
    {
        [SerializeField] private GameObject parkingBeam;
        [SerializeField] private Collider parkingSlotCollider;

        private bool _isOccupied;
        private bool _isTarget;

        private const float ParallelAngle = 0f;
        private const float PerpendicularAngle = 90f;
        
        private Transform _carTransform;
        private Rigidbody _carRigidbody;
        private Collider _carCollider;
        private LayerMask _carLayer; 
        
        private const float AngleTolerance = 8f; // Allowed tolerance for angle
        private const float StopThreshold = 0.1f; // Minimum speed to consider the car as stopped
        private const float CheckInterval = 0.2f; // Time interval for checking the conditions


        private void Awake()
        {
            DetectCarInSlot();
        }

        public void SetSlotAsTarget(bool isTarget)
        {
            _isTarget = isTarget;
            parkingBeam.SetActive(isTarget);
            _isOccupied = isTarget;
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
            return _isOccupied;
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                _isOccupied = true;
                _carTransform = other.transform;
                _carRigidbody = other.GetComponent<Rigidbody>();
                _carCollider = other.GetComponent<Collider>();
                if (_isTarget)
                {
                    DeactivateParkingBeam();
                    StartCoroutine(CheckParkingConditions());
                }
            }
        }
        
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                _isOccupied = false;
                if (_isTarget)
                {
                    ActivateParkingBeam();
                }
            }
        }
        
        private IEnumerator CheckParkingConditions()
        {
            while (_isOccupied)
            {
                if (IsCarCompletelyInside() && IsCarAngleCorrect() && IsCarStopped())
                {
                    Debug.Log("Car parked successfully!");
                    ParkingTestGameManager.Instance.OnCarParkedSuccessfully();
                    yield break;
                }

                yield return new WaitForSeconds(CheckInterval);
            }
        }
        
        bool IsCarCompletelyInside()
        {
            Bounds parkingSlotBounds = parkingSlotCollider.bounds;
            Bounds carBounds = _carCollider.bounds;
            
            return parkingSlotBounds.Contains(carBounds.min) && parkingSlotBounds.Contains(carBounds.max);
        }

        bool IsCarAngleCorrect()
        {
            float carAngle = NormalizeAngle(_carTransform.eulerAngles.y);
            float slotAngle
            
            // Calculate the angle difference
            float angleDifference = Mathf.Abs(carAngle - slotAngle);
            float angDifference180 = Mathf.Abs(180f - angleDifference - s);
            
            // Check if the angle difference is within the tolerance
            return angleDifference <= AngleTolerance || angDifference180 <= AngleTolerance;
        }
        
        // Normalize angle to be within [0, 360) range
        float NormalizeAngle(float angle)
        {
            return (angle % 360f + 360f) % 360f;
        }

        bool IsCarStopped()
        {
            return _carRigidbody.velocity.magnitude <= StopThreshold;
        }
        
        private void DetectCarInSlot()
        {
            // Define the detection area based on the parking slot's collider bounds
            var bounds = parkingSlotCollider.bounds;
            Vector3 detectionCenter = bounds.center;
            Vector3 detectionSize = bounds.size;

            // Get all colliders in the detection area
            Collider[] collidersInSlot = Physics.OverlapBox(detectionCenter, detectionSize / 2, Quaternion.identity);

            // Check if any of the colliders belong to a car (by tag)
            foreach (Collider collider in collidersInSlot)
            {
                if (collider.CompareTag("Car"))
                {
                    _isOccupied = true;
                    break;
                }
            }
        }
        
        // void OnDrawGizmos()
        // {
        //     if (parkingSlotCollider != null)
        //     {
        //         Gizmos.color = Color.yellow;
        //         Gizmos.DrawWireCube(parkingSlotCollider.bounds.center, parkingSlotCollider.bounds.size);
        //     }
        // }
    }
}