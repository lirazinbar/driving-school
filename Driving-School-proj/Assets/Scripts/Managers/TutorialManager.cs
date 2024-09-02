using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Light rightLight;
    [SerializeField] private Light leftLight;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Animator _animator;
    private bool helloWasSet = false;

    private float startTime;

    private string description2 =
        "This is Morgan Freeman, your teacher who is going to give you instructions during the class you will be taking." +
        "\n\nAnd this is the car you're going to drive on.";

    private string description3 = "To hold the steering wheel, press the grip buttons on the controls." +
                                  "\n\nTo press the gas you have to press the selection button on the right remote, and to press the brakes you have to press the selection button on the left remote." +
                                  "\n\nYou will see a gear stick on your right,";

    private string description4 =
        "In order to change gear, you must move the gear stick forward to the Drive position or back to the Reverse position. In order to park, move the gear stick to the middle." +
        "\n\nIn addition, you can enjoy quality background songs by clicking the radio button on your right.\n\nWe wish you an enriching, fun and educational driving experience, good luck!";
                                  

    private void Start()
    {
        AudioManager.Instance.Play("Tutorial");
        startTime = Time.time;
    }

    void Update()
    {
        float audioTime = Time.time - startTime;
        if (AudioManager.Instance.IsPlaying("Tutorial") && audioTime > 12 && audioTime < 22)
        {
            descriptionText.SetText(description2);
            if (!helloWasSet)
            {
                _animator.SetTrigger("Hello");
                helloWasSet = true;
            }
        }
        if (AudioManager.Instance.IsPlaying("Tutorial") && audioTime > 22 && audioTime < 37)
        {
            descriptionText.SetText(description3);
        }
        if (AudioManager.Instance.IsPlaying("Tutorial") && audioTime > 37)
        {
            descriptionText.SetText(description4);
        }
        
        if (AudioManager.Instance.IsPlaying("Tutorial") && audioTime > 18 && audioTime < 20)
        {
            rightLight.enabled = true;
        }
        
        if (
            (AudioManager.Instance.IsPlaying("Tutorial") && 
             (audioTime > 20 || audioTime < 18)) || 
            !AudioManager.Instance.IsPlaying("Tutorial"))
        {
            rightLight.enabled = false;
        }
        
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            if (AudioManager.Instance.IsPlaying("Tutorial"))
            {
                AudioManager.Instance.Stop("Tutorial");
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
        }

        if (OVRInput.GetUp(OVRInput.RawButton.B))
        {
            if (AudioManager.Instance.IsPlaying("Tutorial"))
            {
                AudioManager.Instance.Stop("Tutorial");
            }
            string nextScene = PlayerPrefs.GetString("NextScene");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
    }
}
