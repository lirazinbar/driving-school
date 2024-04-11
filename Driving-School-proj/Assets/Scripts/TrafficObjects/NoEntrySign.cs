using UnityEngine;

namespace TrafficObjects
{
    public class NoEntrySign : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Car"))
            {
                Debug.Log("NoEntrySign triggered.");
                EventsManager.Instance.TriggerCarPassedNoEntrySignEvent(other.gameObject.GetInstanceID());
            }
        }
    }
}
