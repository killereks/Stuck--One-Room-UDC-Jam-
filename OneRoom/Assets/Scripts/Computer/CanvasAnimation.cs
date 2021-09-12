using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using NaughtyAttributes;

public class CanvasAnimation : MonoBehaviour, IInteractable {
    Vector3 defaultPos;
    Quaternion defaultRot;
    Vector3 defaultScale;
    Vector2 defaultSize;

    public CinemachineVirtualCamera vCam;
    public Canvas canvas;
    RectTransform canvasRect;
    CanvasScaler canvasScaler;

    bool usingComputer;

    // TO DO, add a delay between changing the states of 'usingComputer' otherwise the invoke racks up and its baaad

    private void Start() {
        canvasRect = canvas.GetComponent<RectTransform>();
        canvasScaler = canvas.GetComponent<CanvasScaler>();

        defaultSize = canvasRect.sizeDelta;
        defaultPos = canvasRect.localPosition;
        defaultRot = canvasRect.localRotation;
        defaultScale = canvasRect.localScale;

        SnapCameraToCanvas();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F) && usingComputer) {
            GameObject currentlySelectedGO = EventSystem.current.currentSelectedGameObject;
            // make sure we have nothing selected, might break with buttons or other ui elements
            if (currentlySelectedGO == null) {
                InteractWithComputer(false);
            } // make sure we aren't typing something in
            else if (currentlySelectedGO && currentlySelectedGO.GetComponent<TMPro.TMP_InputField>() == null) {
                InteractWithComputer(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && usingComputer) {
            InteractWithComputer(false);
        }
    }

    bool Typing() {
        return Input.anyKeyDown && !( Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) );
    }

    public void InteractWithComputer(bool newState) {
        float blendTime = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;

        usingComputer = newState;
        if (usingComputer) {
            vCam.Priority = 250;
            PlayerMovement.Instance.LockCursor(false);
            Invoke(nameof(AnimationIn), blendTime);
        } else {
            vCam.Priority = 1;
            LeanTween.delayedCall(0.2f, () => PlayerMovement.Instance.LockCursor(true));
            AnimationOut();
        }
    }

    void SnapCameraToCanvas() {
        vCam.transform.rotation = canvas.transform.rotation;

        Vector3 canvasPos = canvas.transform.position;
        float x = canvasPos.x;
        float y = canvasPos.y;
        float z = canvasPos.z;

        float fov = vCam.m_Lens.FieldOfView / 2f;
        float canvasHeight = ((RectTransform) canvas.transform).rect.height * canvas.transform.lossyScale.y;

        float opposite = canvasHeight / 2f;
        float adjacent = opposite / Mathf.Tan(Mathf.Deg2Rad * fov);
        vCam.transform.position = new Vector3(x,y,z) - vCam.transform.forward * adjacent;
        vCam.m_Lens.FarClipPlane = Mathf.Ceil(adjacent);
    }

    // going into the computer
    public void AnimationIn() {
        if (!usingComputer) return;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);
    }
    // leaving the computer
    public void AnimationOut() {
        canvas.renderMode = RenderMode.WorldSpace;
        ResetTransform();
    }

    void ResetTransform() {
        canvasRect.sizeDelta = defaultSize;
        canvasRect.localPosition = defaultPos;
        canvasRect.localRotation = defaultRot;
        canvasRect.localScale = defaultScale;
    }

    public void Interact() {
        if (!usingComputer) {
            InteractWithComputer(true);
        }
    }
}