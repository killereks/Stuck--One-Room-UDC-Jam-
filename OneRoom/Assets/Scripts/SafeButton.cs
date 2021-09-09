using UnityEngine;

public class SafeButton : MonoBehaviour, IInteractable {

    public SafeManager safeManager;
    public char buttonCharacter;

    public void Interact() {
        safeManager.InsertCode(buttonCharacter);
    }
}
