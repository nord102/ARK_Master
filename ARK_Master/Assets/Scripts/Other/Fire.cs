using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Text;


public class Fire : MonoBehaviour {

	public float divideTime = 10;
	private int hitPoints;
	private float timer;

	const int NORTH = 0;
	const int SOUTH = 1;
	const int WEST = 2;
	const int EAST = 3;

	const int FIRE_DAMAGE = 5;
	
	//public GameObject fire;
	private GameObject cloneFire;

	public Animator animator;

	private bool flag = false;

	// Use this for initialization
	void Start () 
	{
		hitPoints = 10;
		animator = GetComponent<Animator>();
	}

	void OnCollisionEnter2D(Collision2D other) {

		//Debug.Log (other.gameObject.name);
		if (other.gameObject.tag == "Fire" || other.gameObject.name.Contains ("Fire")) {
			//Debug.Log ("collided");
			//Destroy (this.gameObject);
		} else if (other.gameObject.tag == "ExtinguisherSpray" || other.gameObject.name.Contains ("ExtinguisherSpray")) {
			Damage (10);
		} else if (other.gameObject.name == "Player") {
			Player.instance.Damage(FIRE_DAMAGE);
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log (other.gameObject.name);
		if (other.gameObject.tag == "Fire" || other.gameObject.name.Contains ("Fire")) {
			//Debug.Log ("collided");
			//Destroy (this.gameObject);
		} else if (other.gameObject.tag == "ExtinguisherSpray" || other.gameObject.name.Contains ("ExtinguisherSpray")) {
			Damage (10);
		} else if (other.gameObject.name == "Player") {
			Player.instance.Damage(FIRE_DAMAGE);
		}
	}

	public void Damage(int amount)
	{
		this.hitPoints -= amount;

		if (this.hitPoints <= 0)
			Destroy (this.gameObject);
	}


    private Vector3 FindAvailableSpace()
    {
        int[] availableSpace = new int[4];
        List<Vector3> availableSpaces = AvailablePosition();
        //int[] available = this.AvailableAdjacentSpace();

        for (int i = 0; i < 4; i++)
        {

            availableSpace[i] = 0;
            Vector3 basePosition = this.gameObject.transform.position;

            switch (i)
            {

                case NORTH:
                    basePosition.y -= 1f;
                    break;
                case SOUTH:
                    basePosition.y += 1f;
                    break;
                case WEST:
                    basePosition.x -= 1f;
                    break;
                case EAST:
                    basePosition.x += 1f;
                    break;

                default:
                    break;

            }

            //Debug.Log(basePosition);
            //Debug.Log("STARTING RUN THROUGH LIST");
            foreach (var pos in availableSpaces)
            {
                if (basePosition.x == pos.x && basePosition.y == pos.y)
                {
                    availableSpace[i] = 1;
                    break;
                }

                //else
                //{
                    
                //    Debug.Log(pos);
                //}
            }
            //Debug.Log("END!");

        }


        int direction = RandomDirection(availableSpace);


        Vector3 posit = this.gameObject.transform.position;

        switch (direction)
        {

            case NORTH:
                posit.y -= 1f;
                break;
            case SOUTH:
                posit.y += 1f;
                break;
            case WEST:
                posit.x -= 1f;
                break;
            case EAST:
                posit.x += 1f;
                break;

            default:
                //return base object
                break;

        }


        return posit;
    }


	// Update is called once per frame
	void Update () 
	{
		this.timer += Time.deltaTime;
		//Debug.Log (this.timer);

		//this.gameObject.transform.position.
		if (this.timer >= this.divideTime && !this.flag)
		{
			
			Vector3 v = this.gameObject.transform.position;
            //Vector3 newPosition = Position (v);
            Vector3 newPostion = FindAvailableSpace();
			//Vector2 newPosition = AvailablePosition();

			if (newPostion != this.gameObject.transform.position) {
				cloneFire = Instantiate (this.gameObject, newPostion, Quaternion.identity) as GameObject;
				//Generate.instance.currentRoom.roomLayout[(int)newPosition.x, (int)newPosition.y] = -1;
				cloneFire.SetActive (true);
			}
			this.flag = true;
		}

		//Debug.Log (this.timer);
		//this.gameObject.renderer.size;
	}
		

	//Determining where the bad spaces are in the room
	private List<Vector3> AvailablePosition()
	{
		
		List<Vector3> possibleList = new List<Vector3>();
		for (int i = 0; i < Mathf.Sqrt(Generate.instance.currentRoom.roomLayout.Length); i++)
		{
			for (int j = 0; j < Mathf.Sqrt(Generate.instance.currentRoom.roomLayout.Length); j++)
			{
				if(Generate.instance.currentRoom.roomLayout[i,j] == 1) //-1 || Generate.instance.currentRoom.roomLayout[i,j] == -2)
				{
					possibleList.Add(new Vector3(i, j, 0));
				}
			}
		}

		//int tempNum = UnityEngine.Random.Range(0, (int)Mathf.Sqrt(possibleList.Count));
		return possibleList;
	}


	//private Vector3 Position(Vector3 basePositionInput)
	//{
	//	int[] available = this.AvailableAdjacentSpace ();
	//	int direction = this.RandomDirection (available);

	//	Vector3 basePosition = basePositionInput;

	//	switch (direction) {

	//	case NORTH:
	//		basePosition.y -= 1f;
	//		break;
	//	case SOUTH:
	//		basePosition.y += 1f;
	//		break;
	//	case WEST:
	//		basePosition.x -= 1f;
	//		break;
	//	case EAST:
	//		basePosition.x += 1f;
	//		break;

	//	default:
	//		break;
			
	//	}

	//	return basePosition;
	//}

	private int RandomDirection(int[] availableSpacesInput)
	{
		bool breakCondition = true;
		int direction = 0;

		bool atLeastOneAvailabe = false;

		foreach (int i in availableSpacesInput) {
		
			if (i == 1) {
			
				atLeastOneAvailabe = true;
				break;
			}
		}

        if (atLeastOneAvailabe == false)
        {
            return -1;
        }

        while (breakCondition && atLeastOneAvailabe) {
			direction = Random.Range (0, 3);

			if (availableSpacesInput [direction] == 1) {
				breakCondition = false;
			}
		}



		//Debug.Log (direction);
		return direction;
	}
		
	//private int[] AvailableAdjacentSpace()
	//{
	//	int[] availableSpace = new int[4];
	//	GameObject[] gos = GameObject.FindGameObjectsWithTag("Fire");
	//	List<Vector3> borders = AvailablePosition ();

	//	for (int i = 0; i < availableSpace.Length; i++) {

	//		availableSpace [i] = 1;
	//		Vector3 basePosition = this.gameObject.transform.position;

	//		switch (i) {

	//		case NORTH:
	//			basePosition.y -= 1f;
	//			break;
	//		case SOUTH:
	//			basePosition.y += 1f;
	//			break;
	//		case WEST:
	//			basePosition.x -= 1f;
	//			break;
	//		case EAST:
	//			basePosition.x += 1f;
	//			break;

	//		default:
	//			break;

	//		}

	//		foreach (GameObject go in gos) {
	//			if (go.transform.position == basePosition) {
	//				availableSpace [i] = 0;
	//				break;
	//			}
	//		}
 //           //Debug.Log(basePosition);
 //           foreach (var border in borders)
 //           {

 //              // Debug.Log(border);
 //               //
 //               if (border == basePosition)
 //               {
 //                   Debug.Log("Is this getting hit");
 //                   availableSpace[i] = 0;
 //                   break;
 //               }
 //           }

 //       }

	//	return availableSpace;
	//}

	//GameObject FindClosestEnemy() {
	//	GameObject[] gos;
	//	gos = GameObject.FindGameObjectsWithTag("Fire");
	//	GameObject closest = null;
	//	float distance = Mathf.Infinity;
	//	Vector3 position = transform.position;
	//	foreach (GameObject go in gos) {
	//		Vector3 diff = go.transform.position - position;
	//		float curDistance = diff.sqrMagnitude;
	//		if (curDistance < distance) {
	//			closest = go;
	//			distance = curDistance;
	//		}
	//	}
	//	return closest;
	//}

}
 