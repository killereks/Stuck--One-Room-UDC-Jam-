using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class SafeManager : MonoBehaviour {

    string currentCode;

    public string correctCode;

    public Transform door;
    public TextMeshPro displayText;

    bool isOpen;

    private void Start() {
        currentCode = string.Empty;
    }

    public void InsertCode(char character) {
        if (isOpen) return;

        if (character == '*') {
            if (currentCode.Length > 0) {
                currentCode = currentCode.Substring(0, currentCode.Length - 1);
            }
        } else if (character == '#') {
            if (currentCode == correctCode) {
                // correct code
                LeanTween.rotateAround(door.gameObject, Vector3.up, -90f, 0.5f).setEaseInOutSine();

                isOpen = true;
            } else {
                // alarm noise - wrong code
                Clear();
            }
        } else {
            if (currentCode.Length < correctCode.Length) {
                currentCode += character;
            } else {
                // trying to input too many characters
                // beep noise
            }
        }

        displayText.text = currentCode;
    }

    public void Clear() {
        if (isOpen) return;

        currentCode = "";
        displayText.text = "";
    }

}
