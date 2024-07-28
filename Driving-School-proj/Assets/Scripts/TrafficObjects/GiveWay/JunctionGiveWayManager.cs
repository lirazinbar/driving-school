using UnityEngine;

namespace TrafficObjects.GiveWay {
    public class JunctionGiveWayManager: MonoBehaviour
    {
        private bool _giveWayLock;
        
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