using TrafficObjects.GiveWay;
using UnityEngine;

namespace Roads
{
    public class PedestriansCrossPoint: MonoBehaviour
    {
        
        [SerializeField] private GameObject pedestriansCloseSpawner;
        [SerializeField] private GameObject pedestriansFarSpawner;
        [SerializeField] private StopSign stopSign;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pedestrian"))
            {
                PedestrianController pedestrianController = other.GetComponent<PedestrianController>();
                if (pedestrianController.SpawnerId() == pedestriansCloseSpawner.GetInstanceID())
                {
                   // Pedestrians starts crossing
                   stopSign.PedestrianStartCrossing();
                }
                else if (pedestrianController.SpawnerId() == pedestriansFarSpawner.GetInstanceID())
                {
                    // Pedestrian finished crossing
                    stopSign.PedestrianFinishCrossing();
                }
            }
        }
    }
}