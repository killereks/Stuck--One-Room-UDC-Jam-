using UnityEngine;

public class ScrollSize : MonoBehaviour {

    public TMPro.TextMeshProUGUI zoomText;
    public Vector2 minMaxSize;

    public Transform imagePreview;

    float rotation;

    float targetScale = 1f;

    public void Rotate90(bool clockwise) {
        if (clockwise) {
            rotation -= 90f;
        } else {
            rotation += 90f;
        }

        rotation %= 360f;

        LeanTween.rotateZ(imagePreview.gameObject, rotation, 0.3f).setEaseInOutSine();
    }

    // Update is called once per frame
    void Update(){
        targetScale += Input.GetAxis("Mouse ScrollWheel");

        targetScale = Mathf.Clamp(targetScale, minMaxSize.x, minMaxSize.y);

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale * Vector3.one, 10f * Time.deltaTime);

        zoomText.text = transform.localScale.x.ToString("n1") + "x";
    }
}
