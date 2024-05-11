using UnityEngine;

namespace Managers
{
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

        public void UpdateStopSignEvent(int carId, bool carStopped)
        {
            if (carId == mainCar.GetInstanceID())
            {
                if (carStopped)
                {
                    Debug.Log("Main car stopped before the stop sign.");
                }
                else
                {
                    Debug.Log("Main car passed stop sign without stopping.");
                    FeedbackManager.Instance.UpdateScore(FeedbackScore.StopSign);
                }
            }
        }
    
        public void UpdateNoEntrySignEvent(int carId)
        {
            if (carId == mainCar.GetInstanceID())
            {
                Debug.Log("Main car passed no entry sign.");
                FeedbackManager.Instance.UpdateScore(FeedbackScore.NoEntry);
            }
        }
    
        public void UpdateCarPassedRedLightEvent(int carId)
        {
            if (carId == mainCar.GetInstanceID())
            {
                Debug.Log("Main car passed red light.");
                FeedbackManager.Instance.UpdateScore(FeedbackScore.RedLight);
            }
        }
    
        public void UpdateCarBrokeSpeedLimitEvent()
        {
            Debug.Log("Main car broke speed limit.");
            FeedbackManager.Instance.UpdateScore(FeedbackScore.Speeding);
        }
    }
}
