using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TrafficManager : MonoBehaviour
{
    public static TrafficManager Instance { get; private set; }
    private Dictionary<int, (bool carStopped, bool carPassed)> _stopSignObjects = new Dictionary<int, (bool, bool)>();

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
    
    private void Start()
    {
        EventsManager.Instance.carReachedStopSignEvent.AddListener(OnCarReachedStopSign);
        EventsManager.Instance.carPassedStopSignEvent.AddListener(OnCarPassedStopSign);
        EventsManager.Instance.carStoppedBeforeStopSignEvent.AddListener(OnCarStoppedBeforeStopSign);
        
    }

    private void OnDestroy()
    {
        // foreach (StopSign stopSign in _stopSignObjects)
        {
            // Unsubscribe the events triggered by each stop sign
            EventsManager.Instance.carReachedStopSignEvent.RemoveListener(OnCarReachedStopSign);
            EventsManager.Instance.carPassedStopSignEvent.RemoveListener(OnCarPassedStopSign);
            EventsManager.Instance.carStoppedBeforeStopSignEvent.RemoveListener(OnCarStoppedBeforeStopSign);
        }
    }

    private async void OnCarReachedStopSign(GameObject car, int stopSignId)
    {
        Debug.Log("Car reached the stop sign.");
        _stopSignObjects[stopSignId] = (false, false);
        
        await WaitForConditionsAsync(car, stopSignId);
    }
    
    private void OnCarPassedStopSign(int stopSignId)
    {
        Debug.Log("Car passed the stop sign.");
        var (carStopped, carPassed) = _stopSignObjects[stopSignId];
        _stopSignObjects[stopSignId] = (carStopped, true);
    }

    private void OnCarStoppedBeforeStopSign(int stopSignId)
    {
        Debug.Log("Car stopped at the stop sign.");
        var (carStopped, carPassed) = _stopSignObjects[stopSignId];
        _stopSignObjects[stopSignId] = (true, carPassed);
    }
    
    private async Task WaitForConditionsAsync(GameObject car, int stopSignId)
    {
        // Wait until either the car passes the stop sign or the car stops
        while (!(_stopSignObjects[stopSignId].carStopped || _stopSignObjects[stopSignId].carPassed))
        {
            await Task.Delay(100); // Adjust the delay as needed
        }
        
        GameManager.Instance.UpdateStopSignEvent(car, _stopSignObjects[stopSignId].carStopped);
    }
    
    private void CarPassedNoEntrySign(GameObject car)
    {
        Debug.Log("Car passed the no entry sign.");
        GameManager.Instance.UpdateNoEntrySignEvent(car);
    }
}
