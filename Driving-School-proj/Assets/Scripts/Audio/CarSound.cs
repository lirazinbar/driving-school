using UnityEngine;

namespace Audio
{
    public class CarSound : MonoBehaviour
    {   
        private float minSpeed = 0f;
        private float maxSpeed = 30f;
        private float _currentSpeed;

        private Rigidbody _rb;
        private AudioSource _carAudio;

        private float minPitch = 0.2f;
        private float maxPitch = 2.0f;
        
        private AudioManager audioManager;
        private bool playEngineSound;

        private void Start()
        {
            _carAudio = GetComponent<AudioSource>();
            _rb = GetComponent<Rigidbody>();
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.Play("CarIgnition");
        }

        private void Update()
        {
            if (!playEngineSound)
            {
                if (!audioManager.IsPlaying("CarIgnition"))
                {
                    playEngineSound = true;
                    _carAudio.Play();
                }
            }
            else
            {
                Debug.Log("Engine sound is playing");
                EngineSound();
            }
        }

        private void EngineSound()
        {
            float carSpeed = GetSpeed();
            Debug.Log("Car speed: " + carSpeed);
            
            if (carSpeed < maxSpeed)
            {
                _carAudio.pitch = Mathf.Lerp(minPitch, maxPitch, (carSpeed - minSpeed) / (maxSpeed - minSpeed));
            }
            else
            {
                _carAudio.pitch = maxPitch;
            }
        }
        
        private float GetSpeed() {
            return _rb.velocity.magnitude * 3.6f;
        }
    }
}