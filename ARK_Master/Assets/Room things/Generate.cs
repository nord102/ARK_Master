using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour
{
    public static Generate instance = null;

    #region Door
    public GameObject door;
    private GameObject cloneDoor;

    public Door currentDoor;
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
    private List<GameObject> roomGameObjectList = new List<GameObject>();
    private List<RoomComponent> roomComponentList = new List<RoomComponent>();
    private List<GameObject> doorGameObjectList = new List<GameObject>();
    #endregion

    //--
    #region Object things
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    private GameObject cloneObject1;
    private GameObject cloneObject2;
    private GameObject cloneObject3;
    #endregion
    //--

    //Create Image for the first room
    //Create list of rooms and add first room to it

    void Start()
    {
        PlaceStartRoom();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #region List Accessors
    public List<GameObject> GetRoomGameObjectList()
    {
        return roomGameObjectList;
    }

    public List<RoomComponent> GetRoomComponentList()
    {
        return roomComponentList;
    }

    public List<GameObject> GetDoorGameObjectList()
    {
        return doorGameObjectList;
    }

    #endregion

    void PlaceStartRoom()
    {
        //Create Room GameObject
        cloneStartRoom = Instantiate(startRoom, new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;
        roomGameObjectList.Add(cloneStartRoom);

        //Initialize Room
        Room newRoom = cloneStartRoom.GetComponent<Room>();
        newRoom.Initialize(roomGameObjectList.Count, 10, 0, "Explored", 0, 0);
        newRoom.draggingState = false;

        //Initialize Room Components
        foreach (RoomComponent roomCom in newRoom.GetComponentList())
        {
            roomComponentList.Add(roomCom);
        }
    }

    public void checkForDoors()
    {
        bool doorMade = false;

        //Room that was most recently placed
        GameObject recentRoomGameObject = roomGameObjectList[roomGameObjectList.Count - 1];
        Room recentRoom = recentRoomGameObject.GetComponent<Room>();

        //--
        recentRoom.InitializeDoorList();
        //--

        Door newDoor = new Door();

        //Check all of the recent Room Components for neighbours       
        foreach (RoomComponent recentRoomCom in recentRoom.GetComponentList())
        {
            foreach (RoomComponent globalRoomCom in roomComponentList)
            {
                #region (X+, Y) (RIGHT)
                if (((recentRoomCom.posX + (recentRoom.dimension - 1) == globalRoomCom.posX) && (recentRoomCom.posY == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {
                    /// <summary>
                    /// Make Door - Coordinates at (X+,Y+1/2)
                    /// </summary>

                    //Make Door GameObject
                    cloneDoor = Instantiate(door, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

                    //Get the Door Script Component of the Door GameObject
                    newDoor = cloneDoor.GetComponent<Door>();

                    //Initialize newDoor with variables
                    newDoor.Initialize(recentRoom.GetDoorList().Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX + recentRoom.dimension - 1), (recentRoomCom.posY + (recentRoom.dimension - 1) / 2), 1);

                    //Set Door GameObject Position
                    cloneDoor.transform.position = new Vector3(newDoor.posX, newDoor.posY, 1f);
                    doorMade = true;
                }
                #endregion
                #region (X-, Y) (LEFT)
                else if (((recentRoomCom.posX - (recentRoom.dimension - 1) == globalRoomCom.posX) && (recentRoomCom.posY == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {
                    /// <summary>
                    /// Make Door - Coordinates at (X,Y+1/2)
                    /// </summary>

                    //Make Door GameObject
                    cloneDoor = Instantiate(door, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

                    //Get the Door Script Component of the Door GameObject
                    newDoor = cloneDoor.GetComponent<Door>();

                    //Initialize newDoor with variables
                    newDoor.Initialize(recentRoom.GetDoorList().Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX), (recentRoomCom.posY + (recentRoom.dimension - 1) / 2), 1);

                    //Set Door GameObject Position
                    cloneDoor.transform.position = new Vector3(newDoor.posX, newDoor.posY, 1f);
                    doorMade = true;
                }
                #endregion
                #region (X, Y+) (UP)
                else if (((recentRoomCom.posX == globalRoomCom.posX) && (recentRoomCom.posY + (recentRoom.dimension - 1) == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {
                    /// <summary>
                    /// Make Door - Coordinates at (X+1/2,Y+)
                    /// </summary>

                    //Make Door GameObject
                    cloneDoor = Instantiate(door, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

                    //Get the Door Script Component of the Door GameObject
                    newDoor = cloneDoor.GetComponent<Door>();

                    //Initialize newDoor with variables
                    newDoor.Initialize(recentRoom.GetDoorList().Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX + (recentRoom.dimension - 1) / 2), (recentRoomCom.posY + (recentRoom.dimension - 1)), 1);

                    //Set Door GameObject Position
                    cloneDoor.transform.position = new Vector3(newDoor.posX, newDoor.posY, 1f);
                    doorMade = true;
                }
                #endregion
                #region (X, Y-) (DOWN)
                else if (((recentRoomCom.posX == globalRoomCom.posX) && (recentRoomCom.posY - (recentRoom.dimension - 1) == globalRoomCom.posY)) && (recentRoomCom.roomID != globalRoomCom.roomID))
                {
                    /// <summary>
                    /// Make Door - Coordinates at (X+1/2,Y)
                    /// </summary>

                    //Make Door GameObject
                    cloneDoor = Instantiate(door, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;

                    //Get the Door Script Component of the Door GameObject
                    newDoor = cloneDoor.GetComponent<Door>();

                    //Initialize newDoor with variables
                    newDoor.Initialize(recentRoom.GetDoorList().Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX + (recentRoom.dimension - 1) / 2), recentRoomCom.posY, 1);

                    //Set Door GameObject Position
                    cloneDoor.transform.position = new Vector3(newDoor.posX, newDoor.posY, 1f);
                    doorMade = true;
                }
                #endregion

                if (doorMade)
                {
                    doorMade = false;

                    //Add door to global Dor GameObjectList
                    doorGameObjectList.Add(cloneDoor);

                    //--
                    //Debug.Log("DoorPOSCHANGED: " + (newDoor.posX - recentRoom.posX) + " " + (newDoor.posY - recentRoom.posY));
                    int index = recentRoom.GetDoorList().FindIndex(d => d.posX == newDoor.posX && d.posY == newDoor.posY);
                    newDoor.SetDoorID(index);
                    recentRoom.GetDoorList()[index] = newDoor;
                    //--

                    //Add to Recent room door list
                    recentRoom.GetDoorList().Add(newDoor);

                    //Add to Connected room door list
                    GameObject connectingRoomGameObject = roomGameObjectList[globalRoomCom.roomID - 1];
                    Room connectingRoom = connectingRoomGameObject.GetComponent<Room>();

                    #region Take Out Walls at Door Location
                    //Create Door
                    //Take out the Wall on the Dragged Room
                    foreach (Transform child in recentRoomGameObject.transform)
                    {
                        foreach (Transform smallerChild in child.transform)
                        {
                            //Destroy and replace smallerchild
                            if (smallerChild.transform.position.x == newDoor.posX && smallerChild.transform.position.y == newDoor.posY)
                            {
                                //Hide Wall where Door is going
                                smallerChild.gameObject.SetActive(false);
                                break;
                            }
                        }
                    }

                    //Take out Wall on the Connected Room
                    foreach (Transform child in connectingRoomGameObject.transform)
                    {
                        foreach (Transform smallerChild in child.transform)
                        {
                            //Destroy and replace smallerchild
                            if (smallerChild.transform.position.x == newDoor.posX && smallerChild.transform.position.y == newDoor.posY)
                            {
                                //Hide Wall where Door is going
                                smallerChild.gameObject.SetActive(false);
                                break;
                            }
                        }
                    }
                    #endregion
                }
            }
        }
    }

    //Possibly fix each room's starting World Position
    public GameObject GenerateRoom(int roomShape)
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

    public void PopulateRoom(Room room)
    {
        int tempX = 0;
        int tempY = 0;

        int minItems = UnityEngine.Random.Range(1, (room.roomLayout.Length - 1) / 2);
        int maxItems = UnityEngine.Random.Range(1, (room.roomLayout.Length - 1));
        if (maxItems < minItems) { maxItems = minItems; }
        int actualItems = UnityEngine.Random.Range(minItems, maxItems);

        //string t = "";
        //for (int i = 0; i < 7; ++i)
        //{
        //    for (int j = 0; j < 7; ++j)
        //    {
        //        t += room.roomLayout[i, j] + " ";
        //    }
        //    t += "\n";
        //}
        //Debug.Log(t);

        for (int i = 0; i < actualItems; i++)
        {
            tempX = UnityEngine.Random.Range(1, (int)Mathf.Sqrt(room.roomLayout.Length));
            tempY = UnityEngine.Random.Range(1, (int)Mathf.Sqrt(room.roomLayout.Length));

            if (room.roomLayout[tempX, tempY] == 0)
            {
                //Debug.Log(tempX + ", " + tempY);
                RoomObject newRoomObject = new RoomObject(room.GetObjectList().Count, "Banana", true, tempX, tempY);
                room.GetObjectList().Add(newRoomObject);

                cloneObject1 = Instantiate(object1, new Vector3(room.posX + tempX, room.posY + tempY, 0f), Quaternion.identity) as GameObject;
                Renderer rend = cloneObject1.GetComponent<Renderer>();
                rend.material = GetRandomRoomObject(room.roomType);
                room.roomLayout[tempX, tempY] = -1;
            }
        }

    }

    Material GetRandomRoomObject(int roomType)
    {
        Material m = null;

        //0-4, the number of materials we currently have
        switch (Random.Range(0, 5))
        {
            case 0:
                m = Resources.Load<Material>("Cabinet");
                break;
            case 1:
                m = Resources.Load<Material>("Chair");
                break;
            case 2:
                m = Resources.Load<Material>("Computer");
                break;
            case 3:
                m = Resources.Load<Material>("Crate");
                break;
            case 4:
                m = Resources.Load<Material>("Table");
                break;
        }
        return m;
    }
}