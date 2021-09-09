using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour, IInteractable {

    [ColorUsage(true, true)]
    public Color color;

    public Transform liquidT;
    public Transform spoonT;

    public float spoonRotationSpeed;

    Color lastColor;

    public Material liquidMaterial;

    LTDescr colorAnimationDesc;

    bool canInteract;

    List<Color> colors = new List<Color>();

    private void Start() {
        colors.Add(color);
        UpdateColor();
    }
    private void OnValidate() {
        liquidMaterial.SetColor("_Color", color);
    }

    public void MixColor(Color color) {
        colors.Add(color);

        LeanTween.rotateAround(spoonT.gameObject, Vector3.up, spoonRotationSpeed, 2f).setEaseInOutSine();
        LeanTween.rotateAround(liquidT.gameObject, Vector3.up, spoonRotationSpeed/2f, 1f).setDelay(0.5f).setEaseInOutSine();

        UpdateColor();
    }

    void UpdateColor() {
        canInteract = false;

        Vector3 colorAverage = Vector3.zero;

        for (int i = 0; i < colors.Count; i++) {
            Color color = colors[i];
            colorAverage.x += color.r;
            colorAverage.y += color.g;
            colorAverage.z += color.b;
        }

        Color outcomeColor = new Color(colorAverage.x / colors.Count, colorAverage.y / colors.Count, colorAverage.z / colors.Count);

        colorAnimationDesc = LeanTween.value(0f, 1f, 1f).setDelay(1f).setOnUpdate((float value) => {
            Color currentColor = Color.Lerp(lastColor, outcomeColor, value);
            liquidMaterial.SetColor("_Color", currentColor);
            liquidMaterial.SetFloat("_SwirlPower", value * -35f);
        }).setOnComplete(() => {
            canInteract = true;

            liquidMaterial.SetFloat("_SwirlPower", 0f);

            lastColor = outcomeColor;
        });
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            MixColor(Color.red);
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            MixColor(Color.green);
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            MixColor(Color.blue);
        }

    }

    public void Interact() {
        if (!canInteract) return;

        MixColor(Color.red);
    }

    // 0 -> 0
    // 0.5 -> 1
    // 1 -> 0

    // 0 -> 0
    // pi/2 -> 1

    public float SineDecay(float value) {
        value = Mathf.Clamp01(value);

        return Mathf.Sin(value * Mathf.PI);
    }
}
