using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Door : MonoBehaviour, IInteractable
{

    float targetYRotation;

    [BoxGroup("Settings")]
    public float smooth;
    [BoxGroup("Settings")]
    public bool autoClose;
    [BoxGroup("Settings")]
    public Transform hinge;
    [BoxGroup("Settings")]
    public bool invertOpeningDir;
    [BoxGroup("Settings")]
    public bool isLockedByDefault = true;

    [BoxGroup("AI Settings")]
    public Transform lockT;


    Transform player;

    bool isOpen;
    private string description;

    public bool IsLocked
    {
        get;
        private set;
    }

    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
    }

    float timer = 0f;
    Vector3 defaultRot;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        defaultRot = hinge.eulerAngles;

        SetLocked(isLockedByDefault);
    }

    private void Update()
    {
        hinge.rotation = Quaternion.Lerp(hinge.rotation, Quaternion.Euler(defaultRot.x, defaultRot.y + targetYRotation, defaultRot.z), smooth * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer <= 0f && isOpen && autoClose)
        {
            ToggleDoor(player.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(hinge.position, hinge.forward);
    }

    public string GetDescription()
    {
        return "door";
    }

    public void Interact()
    {
        description = "Open the door";
        if (isOpen) description = "Close the door";


        if (IsLocked)
        {
            ToggleDoor(player.position);
        }
    }

        public void SetLocked(bool newState)
    {
        IsLocked = newState;
    }

    public void ToggleDoor(Vector3 pos)
    {
        if (isOpen && !IsLocked) return;
        isOpen = !isOpen;

        if (isOpen)
        {
            Debug.Log("closing");
            Vector3 dir = (pos - transform.position);
            targetYRotation = -Mathf.Sign(Vector3.Dot(hinge.forward, dir)) * 90f;
            if (invertOpeningDir)
            {
                targetYRotation *= -1f;
            }
            timer = 5f;
        }
        else
        {
            Debug.Log("opening");
            targetYRotation = 0f;
        }
    }

    public void Open(Vector3 pos)
    {
        if (IsLocked) return;

        if (!isOpen)
        {
            ToggleDoor(pos);
        }
    }
    public void Close(Vector3 pos)
    {
        if (isOpen)
        {
            ToggleDoor(pos);
        }
    }
}