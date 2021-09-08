using UnityEngine;
using UnityEngine.EventSystems;

public class Window : MonoBehaviour {

    public void Close() {
        LeanTween.scaleY(gameObject, 0f, 0.2f);
        LeanTween.scaleX(gameObject, 0f, 0.2f).setDelay(0.1f).setOnComplete(() => {
            Destroy(gameObject);
        });
    }
}
