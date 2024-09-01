using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.Play("Tutorial");

    }

    void Update()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
        }

        if (OVRInput.GetUp(OVRInput.RawButton.B))
        {
            string nextScene = PlayerPrefs.GetString("NextScene");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
    }
}
