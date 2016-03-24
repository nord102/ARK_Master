using UnityEngine;
using System.Collections;

public class RoomComponent
{
    public int componentID { get; private set; }
    public int roomID { get; set; }

   
    private int dimension = 7;

    public int posX { get; set; }
    public int posY { get; set; }

    public RoomComponent()
    {

    }

    public RoomComponent(int newComponentID)
    {
        componentID = newComponentID;
    }
}
