using Managers;
using TrafficObjects;
using UnityEngine;

namespace Pedestrian
{
    public class PedestriansCrossingStopCollider: MonoBehaviour
    {
        private bool _pedestrianIsCrossing;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                {
                    string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                    if (hitSide.Equals("Back") && _pedestrianIsCrossing)
                    {
                        EventsManager.Instance.TriggerCarDidNotGiveWayToPedestrianEvent();
                    }
                }
            }
        }
        
        public void SetPedestrianIsCrossing(bool isCrossing)
        {
            _pedestrianIsCrossing = isCrossing;
            if (_pedestrianIsCrossing)
            {
                gameObject.layer = LayerMask.NameToLayer("PedestriansCrossing");
            }
            else
            {
                // Ignored by all cars raycast
                gameObject.layer = LayerMask.NameToLayer("PedestriansNotCrossing");
            }
        }
    }
}