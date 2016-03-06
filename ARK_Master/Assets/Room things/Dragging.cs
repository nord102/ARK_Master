using UnityEngine;
using System.Collections;

public class Dragging : MonoBehaviour
{
    public float xStep = 6f;
    public float yStep = 6f;
    public int gridStepsX = 0;
    public int gridStepsY = 0;
    


    public GameObject gameObjectToDrag;

    public Vector3 GOCenter;
    public Vector3 touchPosition;
    public Vector3 offset;
    public Vector3 newGOCenter;

    RaycastHit hit;

    public bool draggingMode = false;

    // Use this for initialization
    void Start ()
    {
	
	}

	// Update is called once per frame
	void Update ()
    {
        //to check what will be getting dragged
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray,out hit))
            {
                gameObjectToDrag = hit.collider.gameObject;
                GOCenter = gameObjectToDrag.transform.position;
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset = touchPosition - GOCenter;
                draggingMode = true;
            }

        }

        //while dragging
        if(Input.GetMouseButton(0))
        {
            if(draggingMode)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                newGOCenter = touchPosition - offset;

                
                ///GRID BASED MOVEMENT

                Vector3 pos = gameObjectToDrag.transform.position;

                //X POSITION
                gridStepsX = Mathf.RoundToInt(newGOCenter.x / xStep);               
                pos.x = ((float)gridStepsX) * xStep;
                //Adjusts x position
                if (pos.x < 0)
                { pos.x += Mathf.Abs(gridStepsX); }
                else if (pos.x > 0)
                { pos.x -= Mathf.Abs(gridStepsX); }

                //Y POSITION
                gridStepsY = Mathf.RoundToInt(newGOCenter.y / yStep);               
                pos.y = ((float)gridStepsY) * yStep;
                //Adjusts y position
                if (pos.y < 0)
                { pos.y += Mathf.Abs(gridStepsY); }
                else if (pos.y > 0)
                { pos.y -= Mathf.Abs(gridStepsY); }
                Debug.Log("GRID:" + gridStepsY + " POS" + pos.y);
                //Z POSITION
                pos.z = GOCenter.z;
                
                //Set position
                gameObjectToDrag.transform.position = pos;

                //IF THE ROOM IS OVERLAPPING ANOTHER ROOM
                if (Input.GetKeyDown(KeyCode.Space))
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
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
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
                }

            }
        }

        //after release
        if(Input.GetMouseButtonUp(0))
        {
            draggingMode = false;
        }

	}
}
