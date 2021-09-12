using UnityEngine;

public class Shatter : MonoBehaviour {

    Rigidbody[] rigidbodies;

    public GameObject normalPot;
    public GameObject brokenPot;

    void Start(){
        rigidbodies = brokenPot.GetComponentsInChildren<Rigidbody>();
    }


    public void EnableShatter() {
        brokenPot.SetActive(true);
        normalPot.SetActive(false);

        foreach (Rigidbody rb in rigidbodies) {
            rb.isKinematic = false;

            rb.AddForce(rb.transform.position - PlayerMovement.Instance.transform.position, ForceMode.VelocityChange);

            LeanTween.scale(rb.gameObject, Vector3.zero, 2f).setDelay(2f).setEaseInOutSine().setOnComplete(() => {
                Destroy(rb.gameObject);
            });
        }
    }
}
