using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeMenu : MonoBehaviour
{
    [SerializeField] private Canvas playModeMenuCanvas;     
    [SerializeField] private Canvas chooseRouteMenuCanvas;     
    [SerializeField] private Canvas mainMenuCanvas;
    
    public void onGetBackToMainMenu()
    {
        playModeMenuCanvas.gameObject.SetActive(false);
        chooseRouteMenuCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
    }
}
