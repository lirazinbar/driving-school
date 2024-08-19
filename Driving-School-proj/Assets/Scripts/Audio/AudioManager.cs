using UnityEngine;

namespace Audio
{
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        private Sound[] _sounds;
        
        private const string AudioPath = "Audio";
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            LoadSounds();
        }

        private void LoadSounds()
        {
            AudioClip[] audioClips = Resources.LoadAll<AudioClip>(AudioPath);
            _sounds = new Sound[audioClips.Length];
            for (int i = 0; i < audioClips.Length; i++)
            {
                _sounds[i] = new Sound
                {
                    name = audioClips[i].name,
                    clip = audioClips[i],
                    source = gameObject.AddComponent<AudioSource>()
                };
                _sounds[i].source.clip = audioClips[i];
            }
        }
        
        public void Play(string name)
        {
            Sound s = System.Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Play();
        }
        
        public void Stop(string name)
        {
            Sound s = System.Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Stop();
        }
        
        public bool IsPlaying(string name)
        {
            Sound s = System.Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return false;
            }
            return s.source.isPlaying;
        }
        
        public float GetAudioClipLength(string name)
        {
            Sound s = System.Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return 0;
            }
            return s.clip.length;
        }
        
        public AudioSource GetAudioSource(string name)
        {
            Sound s = System.Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return null;
            }
            return s.source;
        }
        
        public void SetVolume(string name, float volume)
        {
            Sound s = System.Array.Find(_sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.volume = volume;
        }
    }
}