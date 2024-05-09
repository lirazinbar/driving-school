using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRouteMap : MonoBehaviour
{
    [SerializeField] private List<GameObject> componentsPrefabs;
    private void Start()
    {
        EventsManager.Instance.routeMapSaved.AddListener(OnRouteMapSave);
    }

    private void OnDestroy()
    {
        EventsManager.Instance.routeMapSaved.RemoveListener(OnRouteMapSave);
    }
    
    private void OnRouteMapSave(ComponentObject[,] routeMap)
    {
        Debug.Log("route map saved event event triggered");
        
        int rows = routeMap.GetLength(0);
        int cols = routeMap.GetLength(1);

        // Iterate over the routeMap matrix
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Get the SlotObject at the current cell
                ComponentObject componentObject = routeMap[i, j];

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
                        Vector3 prefabPosition =new Vector3( transform.position.x + (j+1) * 100, 0, transform.position.z - (i) * 100);
                        
                        GameObject newComponent = Instantiate(prefab, prefabPosition, Quaternion.Euler(0, 0, 0));
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
}
