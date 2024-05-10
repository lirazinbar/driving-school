using Managers;
using Roads;
using UnityEngine;
using UnityEngine.Serialization;

namespace TrafficObjects
{
    public class TrafficLightController : MonoBehaviour
    {
        [Header("Lights")]
        [SerializeField] private Light redLight;
        [SerializeField] private Light yellowLight;
        [SerializeField] private Light greenLight;
    
        [SerializeField] private TrafficLightSurfaceDetector trafficLightSurfaceDetector;
        private JunctionTrafficLightsManager _junctionTrafficLightsManager;
        public enum LightState { Red, Yellow, Green, RedAndYellow};
        private LightState _currentLightState;

        private const float YellowLightDuration = 2f;
        private const float GreenLightDuration = 5f;

        private float _timer;
        private bool isEmpty = true;

        void Awake()
        {
            _junctionTrafficLightsManager = GetComponentInParent<JunctionTrafficLightsManager>();
            // Start with red light
            redLight.enabled = true;
            yellowLight.enabled = false;
            greenLight.enabled = false;
            _currentLightState = LightState.Red;
        }

        void Update()
        {
            // Update the timer
            _timer += Time.deltaTime;

            // Check if it's time to switch lights
            switch (_currentLightState)
            {
                case LightState.Red:
                    break;
                case LightState.Yellow:
                    if (_timer >= YellowLightDuration)
                    {
                        SetLightState(LightState.Red);
                    }
                    break;
                case LightState.Green:
                    if (_timer >= GreenLightDuration)
                    {
                        SetLightState(LightState.Yellow);
                    }
                    break;
                case LightState.RedAndYellow:
                    if (_timer >= YellowLightDuration)
                    {
                        SetLightState(LightState.Green);
                    }
                    break;
            }
        }

        private void SetLightState(LightState newState)
        {
            // Reset the timer
            _timer = 0f;

            // Disable all lights
            redLight.enabled = false;
            yellowLight.enabled = false;
            greenLight.enabled = false;

            // Enable the appropriate light based on the state
            switch (newState)
            {
                case LightState.Red:
                    redLight.enabled = true;
                    YieldTurn();
                    break;
                case LightState.Yellow:
                    yellowLight.enabled = true;
                    trafficLightSurfaceDetector.OnLightChanged(LightState.Yellow);
                    break;
                case LightState.Green:
                    greenLight.enabled = true;
                    trafficLightSurfaceDetector.OnLightChanged(LightState.Green);
                    break;
                case LightState.RedAndYellow:
                    redLight.enabled = true;
                    yellowLight.enabled = true;
                    break;
            }

            // Update the current light state
            _currentLightState = newState;
        }
    
        public LightState GetCurrentLightState()
        {
            return _currentLightState;
        }
        
        public void OnCarPassedStopLine(int carId)
        {
            if (_currentLightState == LightState.Red)
            {
                EventsManager.Instance.TriggerCarPassedRedLightEvent(carId);
            }
        }
        
        public void StartSequence()
        {
            SetLightState(LightState.RedAndYellow);
        }
        
        private void YieldTurn()
        {
            _junctionTrafficLightsManager.OnTrafficLightChangedToRed();
        }

        public bool IsEmpty()
        { 
            return isEmpty;
        }
        
        public void SetIsEmpty(bool value)
        {
            isEmpty = value;
        }
    }
}