using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Canvas playModeMenuCanvas;     
    [SerializeField] private TMP_InputField playerNameInput;     
    [SerializeField] private Canvas chooseRouteMenuCanvas;     
    //[SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private GameObject routeComponentPrefab;
    [SerializeField] private GameObject gridContainerGameObject;

    /*public void EnterPlayModeMenu()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        playModeMenuCanvas.gameObject.SetActive(true);
    }
    */
    public void SaveNameAndLoadRoutes()
    {
        PlayerPrefs.SetString("PlayerName", this.playerNameInput.text);
        playModeMenuCanvas.gameObject.SetActive(false);
        
        StartCoroutine(DatabaseManager.Instance.GetRoutes(OnRoutesFetched));
    }

    private void OnRoutesFetched(List<MapMatrixObject> routeList)
    {
        for (int i = gridContainerGameObject.transform.childCount - 1; i > 0; i--)
        {
            Destroy(gridContainerGameObject.transform.GetChild(i).gameObject);
        }
        
        for (int index = 0; index < routeList.Count; index++)
        {
            MapMatrixObject route = routeList[index];
            GameObject newComponent = Instantiate(routeComponentPrefab, gridContainerGameObject.transform);
            newComponent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = route.name;
            newComponent.name = "Route" + (index+1);
            
            Button buttonComponent = newComponent.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnChooseRoute(newComponent.name));
        }

        
        chooseRouteMenuCanvas.gameObject.SetActive(true);
    }
    
    public void LoadEnvironmentEditor()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RoutesEditor");
    }
    
    public void OnChooseRoute(string name)
    {
        int routeIndex = int.Parse(name.Substring(5));

        if (routeIndex == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameEnv");
        }
        else
        {
            PlayerPrefs.SetString("RouteNumber", (routeIndex-1)+"");
            UnityEngine.SceneManagement.SceneManager.LoadScene("RoutesCreatorEnv");
        }
    }
}
