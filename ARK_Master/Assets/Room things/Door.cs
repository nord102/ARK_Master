using UnityEngine;
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
        if (col.gameObject.tag == "Player")// && Generate.instance.currentDoor == null)
        {
            //Set the door that is being collided with
            Generate.instance.currentDoor = gameObject.GetComponent<Door>();
            

            Events newRoomEvent = Generate.instance.GetRoomGameObjectList()[gameObject.GetComponent<Door>().roomID_1 - 1].GetComponent<Room>().roomEvent;
            StateMachine.instance.FireEvent(newRoomEvent);
        }
    }
}