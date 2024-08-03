using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class CreateRouteMap : MonoBehaviour
{
    [SerializeField] private List<GameObject> componentsPrefabs;
    private void Start()
    {
        List<MapMatrixObject> routeList = XMLManager.Instance.Load();
        int routeNumber = int.Parse(PlayerPrefs.GetString("RouteNumber"));

        OnRouteMapSave(routeList[routeNumber]);
    }
    
    private void OnRouteMapSave(MapMatrixObject mapMatrixObject)
    {
        Debug.Log("route map saved event event triggered");
        
        // Iterate over the routeMap matrix
        foreach (MapCellObject cell in mapMatrixObject.mapCellObjectsArray)
        {
                // Get the SlotObject at the current cell
                ComponentObject componentObject = cell.componentObject;

                // If the slot is not empty (SlotObject is not null)
                if (componentObject.componentNumber != 0)
                {
                    // Get the component number from the SlotObject
                    int componentNumber = componentObject.componentNumber;

                    // If the component number is within the bounds of the prefabs array
                    if (componentNumber >= 0 && componentNumber <= componentsPrefabs.Count)
                    {
                        // Instantiate the prefab corresponding to the component number
                        GameObject prefab = componentsPrefabs[componentNumber - 1];
                        Vector3 prefabPosition =new Vector3( transform.position.x + (cell.col+1) * 100, 0, transform.position.z - (cell.row) * 100);
                        
                        GameObject newComponent = Instantiate(prefab, prefabPosition, Quaternion.Euler(0, 0, 0), transform);
                        newComponent.transform.GetChild(0).Rotate(new Vector3(0, componentObject.rotation * -1, 0));
                    }
                    else
                    {
                        Debug.LogWarning("Component number out of bounds: " + componentNumber);
                    }
                }
            
        }
    }
}
