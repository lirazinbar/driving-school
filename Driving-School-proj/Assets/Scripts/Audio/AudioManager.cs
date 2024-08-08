using UnityEngine;

namespace Audio
{
    public class AudioManager: MonoBehaviour
    {
        public static AudioManager instance;
        [SerializeField] private Sound[] sounds;
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
        
        public void Play(string name)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Play();
        }
        
        public void Stop(string name)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == name);
            s.source.Stop();
        }
        
        public bool IsPlaying(string name)
        {
            Sound s = System.Array.Find(sounds, sound => sound.name == name);
            return s.source.isPlaying;
        }
    }
}