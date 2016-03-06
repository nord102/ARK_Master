using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour 
{
    public GameObject startRoom;

    private GameObject cloneStartRoom;

    private List<Room> roomList = new List<Room>();



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
    }


    void PlaceStartRoom()
    {
        cloneStartRoom = Instantiate(startRoom, new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;

        Room newRoom = new Room(roomList.Count, 10,"Explored",0,0);
        newRoom.draggingState = false;
        roomList.Add(newRoom);
    }


    void PopulateRoom(int roomID)
    {


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
        if (Input.GetMouseButtonDown(0) && boo)
        {
            //PopulateStartRoom();
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

            cloneStartRoom = Instantiate(startRoom, new Vector3(wordPos.x,wordPos.y,0f), Quaternion.identity) as GameObject;

            boo = false;
           
        }
    }
	
    

}
