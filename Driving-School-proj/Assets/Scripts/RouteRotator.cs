using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RouteRotator : MonoBehaviour, IPointerClickHandler
{
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject slot = transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject;
        if (slot.transform.childCount > 0)
        {
            slot.transform.GetChild(0).transform.Rotate(new Vector3(0, 0, 90));
            
        }
    }
}
