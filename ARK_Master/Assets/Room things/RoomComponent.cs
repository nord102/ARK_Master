using UnityEngine;
using System.Collections;

public class RoomComponent
{
    public int componentID { get; private set; }

    public int [,] layout {get;set;}
    private int dimension = 5;

    public int posX { get; set; }
    public int posY { get; set; }

    public RoomComponent()
    {

    }

    public RoomComponent(int newComponentID)
    {
        componentID = newComponentID;

        layout = new int[dimension, dimension];
    }
}
