using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas playModeMenuCanvas;     
    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private GameObject routeComponentPrefab;
    [SerializeField] private GameObject gridContainerGameObject;

    public void LoadDefaultEnvironment()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        
        List<MapMatrixObject> routeList = XMLManager.Instance.Load();
        
        
        for (int index = gridContainerGameObject.transform.childCount - 1; index < routeList.Count; index++)
        {
            MapMatrixObject route = routeList[index];
            GameObject newComponent = Instantiate(routeComponentPrefab, gridContainerGameObject.transform);
            newComponent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = route.name;
            Debug.Log(route.name);
            
            newComponent.name = "Route" + (index+1);
        }

        
        playModeMenuCanvas.gameObject.SetActive(true);
        // UnityEngine.SceneManagement.SceneManager.LoadScene("GameEnv");
    }
    
    public void LoadEnvironmentEditor()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RoutesEditor");
    }
}
