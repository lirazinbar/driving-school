using Enums;

namespace GameSettings
{
    public class GameSettings
    {
        private PedestrianDifficulty _pedestrianDifficulty;
        private CarsDifficulty _carsDifficulty;
        private bool _nightMode;
        private int _turnsToWin;
        private int _mistakePoints;

        public GameSettings(
            PedestrianDifficulty pedestrianDifficulty, 
            CarsDifficulty carsDifficulty, 
            bool nightMode, 
            int turnsToWin, 
            int mistakePoints)
        {
            _pedestrianDifficulty = pedestrianDifficulty;
            _carsDifficulty = carsDifficulty;
            _nightMode = nightMode;
            _turnsToWin = turnsToWin;
            _mistakePoints = mistakePoints;
        }
        
        public PedestrianDifficulty GetPedestrianDifficulty()
        {
            return _pedestrianDifficulty;
        }
        
        public CarsDifficulty GetCarsDifficulty()
        {
            return _carsDifficulty;
        }
        
        public bool GetNightMode()
        {
            return _nightMode;
        }
        
        public int GetTurnsToWin()
        {
            return _turnsToWin;
        }
        
        public int GetMistakePoints()
        {
            return _mistakePoints;
        }
        
        public void SetPedestrianDifficulty(PedestrianDifficulty pedestrianDifficulty)
        {
            _pedestrianDifficulty = pedestrianDifficulty;
        }
        
        public void SetCarsDifficulty(CarsDifficulty carsDifficulty)
        {
            _carsDifficulty = carsDifficulty;
        }
        
        public void SetNightMode(bool dayNightMode)
        {
            _nightMode = dayNightMode;
        }
        
        public void SetTurnsToWin(int turnsToWin)
        {
            _turnsToWin = turnsToWin;
        }
        
        public void SetMistakePoints(int mistakePoints)
        {
            _mistakePoints = mistakePoints;
        }
    }
}