using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room
{
    public int roomID { get; private set; }

    public int roomType { get; set; }
    public List<RoomComponent> componentList { get; set; }
    private int dimension = 7;





    public string roomState { get; set; }

    public bool draggingState { get; set; }
    

    public List<RoomObject> objectList { get; set; }   
    public List<Door> roomDoorList { get; set; }

    public int posX { get; set; }
    public int posY { get; set; }
   
    /// <summary>
    /// Room Types/#Components
    /// 
    /// 1 - 1x1 - 1
    /// 2 - 1x2 - 2
    /// 3 - 2x1 - 2
    /// 4 - 1x3 - 3
    /// 5 - 3x1 - 3
    /// 6 - L-shape - 3
    /// 7 - L-shape (Horizontal Flip) - 3
    /// 8 - L-shape (Vertical Flip) - 3
    /// 9 - L-shape (Horizontal - Vertical Flip) - 3
    /// 10 - 2x2
    /// </summary>

    public Room()
    {

    }

    public Room(int newRoomID, int newRoomType, string newRoomState, int newPosX, int newPosY)
    {
        roomID = newRoomID;
        roomState = newRoomState;
        roomType = newRoomType;

        posX = newPosX;
        posY = newPosY;

        componentList = new List<RoomComponent>();

        AssignComponents();

        

        objectList = new List<RoomObject>();

        roomDoorList = new List<Door>();
    }



    private void AssignComponents()
    {
        int numComponents = 0;

        if (roomType == 1) //1
        {
            numComponents = 1;
        }
        else if (roomType == 2 || roomType == 3) //2
        {
            numComponents = 2;
        }
        else if (roomType == 4 || roomType == 5 || roomType == 6 || roomType == 7 || roomType == 8 || roomType == 9) //3
        {
            numComponents = 3;
        }
        else if (roomType == 10) //4
        {
            numComponents = 4;
        }
        
        for (int i = 0; i < numComponents; i++)
        {
            RoomComponent component = new RoomComponent(i);
            componentList.Add(component);
        }

        switch (roomType)
        {
            //Type: 1x1 
            //# of Component(s): 1              
            case 1:
                componentList[0].posX = posX;
                componentList[0].posY = posY;
                break;
            //Type: 1x2 
            //# of Component(s): 2
            case 2:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension;

                break;
            //Type: 2x1 
            //# of Component(s): 2
            case 3:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX + dimension;
                componentList[1].posY = posY;
                break;
            //Type: 1x3 
            //# of Component(s): 3
            case 4:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension;

                componentList[2].posX = posX ;
                componentList[2].posY = posY + (2*dimension);
                break;
            //Type: 3x1 
            //# of Component(s): 3
            case 5:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX + dimension;
                componentList[1].posY = posY;

                componentList[2].posX = posX + (2 * dimension);
                componentList[2].posY = posY;
                break;


            //Type: L-shape (Normal) 
            //# of Component(s): 3
            case 6:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension;

                componentList[2].posX = posX + dimension;
                componentList[2].posY = posY;
                break;
            //Type: L-shape (Horizontal Flip) 
            //# of Component(s): 3
            case 7:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension;

                componentList[2].posX = posX + dimension;
                componentList[2].posY = posY + dimension;
                break;
            //Type: L-shape (Vertical Flip) 
            //# of Component(s): 3
            case 8:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX + dimension;
                componentList[1].posY = posY;

                componentList[2].posX = posX + dimension;
                componentList[2].posY = posY + dimension;
                break;
            //Type: L-shape (Horizontal - Vertical Flip) 
            //# of Component(s): 3
            case 9:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX + dimension;
                componentList[1].posY = posY;

                componentList[2].posX = posX + dimension;
                componentList[2].posY = posY - dimension;
                break;
            //Type: 2x2 
            //# of Component(s): 4
            case 10:                
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension;

                componentList[2].posX = posX + dimension;
                componentList[2].posY = posY;

                componentList[3].posX = posX + dimension;
                componentList[3].posY = posY + dimension;
                break;
        }
    }
}
