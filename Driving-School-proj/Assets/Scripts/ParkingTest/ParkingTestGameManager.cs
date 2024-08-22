using UnityEngine;

namespace ParkingTest
{
    public class ParkingTestGameManager : MonoBehaviour
    {
        public static ParkingTestGameManager Instance { get; private set; }
        
        private ParkingSlotPerpendicular[] _parkingSlotsPerpendicular;
        private ParkingSlotParallel[] _parkingSlotsParallel;
        
        private ParkingTestSettings settings;
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
            _parkingSlotsPerpendicular = FindObjectsOfType<ParkingSlotPerpendicular>();
            _parkingSlotsParallel = FindObjectsOfType<ParkingSlotParallel>();
            
            // Choose a random parking slot
            int randomIndex = Random.Range(0, _parkingSlotsPerpendicular.Length);
        }
        
        private void LoadGameSettings()
        {
            // settings = GetSettingsFromPlayerPrefs();
            settings = new ParkingTestSettings(3, ParkingType.PerpendicularAndParallel);
        }

        private ParkingTestSettings GetSettingsFromPlayerPrefs()
        {
            int parkingsToWin = PlayerPrefs.GetInt("ParkingsToWin");
            ParkingType parkingType = (ParkingType) PlayerPrefs.GetInt("ParkingType");
            
            return new ParkingTestSettings(parkingsToWin, parkingType);
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
    }
}