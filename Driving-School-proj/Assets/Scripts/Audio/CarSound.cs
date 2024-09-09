using System.Collections;
using UnityEngine;

namespace Audio
{
    public class CarSound : MonoBehaviour
    {   
        AudioSource _carEngineAudio;
        
        private float minSpeed = 0f;
        private float maxSpeed = 30f;
        private float _currentSpeed;

        private Rigidbody _rb;

        private float minPitch = 0.2f;
        private float maxPitch = 2.0f;
        
        private bool playEngineSound;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _carEngineAudio = AudioManager.Instance.GetAudioSource("CarEngine");
            _carEngineAudio.loop = true;
            
            AudioManager.Instance.Play("CarIgnition");
            
            float carIgnitionAudioClipLength = AudioManager.Instance.GetAudioClipLength("CarIgnition");
            StartCoroutine(PlaySoundAfterDelay(carIgnitionAudioClipLength, "CarEngine"));
        }

        private void Update()
        {
            EngineSound();
        }

        private void EngineSound()
        {
            float carSpeed = GetSpeed();
            
            if (carSpeed < maxSpeed)
            {
                _carEngineAudio.pitch = Mathf.Lerp(minPitch, maxPitch, (carSpeed - minSpeed) / (maxSpeed - minSpeed));
            }
            else
            {
                _carEngineAudio.pitch = maxPitch;
            }
        }
        
        private float GetSpeed() {
            return _rb.velocity.magnitude * 3.6f;
        }
        
        IEnumerator PlaySoundAfterDelay(float delay, string audioClipName)
        {
            yield return new WaitForSeconds(delay);
            AudioManager.Instance.Play(audioClipName);
        }
    }
}