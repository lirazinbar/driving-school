using Enums;
using Unity.AI.Navigation;
using UnityEngine;

namespace TrafficObjects.GiveWay {
    public class JunctionGiveWayManager: MonoBehaviour
    {
        StopSign[] stopSigns;
        private NavMeshSurface navMeshSurface;

        [SerializeField] private bool isVisible = true;
        
        private bool _giveWayLock;
        
        public void Start()
        {
            stopSigns = GetComponentsInChildren<StopSign>();
            navMeshSurface = GetComponent<NavMeshSurface>();
            
            foreach (StopSign stopSign in stopSigns)
            {
                stopSign.SetIsVisible(isVisible);
            }
            
            navMeshSurface.BuildNavMesh();
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
        
        public void SetPedestrianDifficulty(PedestrianDifficulty pedestrianDifficulty)
        {
            if (stopSigns == null)
            {
                stopSigns = GetComponentsInChildren<StopSign>();
            }

            foreach (StopSign stopSign in stopSigns)
            {
                stopSign.SetDifficulty(pedestrianDifficulty);
            }
        }
    }
}