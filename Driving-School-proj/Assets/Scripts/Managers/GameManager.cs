using System.Collections.Generic;
using Audio;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private GameObject mainCar;
        [SerializeField] private TMP_Text successStatus;
        [SerializeField] private GameObject scoreComponentPrefab;
        [SerializeField] private GameObject gridContainerGameOverMenu;
        [SerializeField] private Canvas GameOverCanvas; 
        private AudioManager audioManager;

        //private string playerName;
        void Awake()
        {
            // Singleton
            Instance = this;
            Application.targetFrameRate = 90;
        }
        
        void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        
        public bool IsMainCar(int carId)
        {
            return carId == mainCar.GetInstanceID();
        }

        //public void SetPlayerName(string playerNameInput)
        //{
        //    this.playerName = playerNameInput;
        //    Debug.Log("name: " + this.playerName);
        //}

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
            Debug.Log("red lightt");
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
        
        public void UpdateCarDidNotGiveWayEvent()
        {
            Debug.Log("Main car did not give way");
            FeedbackManager.Instance.UpdateScore(FeedbackScore.GiveWay);
        }
        
        public void UpdateCarDidNotGiveWayToPedestrianEvent()
        {
            Debug.Log("Main car did not give way to pedestrian");
            FeedbackManager.Instance.UpdateScore(FeedbackScore.GiveWayPedestrian);
        }
        public void UpdateCarHitOtherCarEvent()
        {
            Debug.Log("Main car hit another car");
            audioManager.Play("CarCrash");
            FeedbackManager.Instance.UpdateScore(FeedbackScore.CarHit);
        }
        
        public void UpdateCarHitPedestrianEvent()
        {
            Debug.Log("Main car hit a pedestrian");
            audioManager.Play("CarCrash");
            FeedbackManager.Instance.UpdateScore(FeedbackScore.PedestrianHit);
        }
        
        public void GameFinished(bool success, List<FeedbackScore> _feedbackScores)
        {
            if (success)
            {
                Debug.Log("Congratulations! You passed the test!");
            }
            else
            {
                Debug.Log("Game Over! You failed the test!");
            }
            string playerName = PlayerPrefs.GetString("PlayerName");

            List<ScoresObject> scoresList = XMLManager.Instance.LoadScores();
            ScoresObject foundScoresObject = scoresList.Find((scoresObj => scoresObj.playerName == playerName));
            ScoresObject scoresObject;
            
            
            Debug.Log("finish - "+ playerName);
            if (foundScoresObject != null)
            {
                scoresObject = foundScoresObject;
            }
            else
            {
                scoresObject = new ScoresObject(playerName, new List<FeedbackTable>()); 
            }
            
            scoresObject._feedbackTables.Add(new FeedbackTable(_feedbackScores));

            if (foundScoresObject == null)
            {
                scoresList.Add(scoresObject);
            }
            XMLManager.Instance.SaveScores(scoresList);
            
            StartCoroutine(DisplayGameOverAfterDelay(success, _feedbackScores));
        }

        private void DisplayGameOver(bool success, List<FeedbackScore> _feedbackScores)
        {
            GameOverCanvas.gameObject.SetActive(true);
            int sumScores = 100;
            for (int index = 0; index < _feedbackScores.Count; index++)
            {
                FeedbackScore score = _feedbackScores[index];
                GameObject newComponent = Instantiate(scoreComponentPrefab, gridContainerGameOverMenu.transform);
                
                newComponent.transform.GetComponent<TextMeshProUGUI>().text = score.ToString() + "    " + (int)score;
                sumScores += (int)score;
                
                newComponent.name = "ScoreLine" + (index+1);
            }

            if (success)
            {
                successStatus.SetText("You Win!  scores: " + sumScores);
            }
            else
            {
                successStatus.SetText("You Lost!  scores: " + sumScores);
            }
            Time.timeScale = 0f; // Pause the game
            // Debug.Log("Enddd");
        }
        
        private IEnumerator<WaitForSeconds> DisplayGameOverAfterDelay(bool success, List<FeedbackScore> _feedbackScores)
        {
            yield return new WaitForSeconds(2f);
            DisplayGameOver(success, _feedbackScores);
        }

        public void OnGoBackToMainMenu()
        {
            // Debug.Log("mainnnn");
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
