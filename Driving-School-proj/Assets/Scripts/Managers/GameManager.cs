using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Cars;
using Enums;
using TMPro;
using TrafficObjects.GiveWay;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private bool isDefaultRoute;
        [SerializeField] private GameObject mainCar;
        [SerializeField] private Light directionalLight;
        [SerializeField] private InstructorAnimationController instructorAnimationController;
        [SerializeField] private CreateRouteMap routeMap;
        
        [Header("Game Over Menu")]
        [SerializeField] private TMP_Text successStatus;
        [SerializeField] private GameObject scoreComponentPrefab;
        [SerializeField] private GameObject gridContainerGameOverMenu;
        [SerializeField] private Canvas gameOverCanvas;
        [SerializeField] private Canvas gamePauseCanvas;
        [SerializeField] private GameObject gameOverCanvasPosition;
        

        private bool isGamePause = false;
        private GameSettings.GameSettings gameSettings { get; set; }

        //private string playerName;
        void Awake()
        {
            // Singleton
            Instance = this;
            
            Application.targetFrameRate = 90;
            
            LoadGameSettings();
        }

        private void Start()
        {
            if (isDefaultRoute)
            {
                SetGameSettings();
            }
            else
            {
                StartCoroutine(WaitForComponentsCreationAndSetSettings());
            }
        }

        private void Update()
        {
            if (OVRInput.GetUp(OVRInput.RawButton.X))
            {
                isGamePause = true;
                mainCar.gameObject.GetComponent<CarController>().SetIsGamePause(true);
                gamePauseCanvas.gameObject.SetActive(true);
            }
        }

        public bool IsMainCar(int carId)
        {
            return carId == mainCar.GetInstanceID();
        }
        
        public bool IsDefaultRoute()
        {
            return isDefaultRoute;
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
                    FeedbackManager.Instance.UpdateScore("Stop Sign");
                }
            }
        }
    
        public void UpdateNoEntrySignEvent(int carId)
        {
            if (carId == mainCar.GetInstanceID())
            {
                Debug.Log("Main car passed no entry sign");
                FeedbackManager.Instance.UpdateScore("No Entry Sign");
            }
        }
    
        public void UpdateCarPassedRedLightEvent(int carId)
        {
            if (carId == mainCar.GetInstanceID())
            {
                Debug.Log("Main car passed red light");
                FeedbackManager.Instance.UpdateScore("Red Light");
            }
        }
    
        public void UpdateCarBrokeSpeedLimitEvent()
        {
            Debug.Log("Main car broke speed limit");
            FeedbackManager.Instance.UpdateScore("Speed Limit");
        }
        
        public void UpdateCarDidNotGiveWayEvent()
        {
            Debug.Log("Main car did not give way");
            FeedbackManager.Instance.UpdateScore("Give Way");
        }
        
        public void UpdateCarDidNotGiveWayToPedestrianEvent()
        {
            Debug.Log("Main car did not give way to pedestrian");
            FeedbackManager.Instance.UpdateScore("Give Way Pedestrian");
        }
        public void UpdateCarHitOtherCarEvent()
        {
            Debug.Log("Main car hit another car");
            AudioManager.Instance.Play("CarCrash");
            FeedbackManager.Instance.UpdateScore("Car Hit");
        }
        
        public void UpdateCarHitPedestrianEvent()
        {
            Debug.Log("Main car hit a pedestrian");
            AudioManager.Instance.Play("CarCrash");
            FeedbackManager.Instance.UpdateScore("Pedestrian Hit");
        }

        public void UpdateCarHitObstacleEvent()
        {
            Debug.Log("Main car hit an obstacle");
            AudioManager.Instance.Play("CarCrash");
            FeedbackManager.Instance.UpdateScore("Obstacle Hit");
        }

        public void UpdateCarTookWrongTurnEvent()
        {
            Debug.Log("Car took wrong turn");
            FeedbackManager.Instance.UpdateScore("Wrong Direction");
        }
        
        public void UpdateCarTurnedOverEvent()
        {
            Debug.Log("Car turned over");
            FeedbackManager.Instance.UpdateScore("Car Turned Over");
        }
        
        public void GameFinished(bool success, List<string> feedbackScores)
        {
            FeedbackManager.Instance.SetIsUpdatingScore(false);
            if (success)
            {
                Debug.Log("Congratulations! You passed the test!");
                AudioManager.Instance.Play("Arrive");
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
            
            scoresObject._feedbackTables.Add(new FeedbackTable(feedbackScores));

            if (foundScoresObject == null)
            {
                scoresList.Add(scoresObject);
            }
            XMLManager.Instance.SaveScores(scoresList);
            
            StartCoroutine(DisplayGameOverAfterDelay(success, feedbackScores));
        }

        private void DisplayGameOver(bool success, List<string> feedbackScores)
        {
            gameOverCanvas.gameObject.SetActive(true);
            isGamePause = true;
            mainCar.gameObject.GetComponent<CarController>().SetIsGamePause(true);
            
            gameOverCanvas.transform.position = gameOverCanvasPosition.transform.position;
            gameOverCanvas.transform.rotation = gameOverCanvasPosition.transform.rotation;
            
            int sumScores = 100;
            for (int index = 0; index < feedbackScores.Count; index++)
            {
                string feedback = feedbackScores[index];
                int score = FeedbackScore.Table[feedbackScores[index]];
                GameObject newComponent = Instantiate(scoreComponentPrefab, gridContainerGameOverMenu.transform);
                
                newComponent.transform.GetComponent<TextMeshProUGUI>().text = feedback + "    " + score;
                sumScores += score;
                
                newComponent.name = "ScoreLine" + (index+1);
            }

            if (success)
            {
                AudioManager.Instance.Play("GameWon");
                successStatus.SetText("You Win!  scores: " + sumScores);
            }
            else
            {
                AudioManager.Instance.Play("GameLost");
                successStatus.SetText("You Lost!  scores: " + sumScores);
            }
        }
        
        private IEnumerator<WaitForSeconds> DisplayGameOverAfterDelay(bool success, List<string> feedbackScores)
        {
            yield return new WaitForSeconds(2f);
            DisplayGameOver(success, feedbackScores);
        }

        public void OnGoBackToMainMenu()
        {
            gameOverCanvas.gameObject.SetActive(false);
            gamePauseCanvas.gameObject.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuVR");
        }

        public void OnCloseGamePause()
        {
            gamePauseCanvas.gameObject.SetActive(false);
            isGamePause = false;
            mainCar.gameObject.GetComponent<CarController>().SetIsGamePause(false);
        }

        private void LoadGameSettings()
        {
            gameSettings = GetGameSettingsFromPlayerPrefs();
            // dev: 
            // gameSettings = new GameSettings.GameSettings(PedestrianDifficulty.Medium, CarsDifficulty.Medium, false, 3, true);
        }
        
        private void SetGameSettings()
        {
           SetPedestrianDifficulty(gameSettings.GetPedestrianDifficulty());
           
           SetCarsDifficulty(gameSettings.GetCarsDifficulty());
           
           SetNightMode(gameSettings.GetNightMode());

           SetTurnsToWin(gameSettings.GetTurnsToWin());
           
           SetInstructor(gameSettings.GetInstructor());
        }
        
        private IEnumerator WaitForComponentsCreationAndSetSettings()
        {
            bool conditionMet = routeMap.RouteCreationFinished();
            while (!conditionMet)
            {
                yield return new WaitForSeconds(0.1f);
                conditionMet = routeMap.RouteCreationFinished();
            }
            
            SetGameSettings();
        }
        
        private GameSettings.GameSettings GetGameSettingsFromPlayerPrefs()
        {
            PedestrianDifficulty pedestrianDifficulty = (PedestrianDifficulty) PlayerPrefs.GetInt("PedestrianDifficulty");
            CarsDifficulty carsDifficulty = (CarsDifficulty) PlayerPrefs.GetInt("CarsDifficulty");
            bool nightMode = PlayerPrefs.GetInt("NightMode") == 1;
            int numberOfTurnsToWin = PlayerPrefs.GetInt("NumberOfTurnsToWin");
            bool instructor = PlayerPrefs.GetInt("Instructor") == 1;
            
            return new GameSettings.GameSettings(pedestrianDifficulty, carsDifficulty, nightMode, numberOfTurnsToWin, instructor);
        }
        
        public void SetPedestrianDifficulty(PedestrianDifficulty pedestrianDifficulty)
        {
            JunctionGiveWayManager[] junctionManagers = FindObjectsOfType<JunctionGiveWayManager>();
            
            foreach (JunctionGiveWayManager junctionManager in junctionManagers)
            {
                junctionManager.SetPedestrianDifficulty(pedestrianDifficulty);
            }
        }
        
        public void SetCarsDifficulty(CarsDifficulty carsDifficulty)
        {
            AutonomousCarSpawn[] autonomousCarSpawns = FindObjectsOfType<AutonomousCarSpawn>();
            
            foreach (AutonomousCarSpawn autonomousCarSpawn in autonomousCarSpawns)
            {
                autonomousCarSpawn.SetSpawnInterval((float)carsDifficulty);
            }
        }
        
        private void SetNightMode(bool nightMode)
        {
            if (nightMode)
            {
                directionalLight.gameObject.SetActive(false);
                RenderSettings.skybox = null;
                DynamicGI.UpdateEnvironment();

                StreetLight[] streetLights = FindObjectsOfType<StreetLight>();
                foreach (StreetLight streetLight in streetLights)
                {
                    streetLight.TurnOn();
                }
            }
        }
        
        public bool IsNightMode()
        {
            return gameSettings.GetNightMode();
        }
        
        private void SetTurnsToWin(int turnsToWin)
        {
            FeedbackManager.Instance.SetTurnsToWin(turnsToWin);
        }
        
        private void SetInstructor(bool instructor)
        {
            if (instructor)
            {
                instructorAnimationController.StartDriveTalk();
            }
            else
            {
                instructorAnimationController.gameObject.SetActive(false);
            }
        }

    }
}
