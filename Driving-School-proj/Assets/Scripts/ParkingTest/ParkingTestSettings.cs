namespace ParkingTest
{
    public class ParkingTestSettings
    {
        private int _parkingsToWin;
        private ParkingType _parkingType;

        public ParkingTestSettings(int parkingsToWin, ParkingType parkingType)
        {
            _parkingsToWin = parkingsToWin;
            _parkingType = parkingType;
        }
            
        
        public int GetParkingsToWin()
        {
            return _parkingsToWin;
        }
        
        public ParkingType GetParkingType()
        {
            return _parkingType;
        }
    }

}