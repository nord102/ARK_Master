using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Room
{
    /// <To-Do>
    /// Boundary Room Checking
    /// Highlight Room Based on Boundaries
    /// Fix Images?
    /// </To-Do>>

    //Room ID
    public int roomID { get; private set; }

    //X,Y coordinates
    public int posX { get; set; }
    public int posY { get; set; }
      
    /// <Room Layout Array Definitions>
    /// -2 = Doors
    /// -1 = Walls, Dead Zones
    ///  0 = Empty Space
    /// </Room Layout Array Definitions>
    public int[,] roomLayout;
       
    /// <Room Shapes/#Components>
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
    /// </Room Types/#Components>
    public int roomShape { get; set; }

    //Room GameObject
    public GameObject roomGameObject { get; set; }

    //Dragging State (used for placing rooms)
    public bool draggingState { get; set; }

    /// <Room Types>
    /// Medic Bay
    /// Storage Bay
    /// Engine Room?
    /// </summary>
    public int roomType { get; set; }

    //Room Event
    public Events roomEvent { get; set; }

    //Room Dimension-Related variables
    public int dimension = 7;
    public int maxDimension = 19;

    //Room State?
    //NEED TO DEFINE THIS
    public string roomState { get; set; }

    public List<RoomComponent> componentList { get; set; }
    public List<RoomObject> objectList { get; set; }   
    public List<Door> roomDoorList { get; set; }

    //Base Constructor
    public Room()
    {

    }

    //Constructor
    public Room(int newRoomID, int newRoomShape, int newRoomType, GameObject newRoomGameObject, string newRoomState, int newPosX, int newPosY)
    {
        roomID = newRoomID;
        roomState = newRoomState;
        roomShape = newRoomShape;
        roomType = newRoomType;

        posX = newPosX;
        posY = newPosY;

        roomGameObject = newRoomGameObject;

        componentList = new List<RoomComponent>();
        AssignComponents();
        
        objectList = new List<RoomObject>();
        roomDoorList = new List<Door>();
    }



    private void AssignComponents()
    {
        int numComponents = 0;

        if (roomShape == 1) //1
        {
            numComponents = 1;
            roomLayout = new int[dimension, dimension];
        }
        else if (roomShape == 2 || roomShape == 3) //2
        {
            numComponents = 2;
            roomLayout = new int[(2 * dimension) - 1, (2 * dimension) - 1];
        }
        else if (roomShape == 4 || roomShape == 5) //3
        {
            numComponents = 3;
            roomLayout = new int[(3 * dimension) - 2, (3 * dimension) - 2];
        }
        else if (roomShape == 6 || roomShape == 7 || roomShape == 8 || roomShape == 9) //3
        {
            numComponents = 3;
            roomLayout = new int[(2 * dimension) - 1, (2 * dimension) - 1];
        }
        else if (roomShape == 10) //4
        {
            numComponents = 4;
            roomLayout = new int[(2 * dimension) - 1, (2 * dimension) - 1];
        }
        
        //Assign Room Components and add to the component list
        for (int i = 0; i < numComponents; i++)
        {
            RoomComponent component = new RoomComponent(i);
            component.roomID = roomID;
            componentList.Add(component);
        }

        switch (roomShape)
        {
            #region 1x1
            //Type: 1x1 
            //# of Component(s): 1              
            case 1:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = 0;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                        }
                    }
                }               

                break;
            #endregion 
            #region 1x2
            //Type: 1x2 
            //# of Component(s): 2
            case 2:
                componentList[0].posX = posX;
                componentList[0].posY = posY;             

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension - 1;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + (dimension - 1)] = -1;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = 1;
                            roomLayout[i, j + (dimension - 1)] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = 1;
                            roomLayout[i, j + (dimension - 1)] = 0;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + (dimension - 1)] = -1;                            
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + (dimension - 1)] = 0;
                        }
                    }
                }               

                break;
            #endregion
            #region 2x1
            //Type: 2x1 
            //# of Component(s): 2
            case 3:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX + dimension - 1;
                componentList[1].posY = posY;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i + dimension - 1, j] = 0;
                        }
                    }
                }
                break;
            #endregion
            #region 1x3
            //Type: 1x3 
            //# of Component(s): 3
            case 4:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension - 1;

                componentList[2].posX = posX ;
                componentList[2].posY = posY + (2 * (dimension - 1));

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i, j + (2 * (dimension - 1))] = -1;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i, j + (2 * (dimension - 1))] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i, j + (2 * (dimension - 1))] = 0;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i, j + (2 * (dimension - 1))] = -1;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i, j + (2 * (dimension - 1))] = 0;
                        }
                    }
                }

                break;
            #endregion
            #region 3x1
            //Type: 3x1 
            //# of Component(s): 3
            case 5:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX + dimension - 1;
                componentList[1].posY = posY;

                componentList[2].posX = posX + (2 * (dimension - 1));
                componentList[2].posY = posY;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + (2 * (dimension - 1)), j] = 0;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + (2 * (dimension - 1)), j] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                            roomLayout[i + (2 * (dimension - 1)), j] = -1;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                            roomLayout[i + (2 * (dimension - 1)), j] = -1;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + (2 * (dimension - 1)), j] = 0;
                        }
                    }
                }
                break;
            #endregion
            #region L-Shape (Normal)
            //Type: L-shape (Normal) 
            //# of Component(s): 3
            case 6:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension - 1;

                componentList[2].posX = posX + dimension - 1;
                componentList[2].posY = posY;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = 0;
                        }
                    }
                }

                break;
            #endregion
            #region L-shape (Horizontal Flip)
            //Type: L-shape (Horizontal Flip) 
            //# of Component(s): 3
            case 7:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension - 1;

                componentList[2].posX = posX + dimension - 1;
                componentList[2].posY = posY + dimension - 1;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                        }
                    }
                }

                break;
            #endregion
            #region L-Shape (Vertical Flip)
            //Type: L-shape (Vertical Flip) 
            //# of Component(s): 3
            case 8:
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX + dimension - 1;
                componentList[1].posY = posY;

                componentList[2].posX = posX + dimension - 1;
                componentList[2].posY = posY + dimension - 1;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i + dimension - 1, j] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                        }
                    }
                }

                break;
            #endregion
            #region L-shape (Horizontal - Vertical Flip)
            //Type: L-shape (Horizontal - Vertical Flip) 
            //# of Component(s): 3
            case 9:
                componentList[0].posX = posX;
                componentList[0].posY = posY + dimension - 1;

                componentList[1].posX = posX + dimension - 1;
                componentList[1].posY = posY + dimension - 1;

                componentList[2].posX = posX + dimension - 1;
                componentList[2].posY = posY;

                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = -1;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = 0;
                        }
                    }
                }

                break;
            #endregion
            #region 2x2
            //Type: 2x2 
            //# of Component(s): 4
            case 10:                
                componentList[0].posX = posX;
                componentList[0].posY = posY;

                componentList[1].posX = posX;
                componentList[1].posY = posY + dimension - 1;

                componentList[2].posX = posX + dimension- 1;
                componentList[2].posY = posY;

                componentList[3].posX = posX + dimension - 1;
                componentList[3].posY = posY + dimension - 1;
                                
                for (int i = 0; i < dimension; i++)
                {
                    for (int j = 0; j < dimension; j++)
                    {
                        if (i == 0) //Make left walls (ROWS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                        }
                        else if (i == (dimension - 1)) //Make right walls (ROWS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;

                        }
                        else if (j == 0) //Make bottom walls (COLS)
                        {
                            roomLayout[i, j] = -1;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = -1;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                        }
                        else if (i == (dimension - 1)) //Make top walls (COLS)
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = -1;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = -1;
                        }
                        else //Inside is blank
                        {
                            roomLayout[i, j] = 0;
                            roomLayout[i, j + dimension - 1] = 0;
                            roomLayout[i + dimension - 1, j] = 0;
                            roomLayout[i + dimension - 1, j + dimension - 1] = 0;
                        }
                    }
                }
                break;
            #endregion
        }
    }
}
