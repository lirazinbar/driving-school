using System.Collections.Generic;
using UnityEngine;

namespace TrafficObjects.TrafficLight
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
            _currentTrafficLightIndex++;
            if (_currentTrafficLightIndex >= _trafficLights.Length)
            {
                _currentTrafficLightIndex = 0;
            }
            
            // bool isAnyTrafficLightNotEmpty = false;
            // if (_currentTrafficLightIndex != 0)
            // {
            //     for (int i = _currentTrafficLightIndex; i < _trafficLights.Length && !isAnyTrafficLightNotEmpty; i++)
            //     {
            //         if (!_trafficLights[i].IsEmpty())
            //         {
            //             _currentTrafficLightIndex = i;
            //             isAnyTrafficLightNotEmpty = true;
            //         }
            //     }
            // }
            //
            // if (!isAnyTrafficLightNotEmpty)
            // {
            //     _currentTrafficLightIndex = 0;
            // }

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
