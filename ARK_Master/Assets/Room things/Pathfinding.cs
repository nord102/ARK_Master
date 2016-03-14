using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class Pathfinding {

    public static int[,] DeterminePaths(Room room)
    {
        int[,] alteredTileArray = new int[19, 19];
        for (int i = 0; i < alteredTileArray.Length; ++i)
        {
            for (int j = 0; j < alteredTileArray.Length; ++j)
            {
                alteredTileArray[i,j] = -1;
            }

        }
        //int[,] alteredTileArray = room.roomLayout;

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
            while (c.connectedDoors.Count < 2)
            {
                //Keep altering the random number until we find a door without 2 connections, or we run out of doors
                int nextRoomID = Random.Range(0, connectionList.Count);
                bool found = false;
                while (!found)
                {
                    if (triedDoors.Contains(nextRoomID) && triedDoors.Count != triedDoors.Count)
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
                        if (connectionList[nextRoomID].connectedDoors.Count < 2 || triedDoors.Count != triedDoors.Count)
                        {
                            //Either this is ok, or we ran out of doors, either way add the connection
                            found = true;
                            c.connectedDoors.Add(nextRoomID);
                            connectionList[nextRoomID].connectedDoors.Add(c.door.doorID);
                        }
                        else
                        {
                            //We hadn't tried that door yet, but it already has 2 connections, so no good
                            triedDoors.Add(nextRoomID);
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
                int xMultiplier = 0;
                int yMultiplier = 0;

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
                while (!connected)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        //Try to move along the x axis
                        if (alteredTileArray[currentPositionX, currentPositionY] == -1 || currentPositionX == room.roomDoorList[doorID].posX)
                        {
                            //Cant move on the x axis, so we must use the y one
                            currentPositionY += 1 * yMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
                        }
                        else
                        {
                            //We could move on the x axis, so we do
                            currentPositionX += 1 * xMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
                        }
                    }
                    else
                    {
                        //Try to move along the y axis
                        if (alteredTileArray[currentPositionX, currentPositionY] == -1 || currentPositionY == room.roomDoorList[doorID].posY)
                        {
                            //Cant move on the y axis, so we must use the x one
                            currentPositionX += 1 * xMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
                        }
                        else
                        {
                            //We could move on the y axis, so we do
                            currentPositionY += 1 * yMultiplier;
                            alteredTileArray[currentPositionX, currentPositionY] = 1;
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
