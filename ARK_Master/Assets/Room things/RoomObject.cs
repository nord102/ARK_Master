using UnityEngine;
using System.Collections;

public class RoomObject 
{
    public int objectID { get; private set; }

    public string objectType { get; set; }

    public bool objectState { get; set; }

    public int posX { get; set; }
    public int posY { get; set; }


    public RoomObject()
    {

    }

    public RoomObject(int newObjectID, string newObjectType, bool newObjectState, int newPosX, int newPosY)
    {
        objectID = newObjectID;
        objectType = newObjectType;
        objectState = newObjectState;
        posX = newPosX;
        posY = newPosY;
    }

}
