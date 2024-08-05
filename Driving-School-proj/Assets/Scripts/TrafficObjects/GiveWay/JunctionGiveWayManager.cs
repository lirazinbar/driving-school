using UnityEngine;

namespace TrafficObjects.GiveWay {
    public class JunctionGiveWayManager: MonoBehaviour
    {
        [SerializeField] StopSign[] stopSigns;
        private bool _giveWayLock;
        [SerializeField] private bool isVisible = true;
        [SerializeField] private bool pedestriansSpawn = true;
        
        public void Start()
        {
            foreach (StopSign stopSign in stopSigns)
            {
                stopSign.SetIsVisible(isVisible);
                if (!pedestriansSpawn)
                {
                    stopSign.DoNotSpawnPedestrians();
                }
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
            _giveWayLock = false;
        }
        
        public bool IsLockAvailable()
        {
            return !_giveWayLock;
        }
    }
}