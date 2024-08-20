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
        loadEmptyBoard();
        editorCanvas.gameObject.SetActive(true);
    }

    private void loadEmptyBoard()
    {
        MapName.GetComponent<TMP_InputField>().text = "";

        for (int rowIndex = 0; rowIndex < 3; rowIndex++)
        {
            for (int colIndex = 0; colIndex < 8; colIndex++)
            {
                GameObject slot = Grid.transform.GetChild(rowIndex * 8 + colIndex).gameObject;

                if ((rowIndex == 0 && colIndex == 7) || (rowIndex == 2 && colIndex == 0) ||
                    (rowIndex == 2 && colIndex == 7))
                {
                    DraggableItem draggableItem = slot.transform.GetChild(0).gameObject.GetComponent<DraggableItem>();

                    if (draggableItem != null)
                    {
                        // Disable the script
                        draggableItem.enabled = false;
                    }
                }
                else if ((slot.transform.childCount > 0) && (rowIndex != 0 || colIndex != 0))
                {
                    Destroy(slot.transform.GetChild(0).gameObject);
                }
            }
        }
    }
    

    public void LoadExistingRoutes()
    {
        menuCanvas.gameObject.SetActive(false);
        
        // TODO
        // StartCoroutine(DatabaseManager.Instance.GetRoutes(OnRoutesFetched1));
        DatabaseManager.Instance.GetData(OnRoutesFetched1);
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
        
        // TODO
        //StartCoroutine(DatabaseManager.Instance.GetRoutes(OnRoutesFetched2));
        DatabaseManager.Instance.GetData(OnRoutesFetched2);
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

                if ((rowIndex == 0 && colIndex == 7) || (rowIndex == 2 && colIndex == 0) ||
                    (rowIndex == 2 && colIndex == 7))
                {
                    DraggableItem draggableItem = newItem.GetComponent<DraggableItem>();
            
                    if (draggableItem != null)
                    {
                        // Disable the script
                        draggableItem.enabled = false;
                    }
                }
            }
        }
    }
    /*public void onGetBackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }*/
    
    public void onGetBackToEditorMenu()
    {
        chooseRouteCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
    }
}
