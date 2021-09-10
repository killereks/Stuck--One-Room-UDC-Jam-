using UnityEngine;
using NaughtyAttributes;

public class PlayerInteraction : MonoBehaviour {

    [BoxGroup("General Settings")]
    public Camera cam;

    [Range(0.5f, 3f), BoxGroup("Gameplay Settings")]
    public float interactionDistance = 2f;

    [BoxGroup("UI")]
    public GameObject interactionCrosshair;

    public static PlayerInteraction instance;

    private void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update(){
        /*if (PickupController.instance.currentlyHoldingItem() && Input.GetKeyDown(KeyCode.F)) {
            PickupController.instance.Letgo();
        } else {
            
        }*/
        InteractionRay();
    }

    void InteractionRay() {
        Vector2 screenMiddle = new Vector2(Screen.width/2f, Screen.height/2f);

        Ray ray = cam.ScreenPointToRay(screenMiddle);
        RaycastHit hit;

        bool hitSomething = false;

        // CONSIDER USING A MASK/LAYER
        if (Physics.Raycast(ray, out hit, interactionDistance)) {
            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();

            if (interactable != null) {
                hitSomething = true;
                // USE INPUT MANAGER
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) {
                    interactable.Interact();
                }
            } else if (hit.collider.CompareTag("Pickupable") && Input.GetKeyDown(KeyCode.E)) {
                print("pickup");
                PickupController.instance.Pickup(hit.collider.gameObject);
            }
        }

        interactionCrosshair.SetActive(hitSomething);
    }
}
