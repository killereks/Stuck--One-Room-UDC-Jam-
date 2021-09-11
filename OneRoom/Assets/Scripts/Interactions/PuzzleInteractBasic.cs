using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class PuzzleInteractBasic : MonoBehaviour, IInteractable {

    [Tooltip("Item needed to interact"), Required]
    public Item itemRequired;
    [Tooltip("Remove item from inventory after player successfully interacts with this")]
    public bool removeItemAfterUse;
    [Tooltip("What happens after player successfully interacts with this")]
    public UnityEvent outcome;
    [Tooltip("This puzzle can only be done once")]
    public bool oneTimeUse;

    bool used;

    public void Interact() {
        if (oneTimeUse && used) return;

        Item itemInHand = PlayerInventory.instance.ItemInHand();

        if (itemInHand == itemRequired) {
            if (removeItemAfterUse) {
                PlayerInventory.instance.RemoveItem();
            }
            used = true;
            outcome.Invoke();
        }
    }
}
