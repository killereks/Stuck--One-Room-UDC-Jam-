using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    public Item item;

    public static PlayerInventory instance;

    public Image itemIconImage;

    GameObject placeItem = null;
    Bounds placeItemBounds;

    private void Start() {
        instance = this;

        UpdateUI();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemToPickUp"></param>
    /// <returns>True if item was picked up</returns>
    public bool PickupItem(Item itemToPickUp) {
        if (item == null) {
            item = itemToPickUp;
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool IsFull() {
        return item != null;
    }

    public void UpdateUI() {
        itemIconImage.gameObject.SetActive(item != null);

        if (item != null) {
            itemIconImage.sprite = item.icon;
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (item != null) {
                placeItem = Instantiate(item.prefab, transform.position, Quaternion.identity);
                placeItemBounds = placeItem.GetComponent<MeshRenderer>().bounds;
                placeItem.layer = 2;
                item = null;
                UpdateUI();
            }
        }

        if (Input.GetKey(KeyCode.Q)) {
            if (placeItem != null) {
                Ray ray = Camera.main.ViewportPointToRay(Vector3.one / 2f);

                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    placeItem.transform.position = hit.point + Vector3.up * placeItemBounds.size.y / 2f;// + placeItemBounds.size * Vector3.Dot(hit.normal, placeItemBounds.size);
                    placeItem.transform.rotation = Quaternion.FromToRotation(hit.normal, Vector3.up);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Q)) {
            if (placeItem != null) {
                placeItem.layer = 1;
                placeItem = null;
            }
        }
    }
}
