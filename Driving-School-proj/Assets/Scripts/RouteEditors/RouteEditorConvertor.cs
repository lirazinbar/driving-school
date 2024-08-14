using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using JetBrains.Annotations;
using TMPro;


public class RouteEditorConvertor : MonoBehaviour
{
    [SerializeField] private GameObject Grid; 
    [SerializeField] private Canvas editorCanvas;     
    [SerializeField] private Canvas menuCanvas;     
    [SerializeField] private GameObject MapName; 

    ComponentObject[,] RouteMap = new ComponentObject[3, 8];
    
    public void OnSaveRoute()
    {
        /*
        int rows = 3;
        int cols = 8;
        List<MapCellObject> cellsList = new List<MapCellObject>(rows * cols);

        // Initialize the list with default MapCellObject instances
        for (int i = 0; i < rows * cols; i++)
        {
            cellsList.Add(null);
        }

        foreach (Transform slot in Grid.transform)
        {
            int rowIndex = int.Parse(slot.name.Substring(4, 1));
            int colIndex = int.Parse(slot.name.Substring(5, 1));

            int componentNumber = 0;
            int rotationZ = 0;
            
            if (slot.childCount > 0)
            {
                string componentName = slot.GetChild(0).GetComponent<Image>().sprite.name;
                componentNumber = int.Parse(componentName.Substring("component".Length));
                rotationZ = Mathf.RoundToInt(slot.GetChild(0).GetComponent<RectTransform>().localRotation.eulerAngles.z);
            }
            
            ComponentObject componentObject = new ComponentObject(componentNumber, rotationZ);
            MapCellObject mapCellObject = new MapCellObject(rowIndex, colIndex, componentObject);
            Debug.Log("row "+ rowIndex);
            Debug.Log("col "+ colIndex);
            cellsList[rowIndex*8 + colIndex] = mapCellObject;

            RouteMap[rowIndex, colIndex] = componentObject;
        }

        PrintMatrix();
        // Save the route map
        */
        // List<MapMatrixObject> routeList = XMLManager.Instance.LoadRoutes();
        // XMLManager.Instance.LoadRoutes(OnRoutesFetched);
        StartCoroutine(DatabaseManager.Instance.GetRoutes(OnRoutesFetched));

        /* MapMatrixObject mapMatrixObject = new MapMatrixObject(MapName.GetComponent<TMP_InputField>().text, cellsList);
        
        string isNewRoute = PlayerPrefs.GetString("isNewRoute");
        if (isNewRoute == "true")
        {
            routeList.Add(mapMatrixObject);
        }
        else
        {
            int index = int.Parse(isNewRoute.Substring("false-".Length));
            routeList[index] = mapMatrixObject;
        }
        
        XMLManager.Instance.SaveRoutes(routeList);
        
        editorCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);
        */
    }

    private void OnRoutesFetched(List<MapMatrixObject> routeList)
    {
        int rows = 3;
        int cols = 8;
        List<MapCellObject> cellsList = new List<MapCellObject>(rows * cols);

        // Initialize the list with default MapCellObject instances
        for (int i = 0; i < rows * cols; i++)
        {
            cellsList.Add(null);
        }

        foreach (Transform slot in Grid.transform)
        {
            int rowIndex = int.Parse(slot.name.Substring(4, 1));
            int colIndex = int.Parse(slot.name.Substring(5, 1));

            int componentNumber = 0;
            int rotationZ = 0;
            
            if (slot.childCount > 0)
            {
                string componentName = slot.GetChild(0).GetComponent<Image>().sprite.name;
                componentNumber = int.Parse(componentName.Substring("component".Length));
                rotationZ = Mathf.RoundToInt(slot.GetChild(0).GetComponent<RectTransform>().localRotation.eulerAngles.z);
            }
            
            ComponentObject componentObject = new ComponentObject(componentNumber, rotationZ);
            MapCellObject mapCellObject = new MapCellObject(rowIndex, colIndex, componentObject);
            Debug.Log("row "+ rowIndex);
            Debug.Log("col "+ colIndex);
            cellsList[rowIndex*8 + colIndex] = mapCellObject;

            RouteMap[rowIndex, colIndex] = componentObject;
        }

        PrintMatrix();
        // Save the route map
        MapMatrixObject mapMatrixObject = new MapMatrixObject(MapName.GetComponent<TMP_InputField>().text, cellsList);
        
        string isNewRoute = PlayerPrefs.GetString("isNewRoute");
        if (isNewRoute == "true")
        {
            routeList.Add(mapMatrixObject);
        }
        else
        {
            int index = int.Parse(isNewRoute.Substring("false-".Length));
            routeList[index] = mapMatrixObject;
        }
        
       // XMLManager.Instance.SaveRoutes(routeList);
        DatabaseManager.Instance.CreateRoutes(routeList);

        
        editorCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(true);   
    }
    
    void PrintMatrix()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Debug.Log($"Content at cell [{i},{j}]: {RouteMap[i, j].componentNumber}, {RouteMap[i, j].rotation}  ");
            }
        }
    }
}



[System.Serializable]
public class MapMatrixObject
{
    [XmlElement("Name")]
    public string name;
    
    [XmlArray("Matrix")]
    [XmlArrayItem("Cell")]
    public List<MapCellObject> mapCellObjectsList;

    public MapMatrixObject()
    {
        this.name = "";
    }

    public MapMatrixObject(string name, List<MapCellObject> mapCellObjectsList)
    {
        this.name = name;
        this.mapCellObjectsList = mapCellObjectsList;
    }
}

[System.Serializable]
public class MapCellObject
{
    public int row;
    public int col;
    public ComponentObject componentObject;

    public MapCellObject()
    {
        this.row = 0;
        this.col = 0;
        this.componentObject = new ComponentObject();
    }

    public MapCellObject(int row, int col, ComponentObject componentObject)
    {
        this.row = row;
        this.col = col;
        this.componentObject = componentObject;
    }
}

[System.Serializable]
public class ComponentObject
{
    public int componentNumber;
    public int rotation;

    public ComponentObject()
    {
        this.componentNumber = 0;
        this.rotation = 0;
    }

    public ComponentObject(int componentNumber, int rotation)
    {
        this.componentNumber = componentNumber;
        this.rotation = rotation;
    }
}
