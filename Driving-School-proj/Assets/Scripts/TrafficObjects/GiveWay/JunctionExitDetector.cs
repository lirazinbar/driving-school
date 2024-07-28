using System;
using Cars;
using Managers;
using UnityEngine;

namespace TrafficObjects.GiveWay
{
    public class JunctionExitDetector: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                Debug.Log("Car Exited junction: " + other.GetComponent<CarController>().GetInstanceID());
                EventsManager.Instance.TriggerCarExitedJunctionEvent(other.GetComponent<CarController>().GetInstanceID());
            }
        }
    }
}