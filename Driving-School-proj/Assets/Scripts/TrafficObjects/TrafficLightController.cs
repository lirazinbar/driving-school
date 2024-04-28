using Managers;
using UnityEngine;

namespace TrafficObjects
{
    public class TrafficLightController : MonoBehaviour
    {
        [SerializeField] private Light redLight;
        [SerializeField] private Light yellowLight;
        [SerializeField] private Light greenLight;
    
        [SerializeField] private TrafficLightSurfaceDetector _trafficLightSurfaceDetector;

        public enum LightState { Red, Yellow, Green, RedAndYellow};
        private LightState currentLightState;

        private float redLightDuration = 5f;
        private float yellowLightDuration = 2f;
        private float greenLightDuration = 5f;

        private float timer;

        void Start()
        {
            // Start with the red light
            SetLightState(LightState.Red);
        }

        void Update()
        {
            // Update the timer
            timer += Time.deltaTime;

            // Check if it's time to switch lights
            switch (currentLightState)
            {
                case LightState.Red:
                    if (timer >= redLightDuration)
                    {
                        SetLightState(LightState.RedAndYellow);
                    }
                    break;
                case LightState.Yellow:
                    if (timer >= yellowLightDuration)
                    {
                        SetLightState(LightState.Red);
                    }
                    break;

                case LightState.Green:
                    if (timer >= greenLightDuration)
                    {
                        SetLightState(LightState.Yellow);
                    }
                    break;
                case LightState.RedAndYellow:
                    if (timer >= yellowLightDuration)
                    {
                        SetLightState(LightState.Green);
                    }
                    break;
            }
        }

        private void SetLightState(LightState newState)
        {
            // Reset the timer
            timer = 0f;

            // Disable all lights
            redLight.enabled = false;
            yellowLight.enabled = false;
            greenLight.enabled = false;

            // Enable the appropriate light based on the state
            switch (newState)
            {
                case LightState.Red:
                    redLight.enabled = true;
                    _trafficLightSurfaceDetector.OnLightChanged(LightState.Red);
                    break;
                case LightState.Yellow:
                    yellowLight.enabled = true;
                    break;
                case LightState.Green:
                    greenLight.enabled = true;
                    _trafficLightSurfaceDetector.OnLightChanged(LightState.Green);
                    break;
                case LightState.RedAndYellow:
                    redLight.enabled = true;
                    yellowLight.enabled = true;
                    break;
            }

            // Update the current light state
            currentLightState = newState;
        }
    
        public LightState GetCurrentLightState()
        {
            return currentLightState;
        }
        
        public void OnCarPassedStopLine(int carId)
        {
            if (currentLightState == LightState.Red)
            {
                EventsManager.Instance.TriggerCarPassedRedLightEvent(carId);
            }
        }
    }
}