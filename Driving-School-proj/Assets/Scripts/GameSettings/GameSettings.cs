using Enums;

namespace GameSettings
{
    public class GameSettings
    {
        private PedestrianDifficulty _pedestrianDifficulty;
        private CarsDifficulty _carsDifficulty;
        private bool _dayNightMode;
        private int _numberOfTurnsToWin;
        private int _mistakePoints;

        public GameSettings(
            PedestrianDifficulty pedestrianDifficulty, 
            CarsDifficulty carsDifficulty, 
            bool dayNightMode, 
            int numberOfTurnsToWin, 
            int mistakePoints)
        {
            _pedestrianDifficulty = pedestrianDifficulty;
            _carsDifficulty = carsDifficulty;
            _dayNightMode = dayNightMode;
            _numberOfTurnsToWin = numberOfTurnsToWin;
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
        
        public bool GetDayNightMode()
        {
            return _dayNightMode;
        }
        
        public int GetNumberOfTurnsToWin()
        {
            return _numberOfTurnsToWin;
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
        
        public void SetDayNightMode(bool dayNightMode)
        {
            _dayNightMode = dayNightMode;
        }
        
        public void SetNumberOfTurnsToWin(int numberOfTurnsToWin)
        {
            _numberOfTurnsToWin = numberOfTurnsToWin;
        }
        
        public void SetMistakePoints(int mistakePoints)
        {
            _mistakePoints = mistakePoints;
        }
    }
}