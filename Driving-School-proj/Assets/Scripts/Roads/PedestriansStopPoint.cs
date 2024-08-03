using System;
using UnityEngine;

namespace Roads
{
    public class PedestriansStopPoint: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pedestrian"))
            {
                if ()
            }
        }
    }
}