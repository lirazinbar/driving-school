using UnityEngine;
using UnityEngine.Serialization;

namespace TrafficObjects.GiveWay {
    public class JunctionGiveWayManager: MonoBehaviour
    {
        [SerializeField] StopSign[] stopSigns;
        private bool _giveWayLock;
        [SerializeField] private bool isVisible = true;
        
        public void Start()
        {
            foreach (StopSign stopSign in stopSigns)
            {
                stopSign.SetIsVisible(isVisible);
            }
        }
        
        public bool TryTakeLock()
        {
            if (_giveWayLock)
            {
                return false;
            }
            _giveWayLock = true;
            return true;
        }
        
        public void YieldLock()
        {
            Debug.Log("Sign has yielded lock");
            _giveWayLock = false;
        }
        
        public bool IsLockAvailable()
        {
            return !_giveWayLock;
        }
    }
}