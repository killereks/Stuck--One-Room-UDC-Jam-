using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    List<List<Room>> rooms = new List<List<Room>>();

    public List<Room> pastRooms;
    public List<Room> presentRooms;
    public List<Room> futureRooms;

    Vector2Int currentRoomIndex;

    Room currentRoom;

    Room nextRoomDimension;
    Room nextRoomTime;

    public Transform roomPosition, roomPositionMirrored;
    public Transform roomPositionTimewarped, roomPositionMirroredTimewarped;
    public Transform fakeTimePosition;

    public static RoomManager Instance;


    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        rooms.Add(pastRooms);
        rooms.Add(presentRooms);
        rooms.Add(futureRooms);


        for (int y = 0; y < rooms.Count; y++)
        {
            for (int x = 0; x < rooms[y].Count; x++)
            {

                rooms[x][y].SetRoomCoordinates(new Vector2Int(x,y));
                rooms[x][y].gameObject.SetActive(false);
                //also set position in the array
            }

        }

        SetupFirstRoom();
    }

    public void SetupFirstRoom()
    {
        currentRoomIndex = Vector2Int.zero;
        currentRoom = rooms[currentRoomIndex.x][currentRoomIndex.y];
        currentRoom.gameObject.SetActive(true);

        currentRoom.doorTransitionEven = true;
        currentRoom.pictureTransitionEven = true;

        PositionRoom(currentRoom);

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
        currentRoomIndex.y = (currentRoomIndex.y + 1) % rooms.Count;
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

        roomIndexX.x = (currentRoomIndex.x + 1) % rooms[currentRoomIndex.y].Count;
        roomIndexY.y = (currentRoomIndex.y + 1) % rooms.Count;

        //spawn other dimension room
        nextRoomDimension = rooms[roomIndexX.x][roomIndexX.y];
        nextRoomDimension.gameObject.SetActive(true);
        
        nextRoomDimension.doorTransitionEven = !currentRoom.doorTransitionEven;
        nextRoomDimension.pictureTransitionEven = currentRoom.pictureTransitionEven;

        PositionRoom(nextRoomDimension);

        nextRoomDimension.insideRoomCollider.enabled = true;

        //spawn other time room

        nextRoomTime = rooms[roomIndexY.x][roomIndexY.y];
        nextRoomTime.gameObject.SetActive(true);
        

        nextRoomTime.doorTransitionEven = currentRoom.doorTransitionEven;
        nextRoomTime.pictureTransitionEven = !currentRoom.pictureTransitionEven;

        PositionRoom(nextRoomTime);
        

        currentRoom.roomCam.transform.position = nextRoomTime.camPosition.position;

        nextRoomTime.insideRoomCollider.enabled = false;

        //PeekThroughTime();
    }

    public void PeekThroughTime()
    {
        Vector2Int coordinates = nextRoomTime.roomCoordinates;
        coordinates.y = (coordinates.y + 1) % rooms.Count;
        Room peekRoom = rooms[coordinates.x][coordinates.y];

        peekRoom.transform.position = fakeTimePosition.position;
        peekRoom.gameObject.SetActive(true);

        //take a snapshot
        currentRoom.paintingTransition.RenderImages();
        nextRoomTime.paintingTransition.RenderImages();
        peekRoom.paintingTransition.RenderImages();

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
