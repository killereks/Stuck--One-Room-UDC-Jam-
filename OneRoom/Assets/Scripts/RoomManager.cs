using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RoomManager : MonoBehaviour
{
    List<List<Room>> rooms = new List<List<Room>>();

    
    [BoxGroup("Room Setup")]
    public List<Room> pastRooms;
    [BoxGroup("Room Setup")]
    public List<Room> presentRooms;
    [BoxGroup("Room Setup")]
    public List<Room> futureRooms;

    Vector2Int currentRoomIndex;

    [BoxGroup("Settings")]
    public Vector2Int startingRoomIndex;

    Room currentRoom;

    public Room nextRoomDimension { get; set; }
    public Room nextRoomTime { get; set; }

    public Transform roomPosition, roomPositionMirrored;
    public Transform roomPositionTimewarped, roomPositionMirroredTimewarped;
    public Transform fakeTimePosition;

    public static RoomManager Instance;

    public Door door, doorTimewarped;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        rooms.Clear();
        rooms.Add(pastRooms);
        rooms.Add(presentRooms);
        rooms.Add(futureRooms);

        for (int y = 0; y < rooms.Count; y++)
        {
            for (int x = 0; x < rooms[y].Count; x++)
            {
                rooms[y][x].SetRoomCoordinates(new Vector2Int(y,x));
                rooms[y][x].gameObject.SetActive(false);
                //also set position in the array
            }

        }

        SetupFirstRoom();
    }

    public void SetupFirstRoom()
    {
        //currentRoomIndex = Vector2Int.zero;
        currentRoomIndex = startingRoomIndex;
        currentRoom = rooms[currentRoomIndex.y][currentRoomIndex.x];
        currentRoom.gameObject.SetActive(true);

        currentRoom.doorTransitionEven = true;
        currentRoom.pictureTransitionEven = true;

        PositionRoom(currentRoom);
        PlayerMovement.Instance.transform.position = currentRoom.camPosition.position;
        PlayerMovement.Instance.currentRoom = currentRoom;
        currentRoom.insideRoomCollider.enabled = false;

        SetupNextRooms();
    }

    public Room ClosedInAnotherRoom()
    {
        //
        currentRoomIndex.x = (currentRoomIndex.x + 1) % rooms[currentRoomIndex.y].Count;
        Room roomToDeload = currentRoom;
        currentRoom = nextRoomDimension;

        nextRoomTime.gameObject.SetActive(false);
        SetupNextRooms();

        //unload current room
        if(roomToDeload != nextRoomDimension)
        {
            roomToDeload.gameObject.SetActive(false);
        }

        return currentRoom;
    }

    public Room WentThroughPainting()
    {
        //
        currentRoomIndex.y = ((currentRoomIndex.y - 1) < 0) ? (rooms.Count - 1) : (currentRoomIndex.y - 1);
        Room roomToDeload = currentRoom;
        currentRoom = nextRoomTime;
        nextRoomDimension.gameObject.SetActive(false);
        SetupNextRooms();

        //unload current room
        if (roomToDeload != nextRoomTime)
        {
            roomToDeload.gameObject.SetActive(false);
        }

        return currentRoom;
    }

    public void SetupNextRooms()
    {
        //setup 2 rooms -> 1 in each direction
        Vector2Int roomIndexX = currentRoomIndex;
        Vector2Int roomIndexY = currentRoomIndex;

        roomIndexX.x = (roomIndexX.x + 1) % rooms[roomIndexX.y].Count;
        roomIndexY.y = ((roomIndexY.y - 1) < 0) ? (rooms.Count - 1) : (roomIndexY.y - 1);

        //spawn other dimension room
        nextRoomDimension = rooms[roomIndexX.y][roomIndexX.x];
        nextRoomDimension.gameObject.SetActive(true);
        
        nextRoomDimension.doorTransitionEven = !currentRoom.doorTransitionEven;
        nextRoomDimension.pictureTransitionEven = currentRoom.pictureTransitionEven;

        PositionRoom(nextRoomDimension);

        nextRoomDimension.insideRoomCollider.enabled = true;

        //spawn other time room

        nextRoomTime = rooms[roomIndexY.y][roomIndexY.x];
        nextRoomTime.gameObject.SetActive(true);
        

        nextRoomTime.doorTransitionEven = currentRoom.doorTransitionEven;
        nextRoomTime.pictureTransitionEven = !currentRoom.pictureTransitionEven;

        PositionRoom(nextRoomTime);
        Debug.Log((nextRoomTime != null) + " " + (nextRoomDimension != null));

        currentRoom.roomCam.transform.position = nextRoomTime.camPosition.position;

        nextRoomTime.insideRoomCollider.enabled = false;

        PeekThroughTime();
    }

    public void PeekThroughTime()
    {
        Vector2Int coordinates = nextRoomTime.roomCoordinates;
        //coordinates.y = (currentRoomIndex.y + 1) % rooms.Count;
        coordinates.y = ((coordinates.y - 1) < 0) ? (rooms.Count - 1) : (coordinates.y - 1);
        Room peekRoom = rooms[coordinates.y][coordinates.x];

        peekRoom.transform.position = fakeTimePosition.position;
        peekRoom.gameObject.SetActive(true);

        peekRoom.roomCam.transform.position = currentRoom.camPosition.position;
        nextRoomTime.roomCam.transform.position = peekRoom.camPosition.position;

        //take a snapshot
        peekRoom.paintingTransition.RenderImages();
        nextRoomTime.paintingTransition.RenderImages();
        currentRoom.paintingTransition.RenderImages();

        peekRoom.gameObject.SetActive(false);
    }

    public void PositionRoom(Room room)
    {
        if (room.pictureTransitionEven)
        {
            room.transform.position = room.doorTransitionEven ? roomPosition.position : roomPositionMirrored.position;
            room.transform.rotation = room.doorTransitionEven ? roomPosition.rotation : roomPositionMirrored.rotation;
        }
        else
        {
            room.transform.position = room.doorTransitionEven ? roomPositionTimewarped.position : roomPositionMirroredTimewarped.position;
            room.transform.rotation = room.doorTransitionEven ? roomPositionTimewarped.rotation : roomPositionMirroredTimewarped.rotation;
        }

    }

}
