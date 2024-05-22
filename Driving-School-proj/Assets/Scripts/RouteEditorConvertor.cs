using System.Collections;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RouteEditorConvertor : MonoBehaviour
{
    [SerializeField] private GameObject Grid; 
    ComponentObject[,] RouteMap = new ComponentObject[3, 8];
    
    public void OnSaveRoute()
    {
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

            RouteMap[rowIndex, colIndex] = componentObject;
        }

        PrintMatrix();
        EventsManager.Instance.TriggerRouteMapSaved(RouteMap);
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

public class ComponentObject
{
    public int componentNumber;
    public int rotation;

    public ComponentObject(int componentNumber, int rotation)
    {
        this.componentNumber = componentNumber;
        this.rotation = rotation;
    }
}