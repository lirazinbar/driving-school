/*using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PreventClickDrag : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{

    public ScrollRect EdgesScroll;

    public void OnBeginDrag(PointerEventData data)
    {
        EdgesScroll.StopMovement();
        EdgesScroll.enabled = false;
    }

    public void OnEndDrag(PointerEventData data)
    {
        EdgesScroll.enabled = true;
    }
}*/