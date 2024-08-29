using Cars;
using TMPro;
using UnityEngine;

namespace ParkingTest
{
    public class ParkingTestGameManager : MonoBehaviour
    {
        public static ParkingTestGameManager Instance { get; private set; }
        
        private ParkingSlotPerpendicular[] _parkingSlotsPerpendicular;
        private ParkingSlotParallel[] _parkingSlotsParallel;
        
        private ParkingTestSettings _settings;
        private int _parkingsCompleted;
        [SerializeField] private Canvas gameOverCanvas; 
        [SerializeField] private Canvas gamePauseCanvas; 
        [SerializeField] TMP_Text gameStatus;
        [SerializeField] Transform gameOverCanvasPosition;
        [SerializeField] GameObject PlayerCar;

        private void Awake()
        {
            // Singleton
            Instance = this;
            
            Application.targetFrameRate = 90;
            
            LoadGameSettings();
        }

        private void Start()
        {
            FindAllFreeParkingSlots();

            SetRandomParkingSlotAsTarget(_settings.GetParkingType());
        }
        
        private void Update()
        {
            if (OVRInput.GetUp(OVRInput.RawButton.X))
            {
                PlayerCar.gameObject.GetComponent<CarController>().SetIsGamePause(true);
                gamePauseCanvas.gameObject.SetActive(true);
            }
        }
        
        public void OnCloseGamePause()
        {
            gamePauseCanvas.gameObject.SetActive(false);
            PlayerCar.gameObject.GetComponent<CarController>().SetIsGamePause(false);
        }
        
        private void LoadGameSettings()
        {
            _settings = GetSettingsFromPlayerPrefs();
        }

        private ParkingTestSettings GetSettingsFromPlayerPrefs()
        {
            int parkingsToWin = PlayerPrefs.GetInt("ParkingsToWin");
            ParkingType parkingType = (ParkingType) PlayerPrefs.GetInt("ParkingType");
            
            return new ParkingTestSettings(parkingsToWin, parkingType);
        }
        
        private void FindAllFreeParkingSlots()
        {
            _parkingSlotsPerpendicular = FindObjectsOfType<ParkingSlotPerpendicular>();
            _parkingSlotsParallel = FindObjectsOfType<ParkingSlotParallel>();
            // Debug.Log("Perpendicular Parking Slots: " + _parkingSlotsPerpendicular.Length);
            
            // Filter out the occupied parking slots
            _parkingSlotsPerpendicular = System.Array.FindAll(_parkingSlotsPerpendicular, slot => !slot.IsOccupied());
            _parkingSlotsParallel = System.Array.FindAll(_parkingSlotsParallel, slot => !slot.IsOccupied());
            // Debug.Log("Free Perpendicular Parking Slots: " + _parkingSlotsPerpendicular.Length);
        }
        
        private void SetRandomParkingSlotAsTarget(ParkingType parkingType)
        {
            ParkingSlot parkingSlot = GetRandomParkingSlot(parkingType);
            parkingSlot.SetSlotAsTarget(true);
        }
        
        private ParkingSlot GetRandomParkingSlot(ParkingType parkingType)
        {
            if (parkingType == ParkingType.Perpendicular)
            {
                return _parkingSlotsPerpendicular[Random.Range(0, _parkingSlotsPerpendicular.Length)];
            }
            if (parkingType == ParkingType.Parallel)
            {
                return _parkingSlotsParallel[Random.Range(0, _parkingSlotsParallel.Length)];
            }
            return Random.Range(0, 2) == 0
                ? _parkingSlotsPerpendicular[Random.Range(0, _parkingSlotsPerpendicular.Length)]
                : _parkingSlotsParallel[Random.Range(0, _parkingSlotsParallel.Length)];
        }
        
        public void OnCarParkedSuccessfully()
        {
            _parkingsCompleted++;
            CanvasDashboard.Instance.OnCarParkedSuccessfully(_settings.GetParkingsToWin() - _parkingsCompleted);
            if (_parkingsCompleted >= _settings.GetParkingsToWin())
            {
                // Game Won
                gameOverCanvas.gameObject.SetActive(true);
                PlayerCar.gameObject.GetComponent<CarController>().SetIsGamePause(true);
                
                gameOverCanvas.transform.position = gameOverCanvasPosition.transform.position;
                gameOverCanvas.transform.rotation = gameOverCanvasPosition.transform.rotation;
                gameStatus.SetText("You Win! Good Job!");
                Debug.Log("Game Over - You Win!");
            }
            else
            {
                SetRandomParkingSlotAsTarget(_settings.GetParkingType());
            }
        }
        
        public void OnCarHitOtherCar()
        {
            // Game Over
            gameOverCanvas.gameObject.SetActive(true);
            PlayerCar.gameObject.GetComponent<CarController>().SetIsGamePause(true);
            
            gameOverCanvas.transform.position = gameOverCanvasPosition.transform.position;
            gameOverCanvas.transform.rotation = gameOverCanvasPosition.transform.rotation;
            gameStatus.SetText("You Lost! You need more practice...");
            Debug.Log("Game Over - You Hit Another Car!");
        }

        public void OnGetBackToMainMenu()
        {
            gameOverCanvas.gameObject.SetActive(false);
            gamePauseCanvas.gameObject.SetActive(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuVR");
        }
    }
}