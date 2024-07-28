using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public class TrafficManager : MonoBehaviour
    {
        public static TrafficManager Instance { get; private set; }
        private Dictionary<int, (bool carStopped, bool carPassed)> _stopSignObjects = new Dictionary<int, (bool, bool)>();
        [SerializeField] private int speedLimit = 50;

        void Awake()
        {
            // Singleton
            Instance = this;
        }
    
        private void Start()
        {
            EventsManager.Instance.carReachedStopSignEvent.AddListener(OnCarReachedStopSign);
            EventsManager.Instance.carPassedStopSignEvent.AddListener(OnCarPassedStopSign);
            EventsManager.Instance.carStoppedBeforeStopSignEvent.AddListener(OnCarStoppedBeforeStopSign);
            EventsManager.Instance.carPassedNoEntrySignEvent.AddListener(OnCarPassedNoEntrySign);
            EventsManager.Instance.carPassedRedLightEvent.AddListener(OnCarPassedRedLight);
            EventsManager.Instance.carBrokeSpeedLimitEvent.AddListener(OnCarBrokenSpeedLimit);
            EventsManager.Instance.carDidNotGiveWayEvent.AddListener(OnCarDidNotGiveWay);
        }

        private void OnDestroy()
        {
            EventsManager.Instance.carReachedStopSignEvent.RemoveListener(OnCarReachedStopSign);
            EventsManager.Instance.carPassedStopSignEvent.RemoveListener(OnCarPassedStopSign);
            EventsManager.Instance.carStoppedBeforeStopSignEvent.RemoveListener(OnCarStoppedBeforeStopSign);
            EventsManager.Instance.carPassedNoEntrySignEvent.RemoveListener(OnCarPassedNoEntrySign);
            EventsManager.Instance.carPassedRedLightEvent.RemoveListener(OnCarPassedRedLight);
            EventsManager.Instance.carBrokeSpeedLimitEvent.RemoveListener(OnCarBrokenSpeedLimit);
        }
    
        public int GetSpeedLimit()
        {
            return speedLimit;
        }

        private async void OnCarReachedStopSign(int carId, int stopSignId)
        {
            // Debug.Log("Car reached stop sign event triggered");
            _stopSignObjects[stopSignId] = (false, false);
        
            await WaitForConditionsAsync(carId, stopSignId);
        }
    
        private void OnCarPassedStopSign(int stopSignId)
        {
            // Debug.Log("Car passed stop sign event triggered");
            _stopSignObjects[stopSignId] = (_stopSignObjects[stopSignId].carStopped, true);
        }

        private void OnCarStoppedBeforeStopSign(int stopSignId)
        {
            // Debug.Log("Car stopped at stop sign event triggered");
            _stopSignObjects[stopSignId] = (true, _stopSignObjects[stopSignId].carPassed);
        }
    
        private async Task WaitForConditionsAsync(int carId, int stopSignId)
        {
            // Wait until either the car passes the stop sign or the car stops
            while (!(_stopSignObjects[stopSignId].carStopped || _stopSignObjects[stopSignId].carPassed))
            {
                await Task.Delay(100); // Adjust the delay as needed
            }
        
            GameManager.Instance.UpdateStopSignEvent(carId, _stopSignObjects[stopSignId].carStopped);
        }
    
        private void OnCarPassedNoEntrySign(int carId)
        {
            // Debug.Log("Car passed the no entry sign event triggered");
            GameManager.Instance.UpdateNoEntrySignEvent(carId);
        }
    
        private void OnCarPassedRedLight(int carId)
        {
            // Debug.Log("Car passed the red traffic light event triggered");
            GameManager.Instance.UpdateCarPassedRedLightEvent(carId);
        }
    
        private void OnCarBrokenSpeedLimit()
        {
            // Debug.Log("Car broke the speed limit event triggered");
            GameManager.Instance.UpdateCarBrokeSpeedLimitEvent();
        }
        
        private void OnCarDidNotGiveWay()
        {
            // Debug.Log("Car did not give way event triggered");
            GameManager.Instance.UpdateCarDidNotGiveWayEvent();
        }
    }
}
