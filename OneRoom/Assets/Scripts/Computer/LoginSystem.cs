using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.EventSystems;

public class LoginSystem : MonoBehaviour {

    [BoxGroup("Password")]
    public string correctPassword;
    [BoxGroup("Password")]
    public TMP_InputField passwordField;

    [BoxGroup("Password Hint")]
    public TextMeshProUGUI passwordHintText;
    [BoxGroup("Password Hint")]
    public string passwordHint;

    public GameObject loginScreen;
    public GameObject loadIcon;
    public CanvasGroup loginScreenCanvasGroup;
    public GameObject allLoginUI;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            GameObject selectedGO = EventSystem.current.currentSelectedGameObject;

            if (selectedGO == passwordField.gameObject) {
                Submit();
            }
        }
    }

    public void Submit() {
        if (passwordField.text == correctPassword) {
            //loginScreen.SetActive(false);

            LeanTween.scaleY(allLoginUI, 0f, 0.5f).setEaseInOutSine();
            LeanTween.scaleY(loadIcon, 1f, 0.5f).setDelay(0.5f).setEaseInOutSine().setOnComplete(() => {
                loadIcon.GetComponent<Animator>().Play("In");
            });

            loadIcon.SetActive(true);

            LeanTween.value(1f, 0f, 2f).setDelay(4f).setOnUpdate((float value) => {
                loginScreenCanvasGroup.alpha = value;
            }).setEaseInOutSine().setOnComplete(() => loginScreen.SetActive(false));

        } else {
            passwordHintText.text = $"Hint: {passwordHint}";
            passwordField.text = "";
        }
    }
}
