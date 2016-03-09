using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour 
{
    public static Generate instance = null;



    public GameObject startRoom;

    private GameObject cloneStartRoom;

    private List<Room> roomList = new List<Room>();

    private List<RoomComponent> roomComponentList = new List<RoomComponent>();

    //--
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    private GameObject cloneObject1;
    private GameObject cloneObject2;
    private GameObject cloneObject3;

    private bool boo = true;
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
        //cloneStartRoom = Instantiate(startRoom, new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;

        Room newRoom = new Room(roomList.Count, 10,"Explored",0,0);
        newRoom.draggingState = false;
        roomList.Add(newRoom);

        foreach (RoomComponent roomCom in newRoom.componentList)
        {
            roomComponentList.Add(roomCom);
        }
    }

    void checkForDoors()
    {
        //get the room that was just placed 
        Room recentRoom = roomList[roomList.Count - 1];

        //Check all of the recent Room Components for neighbours

        foreach (RoomComponent recentRoomCom in recentRoom.componentList)
        {
            foreach (RoomComponent globalRoomCom in roomComponentList)
            {
                //X+ Y
                if ((recentRoomCom.posX + recentRoom.dimension == globalRoomCom.posX) && (recentRoomCom.posY == globalRoomCom.posY))
                {
                    //Make Door (X+,Y+1/2)
                    Door newDoor = new Door(recentRoom.roomDoorList.Count, recentRoom.roomID, globalRoomCom.roomID,
                        (recentRoomCom.posX + recentRoom.dimension), (recentRoomCom.posY + (recentRoom.dimension + 1) / 2), true);

                    //Add to Recent room door list
                    recentRoom.roomDoorList.Add(newDoor);

                    //Add to Connected room door list?
                    

                }
                //X- Y
                else if ((recentRoomCom.posX - recentRoom.dimension == globalRoomCom.posX) && (recentRoomCom.posY == globalRoomCom.posY))
                {
                    //Make Door (X,Y+1/2)
                }
                //X Y+
                else if ((recentRoomCom.posX == globalRoomCom.posX) && (recentRoomCom.posY + recentRoom.dimension == globalRoomCom.posY))
                {
                    //Make Door (X+1/2,Y+)
                }
                //X Y-
                else if ((recentRoomCom.posX == globalRoomCom.posX) && (recentRoomCom.posY - recentRoom.dimension == globalRoomCom.posY))
                {
                    //Make Door (X+1/2,Y)
                }
            }
        }


    }


    void PopulateRoom(int roomID)
    {


    }

    public GameObject GenRoom()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
        Vector3 wordPos;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 1000f))
        {
            wordPos = hit.point;
        }
        else
        {
            wordPos = Camera.main.ScreenToWorldPoint(mousePos);
        }

        //cloneStartRoom = Instantiate(startRoom, new Vector3(wordPos.x - 10, wordPos.y - 10, 0f), Quaternion.identity) as GameObject;

     

        return cloneStartRoom;
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

            //cloneObject1 = Instantiate(object1, new Vector3(tempX, tempY,0f), Quaternion.identity) as GameObject;
        }


        //pick random points on roomLayout
        //create new object there
        //





    }

    void Update()
    {


<<<<<<< HEAD
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                wordPos = hit.point;
            }
            else
            {
                wordPos = Camera.main.ScreenToWorldPoint(mousePos);
            }

            //cloneStartRoom = Instantiate(startRoom, new Vector3(wordPos.x,wordPos.y,0f), Quaternion.identity) as GameObject;

            boo = false;
           
        }
=======
>>>>>>> 6303f74b5325a09f2e9f5e2046b3d060db210e38
    }
	
    

}
