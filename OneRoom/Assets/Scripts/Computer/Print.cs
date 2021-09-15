using System.Collections;
using UnityEngine;

public class Print : MonoBehaviour {

    public GameObject paperPrefab;
    public Transform paperPosition;

    public Transform printerT;

    bool currentlyPrinting;

    public void PrintImage(Sprite image) {
        if (currentlyPrinting) return;

        currentlyPrinting = true;

        GameObject newPaper = Instantiate(paperPrefab, paperPosition.position, paperPosition.rotation);

        newPaper.transform.SetParent(printerT);

        Texture2D texture = new Texture2D((int)image.rect.width, (int)image.rect.height);
        var pixels = image.texture.GetPixels((int)image.textureRect.x,
                                             (int)image.textureRect.y,
                                             (int)image.textureRect.width,
                                             (int)image.textureRect.height);

        texture.SetPixels(pixels);
        texture.Apply();

        newPaper.GetComponent<MeshRenderer>().material.mainTexture = texture;

        StartCoroutine(AnimatePrinting(newPaper));
    }

    IEnumerator AnimatePrinting(GameObject paper) {
        Vector3 offset = Vector3.left * 0.3f;

        int iterations = 10;

        for (int i = 0; i < iterations; i++) {
            LeanTween.move(paper, paper.transform.position + offset / iterations, 0.3f).setEaseInOutSine();
            yield return new WaitForSeconds(0.4f);
        }

        LeanTween.move(paper, paper.transform.position + offset / iterations * 2f, 0.3f).setEaseInOutSine();

        currentlyPrinting = false;
    }
}
