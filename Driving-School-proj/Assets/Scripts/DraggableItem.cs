using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector] public Transform parentAfterDrag;
    public Transform originalParent;
    public Transform position;

    void Start()
    {
        originalParent = transform.parent;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        if (originalParent.CompareTag("SrcSlot"))
        {
            Transform newItem = Instantiate(transform, position);
            newItem.SetParent(originalParent);
            newItem.name = "Item";
            originalParent = parentAfterDrag;
            // newItem.GetComponent<DraggableItem>().originalParent = parentAfterDrag;
        }
    }
}
