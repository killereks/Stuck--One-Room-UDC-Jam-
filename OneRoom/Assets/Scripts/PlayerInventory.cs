using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    public Item item;

    public static PlayerInventory instance;

    public Image itemIconImage;

    private void Start() {
        instance = this;
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
        if (item == null) {
            itemIconImage.sprite = null;
        } else {
            itemIconImage.sprite = item.icon;
        }
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (item != null) {
                Instantiate(item.prefab, transform.position, Quaternion.identity);
                item = null;
                UpdateUI();
            }
        }
    }
}
