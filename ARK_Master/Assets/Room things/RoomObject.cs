using UnityEngine;
using System.Collections;

public class RoomObject
{
    #region Attributes
    //Object ID
    public int objectID { get; private set; }

    //Object Type?
    public string objectType { get; set; }

    //Object State?
    public bool objectState { get; set; }

    //Global Position -- X,Y Coordinates
    public int posX { get; set; }
    public int posY { get; set; }
    #endregion

    #region Constructors
    //Default Constructor
    public RoomObject()
    {

    }

    //Constructor
    public RoomObject(int newObjectID, string newObjectType, bool newObjectState, int newPosX, int newPosY)
    {
        objectID = newObjectID;
        objectType = newObjectType;
        objectState = newObjectState;
        posX = newPosX;
        posY = newPosY;
    }
    #endregion
}
