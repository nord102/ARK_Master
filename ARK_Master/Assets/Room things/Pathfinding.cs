using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class Pathfinding {

    public static int[,] DeterminePaths(Room room)
    {
        int[,] alteredTileArray = room.roomLayout;

        //Set up a list of all the doors in the room
        List<Connections> connectionList = new List<Connections>();
        foreach (Door d in room.GetDoorList())
        {
            Connections c = new Connections(d);
            connectionList.Add(c);
        }

        //Now go through each door, and randomly assign it 2 connections (update the connected one as well)
        foreach (Connections c in connectionList)
        {
            List<int> triedDoors = new List<int>();
            int failSafe = 0;
            bool AllowOnlyOneConnection = false;
            while ((c.connectedDoors.Count < 2 && failSafe < 50) && !AllowOnlyOneConnection)
            {
                failSafe += 1;
                //Keep altering the random number until we find a door without 2 connections, or we run out of doors
                int nextRoomID = Random.Range(0, connectionList.Count);
                bool found = false;
                int failSafe2 = 0;
                while (!found && failSafe2 < 50)
                {
                    failSafe2 += 1;
                    if (triedDoors.Contains(nextRoomID) && triedDoors.Count != room.GetDoorList().Count)
                    {
                        nextRoomID = nextRoomID + 1;
                        if (nextRoomID > connectionList.Count - 1)
                        {
                            nextRoomID = 0;
                        }
                        triedDoors.Add(nextRoomID);
                    }
                    else
                    {
                        if (connectionList[nextRoomID].connectedDoors.Count < 2 || triedDoors.Count == room.GetDoorList().Count)
                        {
                            //Either this is ok, or we ran out of doors, or we have this connection already, either way add the connection
                            found = true;
                            if (!c.connectedDoors.Contains(nextRoomID) && nextRoomID != c.door.doorID)
                            {
                                c.connectedDoors.Add(nextRoomID);
                                connectionList[nextRoomID].connectedDoors.Add(c.door.doorID);
                                triedDoors.Add(nextRoomID);
                            }
                            else
                            {
                                //We've already added this room, skip. Maybe this room only gets 1 connection then, oh well
                                AllowOnlyOneConnection = true;
                            }

                        }
                        else
                        {
                            //We hadn't tried that door yet, but it already has 2 connections, so no good
                            try
                            {
                                triedDoors.Add(nextRoomID);
                            }
                            catch
                            {
                                Debug.Log("WE HAVE HAD AN ERROR WITH ADDING A TRIED DOOR?");
                            }
                        }
                    }
                } //End while (!found)
            } //End while (c.connectedDoors.Count < 2)
        } //End foreach

        //Now go through each door and it's connections and find a path through the array, avoiding the '-1's
        foreach (Connections c in connectionList)
        {
            //Change this to roomX, roomY when merged
            foreach (int doorID in c.connectedDoors)
            {
                //Positive X means going left, negative is right     Positive Y means going down, negative is up
                int xDistance = c.door.posX - room.GetDoorList()[doorID].posX;
                int yDistance = c.door.posY - room.GetDoorList()[doorID].posY;
                if (xDistance == 0 && yDistance == 0)
                {
                    break;
                }
                int xMultiplier = 1;
                int yMultiplier = 1;

                Debug.Log("DISTANCE: " + xDistance + ", " + yDistance);

                if (xDistance > 0)
                {
                    xMultiplier = -1;
                }
                if (yDistance > 0)
                {
                    yMultiplier = -1;
                }

                //Randomly move towards connected door position, avoiding -1 tiles
                bool connected = false;
                int currentPositionX = c.door.posX - room.posX;
                int currentPositionY = c.door.posY - room.posY;
                int failSafe = 0;
                Debug.Log("CurrentPosition: " + currentPositionX + ", " + currentPositionY);
                Debug.Log("FinalPosition: " + (room.GetDoorList()[doorID].posX - room.posX) + ", " + (room.GetDoorList()[doorID].posY - room.posY));
                while (!connected && failSafe < 50)
                {
                    failSafe += 1;
                    //Debug.Log("Examined Tile: " + currentPositionX + ", " + currentPositionY);
                    if (Random.Range(0, 2) == 0)
                    {
                        //Try to move along the x axis
                        if (alteredTileArray[currentPositionX + (1 * xMultiplier), currentPositionY] == -1 || (c.door.posX - room.posX == currentPositionX + (1 * xMultiplier) && c.door.posY - room.posY == currentPositionY) || (currentPositionX == room.GetDoorList()[doorID].posX - room.posX) && (c.door.posX - room.posX != (currentPositionX + (1 * xMultiplier)) && c.door.posY - room.posY != currentPositionY))
                        {
                            if (currentPositionY == room.GetDoorList()[doorID].posY - room.posY)
                            {
                                //We couldn't move on x, but we are already on the correct y - we have to move 1 into the room, and then correct it later.
                                //Move inwards - How do we determine inwards? if x/y = 0, inwards is 1, else inwards is -1
                                if (currentPositionY > 0)
                                {
                                    currentPositionY += 1 * -1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    yMultiplier = 1;
                                }
                                else
                                {
                                    currentPositionY += 1 * 1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    yMultiplier = -1;
                                }
                                Debug.Log("Stepped away from wall: yMultiplier = " + yMultiplier);
                            }
                            else
                            {
                                //Cant move on the x axis, so we must use the y one
                                currentPositionY += 1 * yMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                            }
                            Debug.Log("Moving Y Because X Blocked: "+ currentPositionX + ", " + currentPositionY);
                        }
                        else
                        {
                            if (currentPositionX == room.GetDoorList()[doorID].posX - room.posX)
                            {
                                if (currentPositionX > 0)
                                {
                                    currentPositionX += 1 * -1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    xMultiplier = 1;
                                }
                                else
                                {
                                    currentPositionX += 1 * 1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    xMultiplier = -1;
                                }
                                Debug.Log("Stepped away from wall: xMultiplier = " + xMultiplier);
                            }
                            else
                            {
                                //We could move on the x axis, so we do
                                currentPositionX += 1 * xMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                            }
                            Debug.Log("Moving X: " + currentPositionX + ", " + currentPositionY);
                        }
                    }
                    else
                    {
                        //Try to move along the y axis
                        //Debug.Log("X:" + currentPositionX + " Y:" + currentPositionY);
                        //Debug.Log("DoorPosY:" + room.roomDoorList[doorID].posY);
                        if (alteredTileArray[currentPositionX, currentPositionY + (1 * yMultiplier)] == -1 || (c.door.posY - room.posY == currentPositionY + (1 * yMultiplier) && c.door.posX - room.posX == currentPositionX) || (currentPositionY == room.GetDoorList()[doorID].posY - room.posY) && (c.door.posY - room.posY != (currentPositionY + (1 * yMultiplier)) && c.door.posX - room.posX != currentPositionX))
                        {
                            if (currentPositionX == room.GetDoorList()[doorID].posX - room.posX)
                            {
                                //We couldn't move on y, but we are already on the correct x - we have to move 1 into the room, and then correct it later.
                                //Move inwards - How do we determine inwards? if x/y = 0, inwards is 1, else inwards is -1
                                if (currentPositionX > 0)
                                {
                                    currentPositionX += 1 * -1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    xMultiplier = 1;
                                }
                                else
                                {
                                    currentPositionX += 1 * 1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    xMultiplier = -1;
                                }
                                Debug.Log("Stepped away from wall: xMultiplier = " + xMultiplier);
                            }
                            else
                            {
                                //Cant move on the y axis, so we must use the x one
                                currentPositionX += 1 * xMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                            }

                            Debug.Log("Moving X Because Y Blocked: " + currentPositionX + ", " + currentPositionY);
                        }
                        else
                        {
                            if (currentPositionY == room.GetDoorList()[doorID].posY - room.posY)
                            {
                                //We couldn't move on x, but we are already on the correct y - we have to move 1 into the room, and then correct it later.
                                //Move inwards - How do we determine inwards? if x/y = 0, inwards is 1, else inwards is -1
                                if (currentPositionY > 0)
                                {
                                    currentPositionY += 1 * -1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    yMultiplier = 1;
                                }
                                else
                                {
                                    currentPositionY += 1 * 1;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    yMultiplier = -1;
                                }
                                Debug.Log("Stepped away from wall: yMultiplier = " + yMultiplier);
                            }
                            else
                            {
                                //We could move on the y axis, so we do
                                currentPositionY += 1 * yMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                            }
                            Debug.Log("Moving Y: " + currentPositionX + ", " + currentPositionY);
                        }
                    }
                    //Have we finished the connection? flag the while loop to end
                    if (currentPositionX == room.GetDoorList()[doorID].posX - room.posX && currentPositionY == room.GetDoorList()[doorID].posY - room.posY)
                    {
                        connected = true;
                    }
                }
            }
        }

        //newRoom.roomLayout = Pathfinding.DeterminePaths(newRoom);

        //int[,] alteredTileArray = newRoom.roomLayout;
        Debug.Log(alteredTileArray.Length);
        string t = "";
        for (int i = 0; i < (int)Mathf.Sqrt(alteredTileArray.Length) ; ++i)
        {
            for (int j = 0; j < (int)Mathf.Sqrt(alteredTileArray.Length); ++j)
            {
                t += alteredTileArray[j, i] + " ";
            }
            t += "\n";
        }
        Debug.Log(t);

        return alteredTileArray;
    }
}

public class Connections
{
    public Door door = new Door();
    public List<int> connectedDoors = new List<int>();

    public Connections(Door d)
    {
        door = d;
    }

}
