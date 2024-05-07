using System.Collections.Generic;
using TrafficObjects;
using UnityEngine;

namespace Roads
{
    public class JunctionTrafficLightsManager : MonoBehaviour
    {
        private TrafficLightController[] _trafficLights;
        private int _currentTrafficLightIndex;

        private void Start()
        {
            _trafficLights = GetComponentsInChildren<TrafficLightController>();
            _trafficLights[_currentTrafficLightIndex].StartSequence();
        }

        private void SwitchTrafficLight()
        {
            _currentTrafficLightIndex = (_currentTrafficLightIndex + 1) % _trafficLights.Length;
            _trafficLights[_currentTrafficLightIndex].StartSequence();
        }
        
        public void OnTrafficLightChangedToRed()
        {
            // Sleep for one second
            StartCoroutine(SwitchTrafficLightAfterDelay());
            
            SwitchTrafficLight();
        }
        
        private IEnumerator<WaitForSeconds> SwitchTrafficLightAfterDelay()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
