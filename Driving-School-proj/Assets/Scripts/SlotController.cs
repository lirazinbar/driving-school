using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            // CheckIfSourceSlotIsEmpty(dropped);
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
            // draggableItem.originalParent = transform;

        }
    }

    private void CheckIfSourceSlotIsEmpty(GameObject item)
    {
        Instantiate(item);
    }
}
