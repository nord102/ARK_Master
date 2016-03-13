using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour 
{
    public static Generate instance = null;

    #region Door
    public GameObject door;
    private GameObject cloneDoor;
    #endregion

    #region Room GameObjects
    public GameObject startRoom; 
    public GameObject room_1x1;
    public GameObject room_1x2;
    public GameObject room_2x1;
    public GameObject room_1x3;
    public GameObject room_3x1;
    public GameObject room_LShape_Normal;
    public GameObject room_LShape_HFlip;
    public GameObject room_LShape_VFlip;
    public GameObject room_LShape_HVFlip;
    public GameObject room_2x2;

    private GameObject cloneStartRoom;
    private GameObject cloneRoom_1x1;
    private GameObject cloneRoom_1x2;
    private GameObject cloneRoom_2x1;
    private GameObject cloneRoom_1x3;
    private GameObject cloneRoom_3x1;
    private GameObject cloneRoom_LShape_Normal;
    private GameObject cloneRoom_LShape_HFlip;
    private GameObject cloneRoom_LShape_VFlip;
    private GameObject cloneRoom_LShape_HVFlip;
    private GameObject cloneRoom_2x2;
    #endregion

    #region Lists
    public List<Room> roomList = new List<Room>();
    public List<RoomComponent> roomComponentList = new List<RoomComponent>();
    #endregion

    //--
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    private GameObject cloneObject1;
    private GameObject cloneObject2;
    private GameObject cloneObject3;

  
    //--

    //Create Image for the first room
    //Create list of rooms and add first room to it

    void Start()
    {
        PlaceStartRoom();

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


    void PlaceStartRoom()
    {
        cloneStartRoom = Instantiate(startRoom, new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;

        Room newRoom = new Room(roomList.Count, 10,0, cloneStartRoom, "Explored", 0, 0);
        newRoom.draggingState = false;
        roomList.Add(newRoom);

        foreach (RoomComponent roomCom in newRoom.componentList)
        {
            roomComponentList.Add(roomCom);
        }
    }

    public void checkForDoors()
    {  
        bool doorMade = false;

        //get the room that was just placed 
        Room recentRoom = roomList[roomList.Count - 1];



        Door newDoor = new Door();

        //Check all of the recent Room Components for neighbours       
        foreach (RoomComponent recentRoomCom in recentRoom.componentList)
        {            
            foreach (RoomComponent globalRoomCom in roomComponentList)
            {
                //X+ Y (RIGHT)
                if (((recentRoomCom.posX + (recentRoom.dimension - 1) == globalRoomCom.posX) && (recentRoomCom.posY == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {                    
                    //Make Door (X+,Y+1/2)
                    Door tempDoor = new Door(recentRoom.roomDoorList.Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX + recentRoom.dimension-1), (recentRoomCom.posY + (recentRoom.dimension - 1) / 2), true);

                    newDoor = tempDoor;
                    doorMade = true; 
                }
                //X- Y (LEFT)
                else if (((recentRoomCom.posX - (recentRoom.dimension - 1) == globalRoomCom.posX) && (recentRoomCom.posY == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {
                    //Make Door (X,Y+1/2)
                    Door tempDoor = new Door(recentRoom.roomDoorList.Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX), (recentRoomCom.posY + (recentRoom.dimension - 1) / 2), true);

                    newDoor = tempDoor;
                    doorMade = true;                     
                }
                //X Y+ (UP)
                else if (((recentRoomCom.posX == globalRoomCom.posX) && (recentRoomCom.posY + (recentRoom.dimension - 1) == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {
                    //Make Door (X+1/2,Y+)
                    Door tempDoor = new Door(recentRoom.roomDoorList.Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX + (recentRoom.dimension - 1) / 2), (recentRoomCom.posY + (recentRoom.dimension - 1)), true);

                    newDoor = tempDoor;
                    doorMade = true;
                }
                //X Y- (DOWN)
                else if (((recentRoomCom.posX == globalRoomCom.posX) && (recentRoomCom.posY - (recentRoom.dimension - 1) == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {
                    //Make Door (X+1/2,Y)
                    Door tempDoor = new Door(recentRoom.roomDoorList.Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX + (recentRoom.dimension - 1) / 2), recentRoomCom.posY, true);

                    newDoor = tempDoor;
                    doorMade = true;
                }

                if (doorMade)
                {

                    doorMade = false;

                    //Add to Recent room door list
                    recentRoom.roomDoorList.Add(newDoor);

                    //Add to Connected room door list
                    Room connectingRoom = roomList[globalRoomCom.roomID];
                    connectingRoom.roomDoorList.Add(newDoor);
                    
                    //Create Door
                    //Take out the Wall on the Dragged Room
                    foreach (Transform child in recentRoom.roomGameObject.transform)
                    {
                        foreach (Transform smallerChild in child.transform)
                        {
                            //Destroy and replace smallerchild
                            if (smallerChild.transform.position.x == newDoor.posX && smallerChild.transform.position.y == newDoor.posY)
                            {
                                //Create Door GameObject 
                                cloneDoor = Instantiate(door, new Vector3(smallerChild.transform.position.x, smallerChild.transform.position.y, 0f), Quaternion.identity) as GameObject;
                                newDoor.doorGameObject = cloneDoor;

                                //Hide Wall where Door is going
                                smallerChild.gameObject.SetActive(false);
                            }
                        }
                    }

                    //Take out Wall on the Connected Room
                    foreach (Transform child in connectingRoom.roomGameObject.transform)
                    {
                        foreach (Transform smallerChild in child.transform)
                        {
                            //Destroy and replace smallerchild
                            if (smallerChild.transform.position.x == newDoor.posX && smallerChild.transform.position.y == newDoor.posY)
                            {
                                //Hide Wall where Door is going
                                smallerChild.gameObject.SetActive(false);
                            }
                        }
                    }

                    newDoor.doorGameObject.SendMessage("RoomIDAssign", recentRoom.roomID);

                    //Should Add the Door to the Component Layout?
                    //recentRoomCom.layout[newDoor.posX, newDoor.posY] = 2;
                }
            }
        }
    }


    void PopulateRoom(int roomID)
    {


    }
    
    //NEED TO ADD FUNCTIONALITY FOR DIFFERENT ROOMS (MAKE THIS MODULAR)    

    //Add Parameter for the RoomShape (make like an if statement for which room to clone)
    public GameObject GenRoom(int roomShape)
    {
        GameObject generatedRoom = new GameObject();

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 worldPos;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            worldPos = hit.point;
        }
        else
        {
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        }

        switch (roomShape)
        {
            case 1:
                cloneRoom_1x1 = Instantiate(room_1x1, new Vector3(worldPos.x, worldPos.y, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_1x1;
                break;
            case 2:
                cloneRoom_1x2 = Instantiate(room_1x2, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_1x2;
                break;
            case 3:
                cloneRoom_2x1 = Instantiate(room_2x1, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_2x1;
                break;
            case 4:
                cloneRoom_1x3 = Instantiate(room_1x3, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_1x3;
                break;
            case 5:
                cloneRoom_3x1 = Instantiate(room_3x1, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_3x1;
                break;            
            case 6:
                cloneRoom_LShape_Normal = Instantiate(room_LShape_Normal, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_LShape_Normal;
                break;
            case 7:
                cloneRoom_LShape_HFlip = Instantiate(room_LShape_HFlip, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_LShape_HFlip;
                break;
            case 8:
                cloneRoom_LShape_VFlip = Instantiate(room_LShape_VFlip, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_LShape_VFlip;
                break;
            case 9:
                cloneRoom_LShape_HVFlip = Instantiate(room_LShape_HVFlip, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneRoom_LShape_HVFlip;
                break;
            case 10:
                cloneStartRoom = Instantiate(startRoom, new Vector3(worldPos.x - 10, worldPos.y - 10, 0f), Quaternion.identity) as GameObject;
                generatedRoom = cloneStartRoom;
                break;
        }        

        return generatedRoom;
    }

    
    void PopulateStartRoom()
    {
        int tempX = 0; 
        int tempY = 0;

        for (int i = 0; i < 3; i++)
        {
            //tempX = UnityEngine.Random.Range(1, roomList[0].dimension - 2);
            //tempY = UnityEngine.Random.Range(1, roomList[0].dimension - 2);

            RoomObject newRoomObject = new RoomObject(roomList[0].objectList.Count, "Banana", true, tempX, tempY);
            roomList[0].objectList.Add(newRoomObject);

            cloneObject1 = Instantiate(object1, new Vector3(tempX, tempY,0f), Quaternion.identity) as GameObject;
        }


        //pick random points on roomLayout
        //create new object there
        //





    }

    void Update()
    {
        
    }
	
    

}
