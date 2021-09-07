using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Door : MonoBehaviour, IInteractable {

    float targetYRotation;

    [BoxGroup("Settings")]
    public float smooth;
    [BoxGroup("Settings")]
    public bool autoClose;
    [BoxGroup("Settings")]
    public Transform hinge;
    [BoxGroup("Settings")]
    public bool invertOpeningDir;

    Transform player;

    bool isOpen;

    public bool IsOpen {
        get{
            return isOpen;
        }
    }

    float timer = 0f;
    Vector3 defaultRot;

    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        defaultRot = hinge.eulerAngles;
    }

    private void Update(){
        hinge.rotation = Quaternion.Lerp(hinge.rotation, Quaternion.Euler(defaultRot.x, defaultRot.y + targetYRotation, defaultRot.z), smooth * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer <= 0f && isOpen && autoClose){
            ToggleDoor(player.position);
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.DrawRay(hinge.position, hinge.forward);
    }

    public void Interact(){
/*        description = "Open the door";
        if (isOpen) description = "Close the door";*/

        ToggleDoor(player.position);
    }

    public void ToggleDoor(Vector3 pos)
    {
        isOpen = !isOpen;

        if (isOpen){
            Vector3 dir = (pos - transform.position);
            targetYRotation = -Mathf.Sign(Vector3.Dot(hinge.forward, dir)) * 90f;
            if (invertOpeningDir)
            {
                targetYRotation *= -1f;
            }
            timer = 5f;
        }
        else{
            targetYRotation = 0f;
        }
    }

    public void Open(Vector3 pos){

        if (!isOpen){
            ToggleDoor(pos);
        }
    }
    public void Close(Vector3 pos){
        if (isOpen){
            ToggleDoor(pos);
        }
    }
}