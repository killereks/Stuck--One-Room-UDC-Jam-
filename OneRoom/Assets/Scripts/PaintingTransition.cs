using UnityEngine;

public class PaintingTransition : MonoBehaviour, IInteractable {

    public Camera linkedCam;

    public Cinemachine.CinemachineVirtualCamera vCam;
    public Cinemachine.CinemachineVirtualCamera transitionVCam;

    MeshRenderer meshRenderer;

    public string GetDescription() {
        return "Look at painting";
    }

    public void Interact() {
        vCam.Priority = 250;

        LeanTween.delayedCall(1f, () => {
            vCam.Priority = 0;
            transitionVCam.Priority = 250;
        });
    }

    void Start(){
        meshRenderer = GetComponent<MeshRenderer>();

        RenderTexture renderTexture = new RenderTexture(1920, 1080, 16, RenderTextureFormat.ARGB32);

        linkedCam.targetTexture = renderTexture;
        linkedCam.Render();
        linkedCam.enabled = false;

        transform.localScale = new Vector3(linkedCam.aspect, 1f, 1f);

        meshRenderer.material.mainTexture = renderTexture;

        SnapCameraToCanvas();
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
        float canvasHeight = meshRenderer.bounds.size.y * transform.lossyScale.y;

        float opposite = canvasHeight / 2f;
        float adjacent = opposite / Mathf.Tan(Mathf.Deg2Rad * fov);
        vCam.transform.position = new Vector3(x, y, z) - vCam.transform.forward * adjacent;
        vCam.m_Lens.FarClipPlane = Mathf.Ceil(adjacent);

        /*Bounds bounds = meshRenderer.bounds;

        float cameraDistance = 2.0f; // Constant factor
        Vector3 objectSizes = bounds.max - bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * vCam.m_Lens.FieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
        vCam.transform.position = bounds.center - distance * vCam.transform.forward;*/
    }


}
