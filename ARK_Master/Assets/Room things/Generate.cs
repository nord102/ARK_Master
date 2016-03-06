using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generate : MonoBehaviour 
{
    public GameObject startRoom;


    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    private GameObject cloneObject1;
    private GameObject cloneObject2;
    private GameObject cloneObject3;

    private GameObject cloneStartRoom;

    private List<Room> roomList = new List<Room>();

    
    
    //Create Image for the first room
    //Create list of rooms and add first room to it

    void Start()
    {
        PlaceStartRoom();
    }


    void PlaceStartRoom()
    {
        cloneStartRoom = Instantiate(startRoom, new Vector3(0f, 0f, 1f), Quaternion.identity) as GameObject;

        Room newRoom = new Room(roomList.Count, 1,"Explored");
        roomList.Add(newRoom);
    }

    
    void PopulateStartRoom()
    {
        int tempX = 0; 
        int tempY = 0;

        for (int i = 0; i < 3; i++)
        {
            tempX = UnityEngine.Random.Range(1, roomList[0].dimension - 2);
            tempY = UnityEngine.Random.Range(1, roomList[0].dimension - 2);

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
        if (Input.GetMouseButtonDown(0))
        {
            //PopulateStartRoom();

        }

    }
	
    

}
