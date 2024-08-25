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
        
        private void LoadGameSettings()
        {
            // settings = GetSettingsFromPlayerPrefs();
            _settings = new ParkingTestSettings(1, ParkingType.Perpendicular);
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
            if (_parkingsCompleted >= _settings.GetParkingsToWin())
            {
                // Game Won
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
            Debug.Log("Game Over - You Hit Another Car!");
        }
    }
}