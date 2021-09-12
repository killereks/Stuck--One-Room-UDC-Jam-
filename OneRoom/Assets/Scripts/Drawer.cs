using UnityEngine;

public class Drawer : MonoBehaviour, IInteractable {

    public Item itemToUnlock;
    public bool isLocked;

    bool isOpen;

    public float openAmount;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip lockedSound;
    public AudioClip openSound;
    public AudioClip closedSound;

    Vector3 defaultPos;

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * openAmount);
    }

    private void Start() {
        defaultPos = transform.localPosition;
    }

    public void Interact() {
        if (isLocked) {
            if (PlayerInventory.instance.ItemInHand() == itemToUnlock) {
                isLocked = false;
            } else {
                audioSource.PlayOneShot(lockedSound);

                LeanTween.moveLocal(gameObject, defaultPos - transform.right * 0.02f, 0.1f).setLoopPingPong(2);
            }
            return;
        }

        isOpen = !isOpen;

        if (isOpen) {
            audioSource.PlayOneShot(openSound);

            LeanTween.cancel(gameObject);
            LeanTween.moveLocal(gameObject, defaultPos - transform.right * openAmount, 1f).setEaseInOutSine();
        } else {
            audioSource.PlayOneShot(closedSound);

            LeanTween.cancel(gameObject);
            LeanTween.moveLocal(gameObject, defaultPos, 1.8f).setEaseInOutSine();
        }
    }
}
