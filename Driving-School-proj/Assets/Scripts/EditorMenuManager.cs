using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using TMPro;
using UnityEngine.UI;

public class EditorMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas editorCanvas;     
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas editMenuCanvas;
    [SerializeField] private GameObject routeComponentPrefab;
    [SerializeField] private GameObject gridContainerGameObject;

    public void OnCreateNewRoute()
    {
        menuCanvas.gameObject.SetActive(false);
        editorCanvas.gameObject.SetActive(true);
    }

    public void onGetBackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    public void OnEditRouteMenu()
    {
        menuCanvas.gameObject.SetActive(false);
        
        List<MapMatrixObject> routeList = XMLManager.Instance.Load();
        
        
        for (int index = gridContainerGameObject.transform.childCount; index < routeList.Count; index++)
        {
            MapMatrixObject route = routeList[index];
            GameObject newComponent = Instantiate(routeComponentPrefab, gridContainerGameObject.transform);
            newComponent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = route.name;
            Debug.Log(route.name);
            newComponent.name = "Route" + (index+1);
            
            Button buttonComponent = newComponent.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnChooseRouteToEdit(newComponent.name));
        }
        
        editMenuCanvas.gameObject.SetActive(true);
    }

    public void OnChooseRouteToEdit(string name)
    {
        // TODO - hereee
    }
}
