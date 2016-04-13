using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomAvailability : MonoBehaviour 
{
    private int numberOfRooms = 10;

    public List<int> roomList { get; private set; }


    //How you want me to do this?
    //Cue Jeopardy Music
    //Do
    //Do
    //Do
    //Do
    //Do
    //Do
    //Do
    

    /// <Room Availability>
    /// Determines whether or not the room
    /// is available in a specific room type
    /// 
    /// {(Storage),(Medical),(Engineering),(Labratory)}
    /// </summary>
    public int[][] roomTypeAvailability { get; set; }

    /// <summary>
    /// 1 - 1,0,0,0
    /// 2 - 
    /// 3 -
    /// 4 -
    /// 5 -
    /// 6 -
    /// </summary>


    public RoomAvailability()
    {
        //int roomID = 1;



        for (int i = 0; i < numberOfRooms; i++)
        {
            



        }

    }
}
