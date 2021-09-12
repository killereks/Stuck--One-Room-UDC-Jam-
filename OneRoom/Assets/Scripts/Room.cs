using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Room : MonoBehaviour
{
    public Vector2Int roomPosition; 
    public Transform doorTransform;
    public BoxCollider insideRoomCollider;

    public PaintingTransition paintingTransition; // can be null
    public Transform camPosition;
    public Transform itemParent;

    public Vector2Int roomCoordinates;

    public Camera roomCam;

    public bool doorTransitionEven { get; set; }
    public bool pictureTransitionEven { get; set; }

    public void SetRoomCoordinates(Vector2Int number)
    {
        roomCoordinates = number;
    }

    public void OnTriggerEnter(Collider other)
    {
        ItemWorld item = other.GetComponent<ItemWorld>();

        if (item != null) {
            item.transform.SetParent(itemParent);
        }

        if(other.GetComponent<PlayerMovement>() != null)
        {
            RoomManager.currentPlayerRoom = this;
        }
        
    }

    public void ParentPickupsToRoom()
    {
        foreach(Collider collider in Physics.OverlapBox(insideRoomCollider.transform.position, insideRoomCollider.size * 0.5f))
        {
            if (collider.CompareTag("Pickupable")) collider.transform.SetParent(itemParent);

        }
    }

    //4 positions
    //door 0 - door 1
    //picture 0 - picture 1

    //looks at the door from front/back 
    // -> the next room will "spawn" in the opposite one if you go through the door


}
