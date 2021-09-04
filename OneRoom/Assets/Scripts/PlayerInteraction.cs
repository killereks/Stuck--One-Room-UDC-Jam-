using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour {

    [BoxGroup("General Settings")]
    public Camera cam;

    [Range(0.5f, 3f), BoxGroup("Gameplay Settings")]
    public float interactionDistance = 2f;

    [BoxGroup("UI")]
    public GameObject interactionUI;
    [BoxGroup("UI")]
    public TMPro.TextMeshProUGUI interactionText;

    public static PlayerInteraction instance;

    private void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update(){
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
                interactionText.text = interactable.GetDescription();
                // USE INPUT MANAGER
                if (Input.GetKeyDown(KeyCode.E)) {
                    interactable.Interact();
                }
            }
        }

        interactionUI.SetActive(hitSomething);
    }
}
