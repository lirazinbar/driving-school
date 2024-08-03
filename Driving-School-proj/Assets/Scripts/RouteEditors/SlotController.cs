using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        //
        Debug.Log("tag: " + draggableItem.originalParent.tag);
        if (draggableItem.originalParent.CompareTag("SrcSlot") && (transform.childCount == 0 || transform.CompareTag("TrashSlot")) && !transform.CompareTag("SrcSlot"))
        {
            Transform newItem = Instantiate(draggableItem.transform);
            newItem.SetParent(draggableItem.originalParent);
            newItem.name = "Item";
            newItem.GetComponent<DraggableItem>().image.raycastTarget = true;
            draggableItem.parentAfterDrag = transform;
            draggableItem.originalParent = draggableItem.parentAfterDrag;
        }
        //
        else if (transform.childCount == 0)
        {
            draggableItem.parentAfterDrag = transform;
        }
        if (transform.CompareTag("TrashSlot"))
        {
            Destroy(dropped.gameObject);
        }
    }
}
