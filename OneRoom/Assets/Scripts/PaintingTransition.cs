using System.Collections;
using UnityEngine;

public class PaintingTransition : MonoBehaviour, IInteractable {

    public Camera linkedCam;

    public Cinemachine.CinemachineVirtualCamera vCam;

    [Range(0,3)]
    public float scale;

    MeshRenderer meshRenderer;

    public void Interact() {
        vCam.Priority = 250;

        Vector3 pos = linkedCam.transform.position;
        pos.y = PlayerMovement.Instance.cam.transform.position.y;
        linkedCam.transform.position = pos;

        PlayerMovement.Instance.ToggleMovement(false);

        LeanTween.delayedCall(1f, () => {
            vCam.Priority = 0;
            PlayerMovement player = PlayerMovement.Instance;

            player.ToggleMovement(true);

            Vector3 targetPos = linkedCam.transform.position;

            if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit)) {
                targetPos.y = hit.point.y + 0.9f;
            }
            player.transform.position = targetPos;

            player.cam.transform.rotation = linkedCam.transform.rotation;
            player.SetXCameraRotation(linkedCam.transform.eulerAngles.x);

            PaintingTransitionManager.instance.UpdateAllPaintings();
        });
    }

    private void OnValidate() {
        SetPaintingScale();
    }

    void Start(){
        RenderImages();

        SetPaintingScale();
        SnapCameraToCanvas();
    }

    public void RenderImages() {
        meshRenderer = GetComponent<MeshRenderer>();

        linkedCam.enabled = true;

        RenderTexture renderTexture = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);

        linkedCam.targetTexture = renderTexture;
        linkedCam.Render();
        linkedCam.enabled = false;

        meshRenderer.material.mainTexture = renderTexture;
    }

    void SetPaintingScale() {
        transform.localScale = new Vector3(linkedCam.aspect, 1f, transform.localScale.z) * scale;
    }

    void OnDrawGizmos() {
        if (meshRenderer == null) {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(meshRenderer.bounds.center, meshRenderer.bounds.size);
    }

    void SnapCameraToCanvas() {
        //vCam.transform.rotation = transform.rotation;

        Vector3 canvasPos = meshRenderer.bounds.center;//transform.position;
        float x = canvasPos.x;
        float y = canvasPos.y;
        float z = canvasPos.z;

        float fov = vCam.m_Lens.FieldOfView / 2f;
        float canvasHeight = meshRenderer.bounds.size.y;// * transform.lossyScale.y;

        float opposite = canvasHeight / 2f;
        float adjacent = opposite / Mathf.Tan(Mathf.Deg2Rad * fov);
        vCam.transform.position = new Vector3(x, y, z) - vCam.transform.forward * adjacent;
        vCam.m_Lens.FarClipPlane = Mathf.Ceil(adjacent);
    }


}
