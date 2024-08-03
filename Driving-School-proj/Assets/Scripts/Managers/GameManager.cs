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
            Instance = this;
        }

        public void UpdateStopSignEvent(int carId, bool carStopped)
        {
            if (carId == mainCar.GetInstanceID())
            {
                if (carStopped)
                {
                    Debug.Log("Main car stopped before the stop sign");
                }
                else
                {
                    Debug.Log("Main car passed stop sign without stopping");
                    FeedbackManager.Instance.UpdateScore(FeedbackScore.StopSign);
                }
            }
        }
    
        public void UpdateNoEntrySignEvent(int carId)
        {
            if (carId == mainCar.GetInstanceID())
            {
                Debug.Log("Main car passed no entry sign");
                FeedbackManager.Instance.UpdateScore(FeedbackScore.NoEntry);
            }
        }
    
        public void UpdateCarPassedRedLightEvent(int carId)
        {
            if (carId == mainCar.GetInstanceID())
            {
                Debug.Log("Main car passed red light");
                FeedbackManager.Instance.UpdateScore(FeedbackScore.RedLight);
            }
        }
    
        public void UpdateCarBrokeSpeedLimitEvent()
        {
            Debug.Log("Main car broke speed limit");
            FeedbackManager.Instance.UpdateScore(FeedbackScore.Speeding);
        }

        public bool IsMainCar(int carId)
        {
            return carId == mainCar.GetInstanceID();
        }
        
        public void UpdateCarDidNotGiveWayEvent()
        {
            Debug.Log("Main car did not give way");
            FeedbackManager.Instance.UpdateScore(FeedbackScore.GiveWay);
        }
        
        public void GameFinished(bool success)
        {
            if (success)
            {
                Debug.Log("Congratulations! You passed the test!");
            }
            else
            {
                Debug.Log("Game Over! You failed the test!");
            }
            // UI: Show lost/pass the test + score table + input for the name
        }
    }
}
