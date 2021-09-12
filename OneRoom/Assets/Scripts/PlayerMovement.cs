using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

    [BoxGroup("Player Settings")]
    public float walkingSpeed;
    [BoxGroup("Player Settings")]
    public float gravityScale;
    [BoxGroup("Object Settings")]
    public Transform playerT;

    Rigidbody rb;

    [BoxGroup("Object Settings")]
    public Cinemachine.CinemachineVirtualCamera cam;

    float cameraXRotation;
    [BoxGroup("Camera Settings")]
    public Vector2 cameraClamp;
    [BoxGroup("Camera Settings")]
    public float mouseSens = 2f;

    public AudioSource footstepSource;
    public AudioClip[] footstepClips;

    bool canMove;

    Collider collider;

    float footstepTime;
    public float footstepInterval = 4f;

    public static PlayerMovement Instance;

    private void Awake() {
        rb = GetComponent<Rigidbody>();

        collider = GetComponent<Collider>();

        LockCursor(true);
        canMove = true;

        Instance = this;
    }

    public void LockCursor(bool newState) {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !newState;

        ToggleMovement(newState);
    }
    
    public void ToggleMovement(bool newState) {
        canMove = newState;

        if (!canMove) rb.velocity = Vector3.zero;
    }

    public void SetXCameraRotation(float newX) {
        cameraXRotation = newX;
    }

    private void Update() {
        if (canMove) {
            cameraController();
        }
    }

    private void FixedUpdate() {
        movementController();
    }

    private void cameraController() {
        Vector2 mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSens;
        
        playerT.Rotate(Vector3.up, mouse.x, Space.World);

        cameraXRotation -= mouse.y;
        cameraXRotation = Mathf.Clamp(cameraXRotation, cameraClamp.x, cameraClamp.y);

        cam.transform.eulerAngles = new Vector3(cameraXRotation, cam.transform.eulerAngles.y, 0f);
    }

    public void MoveTo(Transform pos) {
        transform.position = pos.position;
    }

    private void movementController() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (!canMove) {
            input = Vector2.zero;
        }

        ManageFootsteps();

        float gravity = rb.velocity.y + Physics.gravity.y * gravityScale * Time.fixedDeltaTime;

        if (IsGrounded() && rb.velocity.y < 0f) {
            gravity = 0f;
        }

        float speed = walkingSpeed;

        Vector3 direction = cam.transform.forward * input.y + cam.transform.right * input.x;
        direction.y = 0f;
        direction = direction.normalized * speed;

        direction *= transform.localScale.magnitude;

        rb.velocity = direction + Vector3.up * gravity;
    }

    void ManageFootsteps() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        Vector3 velocity = rb.velocity;
        velocity.y = 0f; // no footstep sounds in the air

        footstepTime += velocity.magnitude * Time.deltaTime;

        if (footstepTime >= footstepInterval) {
            footstepTime = 0f;
            footstepSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
            //footstepManager.PlayFootstepSound();
            //footstepAudioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position, -Vector3.up, collider.bounds.extents.y + 0.15f, 1);
    }
}
