using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IBeginDragHandler, IDragHandler {
    public Transform dragObject;

    public void OnBeginDrag(PointerEventData eventData) {
        dragObject.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        dragObject.transform.position += (Vector3)eventData.delta;
    }
}
