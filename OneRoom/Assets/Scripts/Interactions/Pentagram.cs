using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pentagram : MonoBehaviour, IInteractable {
    public float radius = 2f;
    public int amountRequired = 5;

    int amountCurrent;

    public GameObject skullPrefab;
    public Item itemRequired;

    List<GameObject> skulls = new List<GameObject>();

    public UnityEvent onComplete;

    private void Start() {
        for (int i = 0; i < amountRequired; i++) {
            float angle = 360f / (float)amountRequired * (float)i * Mathf.Deg2Rad;

            float dx = Mathf.Cos(angle) * radius;
            float dz = Mathf.Sin(angle) * radius;

            Vector3 finalPos = transform.position + new Vector3(dx, 0f, dz);

            skulls.Add(Instantiate(skullPrefab, finalPos, Quaternion.LookRotation(-new Vector3(dx, 0f, dz))));

            skulls[i].SetActive(false);
            skulls[i].transform.SetParent(transform);
            Destroy(skulls[i].GetComponent<Rigidbody>());
            Destroy(skulls[i].GetComponent<ItemWorld>());
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        for (int i = 0; i < amountRequired; i++) {
            float angle = 360f / (float)amountRequired * (float)i * Mathf.Deg2Rad;

            float dx = Mathf.Cos(angle) * radius;
            float dz = Mathf.Sin(angle) * radius;

            Vector3 finalPos = transform.position + new Vector3(dx, 0f, dz);

            Gizmos.DrawSphere(finalPos, 0.05f);
        }
    }

    public void Interact() {
        if (amountCurrent >= amountRequired) return;

        if (PlayerInventory.instance.ItemInHand() == itemRequired) {
            PlayerInventory.instance.RemoveItem();

            GameObject skull = skulls[amountCurrent];
            AnimateSkull(skull);

            amountCurrent++;

            if (amountCurrent >= amountRequired) {
                onComplete.Invoke();
            }
        }
    }

    void AnimateSkull(GameObject skull) {
        Vector3 pos = skull.transform.position;
        Quaternion rotation = skull.transform.rotation;

        skull.SetActive(true);

        skull.transform.position = transform.position;
        skull.transform.rotation = Quaternion.identity;

        LeanTween.value(0f, 1f, 1f).setEaseInOutSine().setOnUpdate((float value) => {
            skull.transform.rotation = Quaternion.Lerp(Quaternion.identity, rotation, value);
            skull.transform.position = Vector3.Lerp(transform.position, pos, value);
        });
    }
}
