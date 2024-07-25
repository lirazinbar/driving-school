using UnityEngine;
using UnityEngine.Serialization;

namespace TrafficObjects.GiveWay
{
    public class StopSign: MonoBehaviour
    {
        [SerializeField] private StopLine stopLine;
        [SerializeField] private StopSurfaceDetector stopSurfaceDetector;
        
        public StopLine GetStopLine()
        {
            return stopLine;
        }
        
        public StopSurfaceDetector GetStopSurfaceDetector()
        {
            return stopSurfaceDetector;
        }
    }
}