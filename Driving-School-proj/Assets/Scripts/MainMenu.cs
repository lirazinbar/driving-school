using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadDefaultEnvironment()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameEnv");
    }
    
    public void LoadEnvironmentEditor()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RoutesEditor");
    }
}
