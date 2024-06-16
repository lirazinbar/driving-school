using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas editorCanvas;     
    [SerializeField] private Canvas menuCanvas;

    public void OnCreateNewRoute()
    {
        menuCanvas.gameObject.SetActive(false);
        editorCanvas.gameObject.SetActive(true);
    }

    public void onGetBackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
