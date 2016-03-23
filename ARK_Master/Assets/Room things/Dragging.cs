using UnityEngine;
using System.Collections;

public class Dragging : MonoBehaviour
{
    #region Dragging Variables
    //GameObject being Dragged
    public GameObject gameObjectToDrag;

    //Grid steps
    private float xStep = 7f;
    private float yStep = 7f;
    private int gridStepsX = 0;
    private int gridStepsY = 0;

    private Vector3 GOCenter;
    private Vector3 touchPosition;
    private Vector3 offset;
    private Vector3 newGOCenter;
    #endregion

    private int globalRoomShape;

    RaycastHit hit;

    public bool draggingMode = false;

    //Boundary Detection
    private bool overlap = false;
    private bool highlighted = false;

    Room newRoom = new Room();

    public void StartDragging(int roomShape)
    {
        //Get the chosen room shape
        globalRoomShape = roomShape;

        //Create GameObject based on room shape
        gameObjectToDrag = Generate.instance.GenRoom(roomShape);
        GOCenter = gameObjectToDrag.transform.position;
        touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = touchPosition - GOCenter;

        //----

        Generate.instance.GetRoomGameObjectList().Add(gameObjectToDrag);

        //Get the Room Script Component of the Room GameObject
        newRoom = gameObjectToDrag.GetComponent<Room>();
        newRoom.Initialize(Generate.instance.GetRoomGameObjectList().Count, globalRoomShape, 0, "Explored", (int)gameObjectToDrag.transform.position.x, (int)gameObjectToDrag.transform.position.y);

        //----

        draggingMode = true;
    }

    void Update()
    {
        if (draggingMode)
        {
            //
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newGOCenter = touchPosition - offset;

            ///GRID BASED MOVEMENT

            Vector3 pos = gameObjectToDrag.transform.position;

            #region X POSITION
            gridStepsX = Mathf.RoundToInt(newGOCenter.x / xStep);
            pos.x = ((float)gridStepsX) * xStep;

            //Adjusts x position
            if (pos.x < 0)
            { pos.x += Mathf.Abs(gridStepsX); }
            else if (pos.x > 0)
            { pos.x -= Mathf.Abs(gridStepsX); }
            #endregion

            #region Y POSITION
            gridStepsY = Mathf.RoundToInt(newGOCenter.y / yStep);
            pos.y = ((float)gridStepsY) * yStep;
            //Adjusts y position
            if (pos.y < 0)
            { pos.y += Mathf.Abs(gridStepsY); }
            else if (pos.y > 0)
            { pos.y -= Mathf.Abs(gridStepsY); }
            #endregion

            #region Z POSITION
            pos.z = GOCenter.z;
            #endregion

            //Update Positions of everything associated with the GameObject being dragged

            //Set position of GameObject
            gameObjectToDrag.transform.position = pos;

            //----

            //Set position of Room
            newRoom.posX = (int)gameObjectToDrag.transform.position.x;
            newRoom.posY = (int)gameObjectToDrag.transform.position.y;

            newRoom.SetRoomComponentCoordinates();
            //----

            //---

            //IF THE ROOM IS OVERLAPPING ANOTHER ROOM

            foreach (RoomComponent roomCom in newRoom.GetComponentList())
            {
                foreach (RoomComponent globalCom in Generate.instance.GetRoomComponentList())
                {
                    if ((roomCom.posX == globalCom.posX) && (roomCom.posY == globalCom.posY))
                    {
                        overlap = true;
                        break;
                    }
                    else
                    {
                        overlap = false;
                    }

                }

                if (overlap)
                {
                    break;
                }
            }

            if (overlap)
            {
                //HIGHLIGHT RED FOR ERROR 
                foreach (Transform child in gameObjectToDrag.transform)
                {
                    foreach (Transform smallerChild in child.transform)
                    {
                        Renderer rend = smallerChild.GetComponent<Renderer>();
                        rend.material.SetColor("_Color", Color.red);
                    }
                }

                highlighted = true;

                overlap = false;
            }
            else
            {
                //NO HIGHLIGHT FOR NO ERROR
                foreach (Transform child in gameObjectToDrag.transform)
                {
                    foreach (Transform smallerChild in child.transform)
                    {
                        Renderer rend = smallerChild.GetComponent<Renderer>();
                        rend.material.SetColor("_Color", Color.white);
                    }
                }

                highlighted = false;
            }


        }


        if (Input.GetMouseButton(0) && draggingMode && (!highlighted))
        {
            draggingMode = false;

            gameObjectToDrag.transform.position = new Vector3(gameObjectToDrag.transform.position.x,gameObjectToDrag.transform.position.y,1f);

            //Update Room to final (X, Y) of Gameobject
            newRoom.posX = (int)gameObjectToDrag.transform.position.x;
            newRoom.posY = (int)gameObjectToDrag.transform.position.y;

            //Update Room Components


            foreach (RoomComponent roomCom in newRoom.GetComponentList())
            {
                Generate.instance.GetRoomComponentList().Add(roomCom);
            }

            Generate.instance.checkForDoors();

            newRoom.roomEvent = EventSystem.GenerateRoomEvent(0, newRoom.GetComponentList().Count);
            // newRoom.roomLayout = Pathfinding.DeterminePaths(newRoom);
        }

    }
}
