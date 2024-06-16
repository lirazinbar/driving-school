using Cars;
using Enums;
using UnityEngine;
using UnityEngine.Events;


namespace Managers
{
    public class EventsManager : MonoBehaviour
    {
        public static EventsManager Instance { get; private set; }
    
        [System.Serializable]
        public class CarReachedStopSignEvent : UnityEvent<int, int> {}
    
        public CarReachedStopSignEvent carReachedStopSignEvent = new CarReachedStopSignEvent();
        public UnityEvent<int> carPassedStopSignEvent = new UnityEvent<int>();
        public UnityEvent<int> carStoppedBeforeStopSignEvent = new UnityEvent<int>();
        public UnityEvent<GearState> carGearStateChangedEvent = new UnityEvent<GearState>();
        public UnityEvent<CrossSectionDirections> carEnteredCrossSectionEvent = new UnityEvent<CrossSectionDirections>();
        public UnityEvent<int> carPassedNoEntrySignEvent = new UnityEvent<int>();
        public UnityEvent<int> carPassedRedLightEvent = new UnityEvent<int>();
        public UnityEvent carBrokeSpeedLimitEvent = new UnityEvent();

        private void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        public void TriggerCarReachedStopSignEvent(int carId, int stopSignId)
        {
            carReachedStopSignEvent?.Invoke(carId, stopSignId);
        }

        public void TriggerCarPassedStopSignEvent(int stopSignId)
        {
            carPassedStopSignEvent?.Invoke(stopSignId);
        }

        public void TriggerCarStoppedBeforeStopSignEvent(int stopSignId)
        {
            carStoppedBeforeStopSignEvent?.Invoke(stopSignId);
        }
    
        public void TriggerCarGearStateChangedEvent(GearState gearState)
        {
            carGearStateChangedEvent?.Invoke(gearState);
        }
    
        public void TriggerCarEnteredCrossSectionEvent(CrossSectionDirections direction)
        {
            carEnteredCrossSectionEvent?.Invoke(direction);
        }

    
        public void TriggerCarPassedNoEntrySignEvent(int carId)
        {
            carPassedNoEntrySignEvent?.Invoke(carId);
        }
    
        public void TriggerCarPassedRedLightEvent(int carId)
        {
            carPassedRedLightEvent?.Invoke(carId);
        }
    
        public void TriggerCarBrokeSpeedLimitEvent()
        {
            carBrokeSpeedLimitEvent?.Invoke();
        }
    }
}