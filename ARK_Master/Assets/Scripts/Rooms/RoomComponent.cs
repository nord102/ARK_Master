using UnityEngine;
using System.Collections;

public class RoomComponent
{
    #region Variables
    private int dimension = 7;
    #endregion

    #region Attributes
    //Component ID
    public int componentID { get; private set; }

    //Room ID that the Component belongs to
    public int roomID { get; set; }

    //Global Position -- X,Y Coordinates
    public int posX { get; set; }
    public int posY { get; set; }
    #endregion

    #region Constructors
    //Default Constructor
    public RoomComponent()
    {

    }

    //Constructor
    public RoomComponent(int newComponentID)
    {
        componentID = newComponentID;
    }
    #endregion
}
