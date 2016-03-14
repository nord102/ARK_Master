using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    //Add RoomPosX and RoomPosY
    //for global room X,Y
    //Keep ComPosX and ComPosY 
    //for now, unless you can refactor

    public int doorID { get; private set; }

    public GameObject doorGameObject { get; set; }

    public int doorGameObjectRoomID { get; set; }

    private Door script;

    public int roomID_1 { get; set; } //Recent
    public int roomID_2 { get; set; } //Global

    public int posX { get; set; }
    public int posY { get; set; }

    public bool doorstate { get; set; }

    public Door()
    {

    }

    public Door(int newDoorID, int newRoomID_1, int newRoomID_2, int newPosX, int newPosY, bool newDoorState)
    {
        doorID = newDoorID;
        roomID_1 = newRoomID_1; 
        roomID_2 = newRoomID_2;
        posX = newPosX;
        posY = newPosY;
        doorstate = newDoorState;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StateMachine.instance.FireEvent(Generate.instance.roomList[doorGameObjectRoomID].roomEvent);
        }
    }

    public void RoomIDAssign(int doorRoomID)
    {
        this.doorGameObjectRoomID = doorRoomID;

    }

}
