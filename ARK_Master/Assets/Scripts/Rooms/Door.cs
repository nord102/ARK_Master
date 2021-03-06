﻿using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    #region Attributes
    //Door ID
    public int doorID { get; private set; }

    //Room ID's that the Door belongs to
    public int roomID_1 { get; set; } //Recent Room / Room Just Placed
    public int roomID_2 { get; set; } //Global Room / Room Already Placed.

    //Global X,Y Coordinates
    public int posX { get; set; }
    public int posY { get; set; }

    /// <Door States>
    ///2 - Being interacted with
    ///1 - Active
    ///0 - Inactive
    /// </Door State>
    public int doorstate { get; set; }
    #endregion

    public void SetDoorID(int newDoorID)
    {
        doorID = newDoorID;
    }

    #region Contructors / Initializer
    //Default Constructor
    public Door()
    {

    }

    //Constructor
    public Door(int newDoorID, int newRoomID_1, int newRoomID_2, int newPosX, int newPosY, int newDoorState)
    {
        Initialize(newDoorID, newRoomID_1, newRoomID_2, newPosX, newPosY, newDoorState);
    }

    //Initializer
    public void Initialize(int newDoorID, int newRoomID_1, int newRoomID_2, int newPosX, int newPosY, int newDoorState)
    {
        doorID = newDoorID;
        roomID_1 = newRoomID_1;
        roomID_2 = newRoomID_2;
        posX = newPosX;
        posY = newPosY;
        doorstate = newDoorState;
    }
    #endregion

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && Generate.instance.currentDoor == null)
        {


            //Check if collision is in specific spot
            //if yes, then go
            //if no, then dont do anything







            //Set the door that is being collided with
            Generate.instance.currentDoor = gameObject.GetComponent<Door>();

            Room doorRoom_1 = Generate.instance.GetRoomGameObjectList()[gameObject.GetComponent<Door>().roomID_1 - 1].GetComponent<Room>();
            Room doorRoom_2 = Generate.instance.GetRoomGameObjectList()[gameObject.GetComponent<Door>().roomID_2 - 1].GetComponent<Room>();
            Events newRoomEvent = null;

            if(doorRoom_1.roomState == 1)
            {
                Generate.instance.currentRoom = doorRoom_1;
                newRoomEvent = doorRoom_1.roomEvent;
            }
            else if (doorRoom_2.roomState == 1)
            {
                Generate.instance.currentRoom = doorRoom_2;
                newRoomEvent = doorRoom_2.roomEvent;
            }

            StateMachine.instance.FireEvent(newRoomEvent);
        }
    }
}