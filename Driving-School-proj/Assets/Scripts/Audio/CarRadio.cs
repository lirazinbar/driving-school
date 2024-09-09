using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class CarRadio : MonoBehaviour
    {
        private int radioSoundIndex = -1;

        private List<string> radioSounds = new List<string> {"GreasedLightning", "ShakeItOff", "happy-mood-ukulele"};
        
        public void OnRadioClicked()
        {
            if (radioSoundIndex != radioSounds.Count && radioSoundIndex != -1)
            {
                AudioManager.Instance.Stop(radioSounds[radioSoundIndex]);
            }
            radioSoundIndex = (radioSoundIndex+1)%(radioSounds.Count+1);
            if (radioSoundIndex != radioSounds.Count)
            {
                AudioManager.Instance.Play(radioSounds[radioSoundIndex]);
            }
        }
    }
}