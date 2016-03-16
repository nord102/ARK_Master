using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class Pathfinding {

    public static int[,] DeterminePaths(Room room)
    {
        int[,] alteredTileArray = room.roomLayout;

        //Set up a list of all the doors in the room
        List<Connections> connectionList = new List<Connections>();
        foreach (Door d in room.roomDoorList)
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
                    if (triedDoors.Contains(nextRoomID) && triedDoors.Count != room.roomDoorList.Count)
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
                        if (connectionList[nextRoomID].connectedDoors.Count < 2  || triedDoors.Count == room.roomDoorList.Count)
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
                int xDistance = c.door.posX - room.roomDoorList[doorID].posX;
                int yDistance = c.door.posY - room.roomDoorList[doorID].posY;
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
                int currentPositionX = c.door.posX;
                int currentPositionY = c.door.posY;
                int failSafe = 0;
                Debug.Log("CurrentPosition: " + currentPositionX + ", " + currentPositionY);
                Debug.Log("FinalPosition: " + room.roomDoorList[doorID].posX + ", " + room.roomDoorList[doorID].posY);
                while (!connected && failSafe < 50)
                {
                    failSafe += 1;
                    Debug.Log("Examined Tile: " + currentPositionX + ", " + currentPositionY);
                    if (Random.Range(0, 2) == 0)
                    {
                        //Try to move along the x axis
                        if (alteredTileArray[currentPositionX, currentPositionY] == -1 || currentPositionX == room.roomDoorList[doorID].posX)
                        {
                            //Cant move on the x axis, so we must use the y one
                            currentPositionY += 1 * yMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
                            Debug.Log("Moving Y");
                        }
                        else
                        {
                            //We could move on the x axis, so we do
                            currentPositionX += 1 * xMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
                            Debug.Log("Moving X");
                        }
                    }
                    else
                    {
                        //Try to move along the y axis
                        //Debug.Log("X:" + currentPositionX + " Y:" + currentPositionY);
                        //Debug.Log("DoorPosY:" + room.roomDoorList[doorID].posY);
                        if (alteredTileArray[currentPositionX, currentPositionY] == -1 || currentPositionY == room.roomDoorList[doorID].posY)
                        {
                            //Cant move on the y axis, so we must use the x one
                            currentPositionX += 1 * xMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
                            Debug.Log("Moving X");
                        }
                        else
                        {
                            //We could move on the y axis, so we do
                            currentPositionY += 1 * yMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
                            Debug.Log("Moving Y");
                        }
                    }
                    //Have we finished the connection? flag the while loop to end
                    if (currentPositionX == room.roomDoorList[doorID].posX && currentPositionY == room.roomDoorList[doorID].posY)
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
        for (int i = 0; i < 7; ++i)
        {
            for (int j = 0; j < 7; ++j)
            {
                t += alteredTileArray[i, j] + " ";
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
