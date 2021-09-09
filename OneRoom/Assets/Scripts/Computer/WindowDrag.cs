using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IBeginDragHandler, IDragHandler {
    public Transform dragObject;

    float defaultZ;

    public void OnBeginDrag(PointerEventData eventData) {
        dragObject.SetAsLastSibling();

        defaultZ = transform.position.z;
    }

    public void OnDrag(PointerEventData eventData) {
        dragObject.transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0f);

        dragObject.transform.position = new Vector3(dragObject.transform.position.x, dragObject.transform.position.y, defaultZ);
    }
}
