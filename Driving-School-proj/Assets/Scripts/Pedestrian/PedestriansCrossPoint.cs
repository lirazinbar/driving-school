using TrafficObjects.GiveWay;
using UnityEngine;

namespace Pedestrian
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
                   pedestrianController.SetIsCrossing(true, stopSign);
                }
                else if (pedestrianController.SpawnerId() == pedestriansFarSpawner.GetInstanceID())
                {
                    // Pedestrian finished crossing
                    stopSign.PedestrianFinishCrossing();
                    pedestrianController.SetIsCrossing(false, null);
                }
            }
        }
    }
}