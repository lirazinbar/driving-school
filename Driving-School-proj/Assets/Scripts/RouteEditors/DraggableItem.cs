using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RouteEditors
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image image;
        [HideInInspector] public Transform parentAfterDrag;
        public Transform originalParent;
        // public Transform position;

        void Start()
        {
            originalParent = transform.parent;
        }
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag");
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag");
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
            // if (originalParent.CompareTag("SrcSlot"))
            // {
            //     Transform newItem = Instantiate(transform, position);
            //     newItem.SetParent(originalParent);
            //     newItem.name = "Item";
            //     originalParent = parentAfterDrag;
            // }
        }
    }
}
