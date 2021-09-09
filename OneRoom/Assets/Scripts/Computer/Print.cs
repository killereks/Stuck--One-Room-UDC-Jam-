using UnityEngine;

public class Print : MonoBehaviour {

    public GameObject paperPrefab;

    public void PrintImage(Sprite image) {
        GameObject newPaper = Instantiate(paperPrefab, transform.position + Vector3.up, Quaternion.identity);

        Texture2D texture = new Texture2D((int)image.rect.width, (int)image.rect.height);
        var pixels = image.texture.GetPixels((int)image.textureRect.x,
                                             (int)image.textureRect.y,
                                             (int)image.textureRect.width,
                                             (int)image.textureRect.height);

        texture.SetPixels(pixels);
        texture.Apply();

        newPaper.GetComponent<MeshRenderer>().material.mainTexture = texture;
    }
}
