using UnityEngine;

public class PuzzleCraft : MonoBehaviour {

    public Item otherItem;
    public GameObject outcome;

    public bool destroyTargetOnUsage;

    public void AttemptCraft() {
        if (PlayerInventory.instance.ItemInHand() == otherItem) {
            PlayerInventory.instance.RemoveItem();
            Instantiate(outcome, transform.position, transform.rotation);
            if (destroyTargetOnUsage) {
                Destroy(gameObject);
            }
        }
    }
}
