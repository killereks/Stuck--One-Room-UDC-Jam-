using UnityEngine;

public class PaintingTransitionManager : MonoBehaviour {

    PaintingTransition[] paintingTransitions;

    public static PaintingTransitionManager instance;

    void Start(){
        paintingTransitions = FindObjectsOfType<PaintingTransition>();

        instance = this;
    }

    public void UpdateAllPaintings() {
        for (int i = 0; i < 3; i++) {
            foreach (PaintingTransition paintingTransition in paintingTransitions) {
                paintingTransition.RenderImages();
            }
        }
    }
}
