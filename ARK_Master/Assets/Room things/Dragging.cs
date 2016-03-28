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
    private bool placeable = false;

    Room newRoom = new Room();

    public void StartDragging(int roomShape)
    {
        //Get the chosen room shape
        globalRoomShape = roomShape;

        //Create GameObject based on room shape
        gameObjectToDrag = Generate.instance.GenerateRoom(roomShape);
        GOCenter = gameObjectToDrag.transform.position;
        touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = touchPosition - GOCenter;

        //Add the GameObject to the Global List
        Generate.instance.GetRoomGameObjectList().Add(gameObjectToDrag);

        //Get the Room Script Component of the Room GameObject
        newRoom = gameObjectToDrag.GetComponent<Room>();
        newRoom.Initialize(Generate.instance.GetRoomGameObjectList().Count, globalRoomShape, 0, "Explored", (int)gameObjectToDrag.transform.position.x, (int)gameObjectToDrag.transform.position.y);

        draggingMode = true;
    }

    public void HighlightToggle()
    {
        #region Highlighting Room
        //If the Room being dragged is overlapping another Room
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
            //Highlight the Room RED for overlap
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
            //Highlight the Room WHITE for no overlap
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
        #endregion
    }

    public void CheckIfPlaceable()
    {
        foreach (RoomComponent newRoomCom in newRoom.GetComponentList())
        {
            foreach (RoomComponent globalRoomCom in Generate.instance.GetRoomComponentList())
            {
                //Check if there is at least one global RoomComponent beside a newRoom Room RoomComponent
                #region (X+, Y) (RIGHT)
                if (((newRoomCom.posX + (newRoom.dimension - 1) == globalRoomCom.posX) && (newRoomCom.posY == globalRoomCom.posY)) && (newRoomCom.roomID != globalRoomCom.roomID))
                {
                    placeable = true;
                    break;
                }
                #endregion
                #region (X-, Y) (LEFT)
                else if (((newRoomCom.posX - (newRoom.dimension - 1) == globalRoomCom.posX) && (newRoomCom.posY == globalRoomCom.posY)) && (newRoomCom.roomID != globalRoomCom.roomID))
                {
                    placeable = true;
                    break;
                }
                #endregion
                #region (X, Y+) (UP)
                else if (((newRoomCom.posX == globalRoomCom.posX) && (newRoomCom.posY + (newRoom.dimension - 1) == globalRoomCom.posY)) && (newRoomCom.roomID != globalRoomCom.roomID))
                {
                    placeable = true;
                    break;
                }
                #endregion
                #region (X, Y-) (DOWN)
                else if (((newRoomCom.posX == globalRoomCom.posX) && (newRoomCom.posY - (newRoom.dimension - 1) == globalRoomCom.posY)) && (newRoomCom.roomID != globalRoomCom.roomID))
                {
                    placeable = true;
                    break;
                }
                #endregion
                else
                {
                    placeable = false;
                }

            }
            if (placeable)
            {
                break;
            }
        }
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

            HighlightToggle();

            CheckIfPlaceable();
            
        }

        //Mouse Click + Room is being dragged + that Room isn't highlighted + it's in a placeable zone
        if (Input.GetMouseButton(0) && draggingMode && (!highlighted) && (placeable))
        {
            draggingMode = false;
            placeable = false;

            //Sets the GameObject position
            gameObjectToDrag.transform.position = new Vector3(gameObjectToDrag.transform.position.x, gameObjectToDrag.transform.position.y, 1f);

            //Sets the Room that was just placed (X, Y) of Gameobject
            newRoom.posX = (int)gameObjectToDrag.transform.position.x;
            newRoom.posY = (int)gameObjectToDrag.transform.position.y;

            //Add Room Components to Global Room Components
            foreach (RoomComponent roomCom in newRoom.GetComponentList())
            {
                Generate.instance.GetRoomComponentList().Add(roomCom);
            }

            //Check for doors
            Generate.instance.CheckForDoors();

            newRoom.roomEvent = EventSystem.GenerateRoomEvent(0, newRoom.GetComponentList().Count);
            newRoom.roomLayout = Pathfinding.DeterminePaths(newRoom);
            Generate.instance.PopulateRoom(newRoom);
        }
    }
}
