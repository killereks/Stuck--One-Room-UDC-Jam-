using UnityEngine;

public class TV : MonoBehaviour {

    public Texture texture;
    public Material material;

    private void Start() {
        material.mainTexture = null;
    }

    public void SetTexture() {
        material.mainTexture = texture;
    }
}
