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
                                //Debug.Log("WE HAVE HAD AN ERROR WITH ADDING A TRIED DOOR?");
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
                //Randomly move towards connected door position, avoiding -1 tiles
                bool connected = false;
                bool forceX = false;
                bool forceY = false;
                int currentPositionX = c.door.posX - room.posX;
                int currentPositionY = c.door.posY - room.posY;

                MoveOneSpaceIntoTheRoom(ref currentPositionX, ref currentPositionY, ref alteredTileArray);

                //Positive X means going left, negative is right     Positive Y means going down, negative is up
                int xDistance = currentPositionX - (room.GetDoorList()[doorID].posX - room.posX);
                int yDistance = currentPositionY - (room.GetDoorList()[doorID].posY - room.posY);
                if (xDistance == 0 && yDistance == 0)
                {
                    break;
                }
                int xMultiplier = 0;
                int yMultiplier = 0;

                //Debug.Log("DISTANCE: " + xDistance + ", " + yDistance);

                if (xDistance > 0)
                {
                    xMultiplier = -1;
                }
                else { xMultiplier = 1; }

                if (yDistance > 0)
                {
                    yMultiplier = -1;
                }
                else { yMultiplier = 1; }

                int failSafe = 0;
                //Debug.Log("CurrentPosition: " + currentPositionX + ", " + currentPositionY);
                //Debug.Log("FinalPosition: " + (room.GetDoorList()[doorID].posX - room.posX) + ", " + (room.GetDoorList()[doorID].posY - room.posY));
                while (!connected && failSafe < 50)
                {
                    failSafe += 1;
                    //Debug.Log("xMultiplier: " + xMultiplier + ", yMultiplier: " + yMultiplier);

                    //We can save a lot of time by checking whether the next tile is a 1 - that means its already a path, and we have just connected to it, this door will be fine
                    if (alteredTileArray[currentPositionX + (1 * xMultiplier), currentPositionY] == 1 || alteredTileArray[currentPositionX, currentPositionY + (1 * yMultiplier)] == 1)
                    {
                        connected = true;
                        continue;
                    }

                    if (Random.Range(0, 2) == 0 || forceX && !forceY) //Pulled X
                    {
                        //Try to move along the x axis - Things that fail this condition: (Next tile was a wall) or (Next tile is the door I just stepped away from) or (Next tile is a door, but not the correct one)
                        if (alteredTileArray[currentPositionX + (1 * xMultiplier), currentPositionY] == -1 || (alteredTileArray[currentPositionX + (1 * xMultiplier), currentPositionY] == -2  && (currentPositionX + (1 * xMultiplier) != room.GetDoorList()[doorID].posX - room.posX || currentPositionY != room.GetDoorList()[doorID].posY - room.posY)))// || (c.door.posX - room.posX == currentPositionX + (1 * xMultiplier) && c.door.posY - room.posY == currentPositionY))//|| (currentPositionX == room.GetDoorList()[doorID].posX - room.posX) && (c.door.posX - room.posX != (currentPositionX + (1 * xMultiplier)) && c.door.posY - room.posY != currentPositionY))
                        {
                            if (currentPositionY == room.GetDoorList()[doorID].posY - room.posY)
                            {
                                //correct y, but x is -1. Damn you L Shaped rooms. Move BACK 1 y, force x until x == door position
                                forceX = true;
                                currentPositionY += 1 * (-yMultiplier);
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                                //Debug.Log("Stepping back Y because X is a wall: " + currentPositionX + ", " + currentPositionY);
                            }
                            else
                            {
                                //Cant move on the x axis, so we must use the y one
                                currentPositionY += 1 * yMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                                //Debug.Log("Moving Y Because X Blocked: " + currentPositionX + ", " + currentPositionY);
                            }
                        }
                        else
                        {                       
                            if (currentPositionX == room.GetDoorList()[doorID].posX - room.posX)
                            {
                                //We could move x, but we are already on the correct x - so move y unless y is -1
                                if (alteredTileArray[currentPositionX, currentPositionY + (1 * yMultiplier)] == -1)
                                {
                                    //correct x, but y is -1. Damn you L Shaped rooms. Move BACK 1 x, force y until y == door position
                                    forceY = true;
                                    currentPositionX += 1 * (-xMultiplier);
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    //Debug.Log("Stepping back X because Y is a wall: " + currentPositionX + ", " + currentPositionY);
                                }
                                else
                                {
                                    currentPositionY += 1 * yMultiplier;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    //Debug.Log("Moving Y Because Already on Correct X: " + currentPositionX + ", " + currentPositionY);
                                }
                            }
                            else
                            {
                                //We could move on the x axis, so we do - Unless the y matches up but is NOT the door
                                currentPositionX += 1 * xMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                                //Debug.Log("Moving X: " + currentPositionX + ", " + currentPositionY);
                            }
                            
                        }
                    }
                    else //Pulled Y
                    {
                        //Try to move along the y axis
                        if (alteredTileArray[currentPositionX, currentPositionY + (1 * yMultiplier)] == -1 || (alteredTileArray[currentPositionX, currentPositionY + (1 * yMultiplier)] == -2 && (currentPositionX != room.GetDoorList()[doorID].posX - room.posX || currentPositionY + (1 * yMultiplier) != room.GetDoorList()[doorID].posY - room.posY)))// (c.door.posY - room.posY == currentPositionY + (1 * yMultiplier) && c.door.posX - room.posX == currentPositionX) )// || (currentPositionY == room.GetDoorList()[doorID].posY - room.posY) && (c.door.posY - room.posY != (currentPositionY + (1 * yMultiplier)) && c.door.posX - room.posX != currentPositionX))
                        {
                            if (currentPositionX == room.GetDoorList()[doorID].posX - room.posX)
                            {
                                //correct x, but y is -1. Damn you L Shaped rooms. Move BACK 1 x, force y until y == door position
                                forceY = true;
                                currentPositionX += 1 * (-xMultiplier);
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                                //Debug.Log("Stepping back X because Y is a wall: " + currentPositionX + ", " + currentPositionY);
                            }
                            else
                            {
                                //Cant move on the y axis, so we must use the x one
                                currentPositionX += 1 * xMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                                //Debug.Log("Moving X Because Y Blocked: " + currentPositionX + ", " + currentPositionY);
                            }
                        }
                        else
                        {
                            if (currentPositionY == room.GetDoorList()[doorID].posY - room.posY)
                            {
                                //We could move x, but we are already on the correct x - so move y unless y is -1
                                if (alteredTileArray[currentPositionX + (1 * xMultiplier), currentPositionY] == -1)
                                {
                                    //correct y, but x is -1. Damn you L Shaped rooms. Move BACK 1 y, force x until x == door position
                                    forceX = true;
                                    currentPositionY += 1 * (-yMultiplier);
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    //Debug.Log("Stepping back Y because X is a wall: " + currentPositionX + ", " + currentPositionY);
                                }
                                else
                                {
                                    //We could move y, but we are already on the correct y - so move x
                                    currentPositionX += 1 * xMultiplier;
                                    alteredTileArray[currentPositionX, currentPositionY] = 1;
                                    //Debug.Log("Moving X Because Already on Correct Y: " + currentPositionX + ", " + currentPositionY);
                                }
                            }
                            else
                            {
                                //We could move on the y axis, so we do
                                currentPositionY += 1 * yMultiplier;
                                alteredTileArray[currentPositionX, currentPositionY] = 1;
                                //Debug.Log("Moving Y: " + currentPositionX + ", " + currentPositionY);
                            }
                            
                        }
                    }
                    //Have we finished the connection? flag the while loop to end
                    if (currentPositionX == room.GetDoorList()[doorID].posX - room.posX && currentPositionY == room.GetDoorList()[doorID].posY - room.posY)
                    {
                        //We just turned this door to a 1, we should put it back
                        alteredTileArray[currentPositionX, currentPositionY] = -2;
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
        //Debug.Log(t);

        return alteredTileArray;
    }

    private static void MoveOneSpaceIntoTheRoom(ref int currentPositionX, ref int currentPositionY, ref int[,] alteredTileArray)
    {
        //We should be able to determine which direction is a step into the room by looking at 2 adjacent spots - below and left
        try //Check below
        {
            if (alteredTileArray[currentPositionX, currentPositionY - 1] > -1) //0 means it's open, 1 means it's a path and that's alright too
            {
                //We can stop here, down is the step inward
                currentPositionY = currentPositionY - 1;
                alteredTileArray[currentPositionX, currentPositionY] = 1;
                return;
            }
        }
        catch
        {
            //Failing to go down means we are at the bottom, a step into the room has to be up
            currentPositionY = currentPositionY + 1;
            alteredTileArray[currentPositionX, currentPositionY] = 1;
            return;
        }

        try //Check left
        {
            if (alteredTileArray[currentPositionX - 1, currentPositionY] > -1) //0 means it's open, 1 means it's a path and that's alright too
            {
                //We can stop here, left is the step inward
                currentPositionX = currentPositionX - 1;
                alteredTileArray[currentPositionX, currentPositionY] = 1;
                return;
            }
        }
        catch
        {
            //Failing to go left means we are at the left edge, a step into the room has to be right
            currentPositionX = currentPositionX + 1;
            alteredTileArray[currentPositionX, currentPositionY] = 1;
            return;
        }

        //Almost 100% confident one of those previous returns has to fire - WE SHOULD NOT BE ABLE TO GET HERE IF YOU DID YOU'VE KILLED US ALL
        return;
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
