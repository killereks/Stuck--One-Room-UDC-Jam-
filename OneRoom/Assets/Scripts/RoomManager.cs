using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<Room> rooms;
    int currentRoomIndex;

    Room currentRoom;
    Room nextRoom;

    public Transform roomPosition, roomPositionMirrored;

    public static RoomManager Instance;



    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].SetRoomNumber(i);
            rooms[i].gameObject.SetActive(false);
            //also set position in the array
        }

        SetupFirstRoom();
    }

    public void SetupFirstRoom()
    {
        currentRoomIndex = 0;
        currentRoom = rooms[currentRoomIndex];
        currentRoom.gameObject.SetActive(true);
        currentRoom.transform.position = roomPosition.position;
        currentRoom.transform.rotation = roomPosition.rotation;
        currentRoom.doorTransitionEven = true;
        currentRoom.insideRoomCollider.enabled = false;

        SetupNextRoom();
    }

    public void ClosedInAnotherRoom()
    {
        //
        currentRoomIndex = (currentRoomIndex + 1) % rooms.Count; //replace with nextRoom index after you have the key
        Room roomToDeload = currentRoom;
        currentRoom = nextRoom;
        SetupNextRoom();

        //unload current room
        if(roomToDeload != nextRoom)
        {
            roomToDeload.gameObject.SetActive(false);
        }
    }

    public void SetupNextRoom()
    {
        int roomIndex = (currentRoomIndex + 1) % rooms.Count;
        nextRoom = rooms[roomIndex];
        nextRoom.gameObject.SetActive(true);
        
        nextRoom.doorTransitionEven = !currentRoom.doorTransitionEven;
        nextRoom.transform.position = nextRoom.doorTransitionEven ? roomPosition.position : roomPositionMirrored.position;
        nextRoom.transform.rotation = nextRoom.doorTransitionEven ? roomPosition.rotation : roomPositionMirrored.rotation;

        nextRoom.insideRoomCollider.enabled = true;

    }

    public Room GetNextRoom() 
    {
        int roomIndex = (currentRoomIndex + 1) % rooms.Count;

        return rooms[roomIndex];
    }

}
