using UnityEngine;
using UnityEngine.Serialization;

namespace TrafficObjects.GiveWay {
    public class JunctionGiveWayManager: MonoBehaviour
    {
        [SerializeField] private StopSign[] stopSigns;
        private bool _giveWayLock;
        
        
    }
}