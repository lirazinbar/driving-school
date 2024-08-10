using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenuManager : MonoBehaviour
{
    [SerializeField] private Canvas editorCanvas;     
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Canvas chooseRouteCanvas;
    [SerializeField] private GameObject routeComponentPrefab;
    [SerializeField] private GameObject gridContainerGameObject;   
    [SerializeField] private GameObject MapName; // of the editor screen
    [SerializeField] private GameObject Grid; // of the editor screen
    [SerializeField] private GameObject Item; // of the editor screen
    private int chosenRouteIndex;

    public void OnCreateNewRoute()
    {
        menuCanvas.gameObject.SetActive(false);
        PlayerPrefs.SetString("isNewRoute", "true");
        editorCanvas.gameObject.SetActive(true);
    }
    

    public void LoadExistingRoutes()
    {
        menuCanvas.gameObject.SetActive(false);
        
         //List<MapMatrixObject> routeList = XMLManager.Instance.LoadRoutes();
        XMLManager.Instance.LoadRoutes(OnRoutesFetched1);
        
        /*
        for (int i = gridContainerGameObject.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gridContainerGameObject.transform.GetChild(i).gameObject);
        }
        
        for (int index = 0; index < routeList.Count; index++)
        {
            MapMatrixObject route = routeList[index];
            GameObject newComponent = Instantiate(routeComponentPrefab, gridContainerGameObject.transform);
            newComponent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = route.name;
            Debug.Log(route.name);
            newComponent.name = "Route" + (index+1);
            
            Button buttonComponent = newComponent.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnChooseRoute(newComponent.name));
        }

        
        chooseRouteCanvas.gameObject.SetActive(true);
        */
    }

    private void OnRoutesFetched1(List<MapMatrixObject> routeList)
    {
        for (int i = gridContainerGameObject.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gridContainerGameObject.transform.GetChild(i).gameObject);
        }
        
        for (int index = 0; index < routeList.Count; index++)
        {
            MapMatrixObject route = routeList[index];
            GameObject newComponent = Instantiate(routeComponentPrefab, gridContainerGameObject.transform);
            newComponent.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = route.name;
            Debug.Log(route.name);
            newComponent.name = "Route" + (index+1);
            
            Button buttonComponent = newComponent.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnChooseRoute(newComponent.name));
        }

        
        chooseRouteCanvas.gameObject.SetActive(true);
    }
    
    public void OnChooseRoute(string name)
    {
        int routeIndex = int.Parse(name.Substring(5));

        if (routeIndex != 0)
        {
            chooseRouteCanvas.gameObject.SetActive(false);
            PlayerPrefs.SetString("isNewRoute", "false-"+ (routeIndex - 1));
            LoadChosenRouteIntoMatrix(routeIndex - 1);
            editorCanvas.gameObject.SetActive(true);
        }
    }
    
    public void LoadChosenRouteIntoMatrix(int index)
    {
        chosenRouteIndex = index;
        //List<MapMatrixObject> routeList = XMLManager.Instance.LoadRoutes();
        XMLManager.Instance.LoadRoutes(OnRoutesFetched2);
        /* MapMatrixObject chosenRoute = routeList[index];
        MapName.GetComponent<TMP_InputField>().text = chosenRoute.name;
        
        foreach (MapCellObject mapCellObject in chosenRoute.mapCellObjectsList)
        {
            int componentNumber = mapCellObject.componentObject.componentNumber;
            int rotationZ = mapCellObject.componentObject.rotation;
            
            int rowIndex = mapCellObject.row;
            int colIndex = mapCellObject.col;

            GameObject slot = Grid.transform.GetChild(rowIndex * 8 + colIndex).gameObject;

            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
            
            if (componentNumber != 0)
            {
                Transform newItem = Instantiate(Item.transform, slot.transform);
                newItem.name = "Item";
                Image itemImage = newItem.transform.GetComponent<Image>();
                Sprite sprite = Resources.Load<Sprite>("Photos/RoutesItems/" + "component"+componentNumber);
               
                itemImage.sprite = sprite;
                
                newItem.GetComponent<DraggableItem>().image.raycastTarget = true;
                newItem.transform.Rotate(new Vector3(0, 0, rotationZ));
            }
        }
        */
    }

    private void OnRoutesFetched2(List<MapMatrixObject> routeList)
    {
        MapMatrixObject chosenRoute = routeList[chosenRouteIndex];
        MapName.GetComponent<TMP_InputField>().text = chosenRoute.name;
        
        foreach (MapCellObject mapCellObject in chosenRoute.mapCellObjectsList)
        {
            int componentNumber = mapCellObject.componentObject.componentNumber;
            int rotationZ = mapCellObject.componentObject.rotation;
            
            int rowIndex = mapCellObject.row;
            int colIndex = mapCellObject.col;

            GameObject slot = Grid.transform.GetChild(rowIndex * 8 + colIndex).gameObject;

            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
            
            if (componentNumber != 0)
            {
                Transform newItem = Instantiate(Item.transform, slot.transform);
                newItem.name = "Item";
                Image itemImage = newItem.transform.GetComponent<Image>();
                Sprite sprite = Resources.Load<Sprite>("Photos/RoutesItems/" + "component"+componentNumber);
               
                itemImage.sprite = sprite;
                
                newItem.GetComponent<DraggableItem>().image.raycastTarget = true;
                newItem.transform.Rotate(new Vector3(0, 0, rotationZ));
            }
        }
    }
    public void onGetBackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    public void onGetBackToEditorMenu()
    {
        chooseRouteCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
    }
}
