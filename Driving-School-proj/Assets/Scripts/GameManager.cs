using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject mainCar;
    void Awake()
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

    public void UpdateStopSignEvent(GameObject car, bool carStopped)
    {
        if (car.GetInstanceID() == mainCar.GetInstanceID())
        {
            if (carStopped)
            {
                Debug.Log("Main car stopped before the stop sign.");
            }
            else
            {
                Debug.Log("Main car passed the stop sign without stopping.");
            }
        }
    }
    
    public void UpdateNoEntrySignEvent(int carId)
    {
        if (carId == mainCar.GetInstanceID())
        {
            Debug.Log("Main car passed the no entry sign.");
            ScoreManager.Instance.UpdateScore("NoEntry");
        }
    }
    
    public void UpdateCarPassedRedLightEvent(int carId)
    {
        if (carId == mainCar.GetInstanceID())
        {
            Debug.Log("Main car passed the red light.");
            ScoreManager.Instance.UpdateScore("RedLight");
            // Update UI
        }
    }
}
