using UnityEngine;
using UnityEngine.EventSystems;

namespace RouteEditors
{
    public class RouteRotator : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject slot;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (slot.transform.childCount > 0)
            {
                slot.transform.GetChild(0).transform.Rotate(new Vector3(0, 0, 90));
            }
        }
    }
}
