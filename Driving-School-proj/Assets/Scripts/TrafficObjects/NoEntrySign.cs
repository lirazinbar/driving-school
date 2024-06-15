using Managers;
using UnityEngine;

namespace TrafficObjects
{
    public class NoEntrySign : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                if (GameManager.Instance.IsMainCar(other.gameObject.GetInstanceID()))
                {
                    Debug.Log("Main car reached no entry sign");
                }
                
                string hitSide = TrafficObjectsUtils.CheckHitSide(transform, other);
                Debug.Log("Hit side: " + hitSide);
                if (hitSide.Equals("Front"))
                {
                    EventsManager.Instance.TriggerCarPassedNoEntrySignEvent(other.gameObject.GetInstanceID());
                }
            }
        }
    }
}
