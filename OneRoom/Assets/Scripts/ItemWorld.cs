using UnityEngine;

public class ItemWorld : MonoBehaviour, IInteractable {
    public Item item;

    PuzzleCraft puzzleCraft;

    private void Start() {
        puzzleCraft = GetComponent<PuzzleCraft>();
    }

    public void Interact() {
        if (PlayerInventory.instance.PickupItem(item)) {
            LeanTween.scale(gameObject, Vector3.zero, 0.2f).setEaseInOutSine();
            Destroy(gameObject);
        } else if (puzzleCraft != null) {
            puzzleCraft.AttemptCraft();
        }
    }
}
