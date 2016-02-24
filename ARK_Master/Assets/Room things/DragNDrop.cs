using UnityEngine;
using System.Collections;
using System;

public class DragNDrop : MonoBehaviour
{

    public GameObject room_1x1;
    public GameObject room_1x2;
    public GameObject room_2x2;
    public GameObject room_smallL;

    private GameObject cloneRoom_1x1;
    private GameObject cloneRoom_1x2;
    private GameObject cloneRoom_2x2;
    private GameObject cloneRoom_smallL;

    private GameObject[] roomSelectList;

    private int roomSelected = 0;

    // Use this for initialization
    void Start()
    {
        roomSelectList = new GameObject[12];

        roomSelectList[0] = room_1x1;
        roomSelectList[1] = room_1x2;
        roomSelectList[2] = room_2x2;
        roomSelectList[3] = room_smallL;
    }





    // Update is called once per frame
    void Update()
    {

       if(Input.GetMouseButton(1))
        {
            cloneRoom_1x1.transform.position += new Vector3(1f,0f,0f);
        }


        //Click on the room they want
        ///Raycast to check which is hit and if it is selectable
        ///Spawn Prefab that follows mouse
        ////Turns red if collision, and normal if not        

    }

    public void Room1x1Click()
    {
        //cloneRoom_1x1 = Instantiate(room_1x1, new Vector2((Input.mousePosition.x), (Input.mousePosition.z)), Quaternion.identity) as GameObject;
        cloneRoom_1x1 = Instantiate(room_1x1, new Vector2(0f, 0f), Quaternion.identity) as GameObject;
    }



}
