using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cauldron : MonoBehaviour, IInteractable {

    public Transform liquidT;
    public Transform spoonT;

    public float spoonRotationSpeed;

    Color lastColor;

    public Material liquidMaterial;

    LTDescr colorAnimationDesc;

    bool canInteract;

    List<Color> colors = new List<Color>();


    public List<Ingredient> ingredients = new List<Ingredient>();

    public UnityEvent onComplete;

    private void Start() {
        liquidMaterial.SetColor("_Color", Color.black);

        canInteract = true;
    }

    public void MixColor(Color color) {
        colors.Add(color);

        LeanTween.rotateAround(spoonT.gameObject, Vector3.up, spoonRotationSpeed, 2f).setEaseInOutSine();
        LeanTween.rotateAround(liquidT.gameObject, Vector3.up, spoonRotationSpeed/2f, 1f).setDelay(0.5f).setEaseInOutSine();

        UpdateColor();
    }

    void UpdateColor() {
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

            if (ingredients.Count == 0) onComplete.Invoke();

            liquidMaterial.SetFloat("_SwirlPower", 0f);

            lastColor = outcomeColor;
        });
    }

    public void Interact() {
        if (!canInteract) return;

        Item item = PlayerInventory.instance.ItemInHand();
        // no item in hand
        if (item == null) return;

        Ingredient ingredient = ingredients.Find(x => x.item == item);

        // invalid ingredient
        if (ingredient == null) return;

        AddIngredient(ingredient);
        ingredients.Remove(ingredient);

        canInteract = false;

        PlayerInventory.instance.RemoveItem();

        LeanTween.delayedCall(2f, () => {
            MixColor(ingredient.color);
        });
    }

    private void AddIngredient(Ingredient ingredient) {
        GameObject ingredientGO = Instantiate(ingredient.item.prefab, transform.position + Vector3.up * 1f, Quaternion.identity);

        Destroy(ingredientGO.GetComponent<ItemWorld>());

        ingredientGO.tag = "Buoyancy";
        buoyancyLogic bLogic = ingredientGO.AddComponent<buoyancyLogic>();

        bLogic.viscosity = 1f;
        bLogic.force = 500f;

        ingredientGO.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.onUnitSphere, ForceMode.Impulse);

        LeanTween.scale(ingredientGO, Vector3.zero, 2f).setDelay(2f).setOnComplete(() => {
            Destroy(ingredientGO);
        });
    }

    [System.Serializable]
    public class Ingredient {
        public Item item;
        public Color color;
    }
}
