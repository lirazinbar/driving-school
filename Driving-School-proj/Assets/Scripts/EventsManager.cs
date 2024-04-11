using UnityEngine;
using UnityEngine.Events;


public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance { get; private set; }
    
    [System.Serializable]
    public class CarReachedStopSignEvent : UnityEvent<GameObject, int> {}
    
    public CarReachedStopSignEvent carReachedStopSignEvent = new CarReachedStopSignEvent();
    public UnityEvent<int> carPassedStopSignEvent = new UnityEvent<int>();
    public UnityEvent<int> carStoppedBeforeStopSignEvent = new UnityEvent<int>();
    public UnityEvent<int> carPassedNoEntrySignEvent = new UnityEvent<int>();
    public UnityEvent<int> carPassedRedLightEvent = new UnityEvent<int>();

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
    
    public void TriggerCarReachedStopSignEvent(GameObject car, int stopSignId)
    {
        carReachedStopSignEvent?.Invoke(car, stopSignId);
    }

    public void TriggerCarPassedStopSignEvent(int stopSignId)
    {
        carPassedStopSignEvent?.Invoke(stopSignId);
    }

    public void TriggerCarStoppedBeforeStopSignEvent(int stopSignId)
    {
        carStoppedBeforeStopSignEvent?.Invoke(stopSignId);
    }
    
    public void TriggerCarPassedNoEntrySignEvent(int carId)
    {
        carPassedNoEntrySignEvent?.Invoke(carId);
    }
    
    public void TriggerCarPassedRedLightEvent(int carId)
    {
        carPassedRedLightEvent?.Invoke(carId);
    }
}