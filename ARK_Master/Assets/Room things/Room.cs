using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room
{
    public int roomID { get; private set; }

    private int[] roomDimension = { 7, 14 };

    public int[,] layout { get; set; }

    public int dimension { get; set; }

    public string roomState { get; set; }

    public int roomType { get; set; }

    public List<RoomObject> objectList { get; set; }
   

    //public List<Door> roomDoorList { get; set; }

    public Room()
    {

    }

    public Room(int newRoomID, int newRoomType, string newRoomState)
    {
        roomID = newRoomID;
        roomState = newRoomState;
        roomType = newRoomType;
        dimension = roomDimension[newRoomType];
        layout = new int[dimension, dimension];

        objectList = new List<RoomObject>();

        //roomDoorList = new List<Door>();
    }

}
