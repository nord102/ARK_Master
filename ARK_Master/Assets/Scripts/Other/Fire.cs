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


    const double FIREFIGHTER_DAMAGE = 5;
    const double OTHER_DAMAGE = 10;

    public int extinguisherDamageOnFire = 4;
    public int regularExtingisherDamage = 2;
	
	//public GameObject fire;
	private GameObject cloneFire;

	public Animator animator;

	private bool flag = false;

	Events currentEvent;

	// Use this for initialization
	void Start () 
	{
		hitPoints = 10;
		animator = GetComponent<Animator>();
		currentEvent    = Generate.instance.currentRoom.roomEvent;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
 		if (other.gameObject.tag == "ExtinguisherSpray" || other.gameObject.name.Contains ("ExtinguisherSpray")) {
			Damage ();
		} else if (other.gameObject.name == "Player") {

            double damage = 0;

            switch (GlobalVariables.selectedCharacter)
            {
                case GlobalVariables.FIREFIGHTER:
                    damage = FIREFIGHTER_DAMAGE;
                    break;

                default:
                    damage = OTHER_DAMAGE;
                    break;
            }

            Player.instance.Damage(damage);
		}
	}

	public void Damage()
	{
        switch(GlobalVariables.selectedCharacter)
        {
            case GlobalVariables.FIREFIGHTER:
                this.hitPoints -= this.extinguisherDamageOnFire;
                break;

            default:
                this.hitPoints -= regularExtingisherDamage;
                break;
        }
		

		if (this.hitPoints <= 0) {

			currentEvent.Enemies.Remove (0);

			//Check if this was the last enemy alive - if so, end the event
			if (currentEvent.Enemies.Count <= 0) {
				StateMachine.instance.EndEvent (currentEvent);
			}
			Destroy (this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		this.timer += Time.deltaTime;

		if (this.timer >= this.divideTime && !this.flag)
		{
			
			Vector3 v = this.gameObject.transform.position;
            Vector3 newPosition = Position (v);

			if (newPosition != this.gameObject.transform.position) {
				cloneFire = Instantiate (this.gameObject, newPosition, Quaternion.identity) as GameObject;
				//Generate.instance.currentRoom.roomLayout[(int)newPosition.x, (int)newPosition.y] = -1;
				currentEvent.Enemies.Add(0);
				cloneFire.SetActive (true);
			}
			this.timer = 0;
		}
	}
		

	//Determining where the bad spaces are in the room
	private List<Vector3> AvailablePosition()
	{
		
		List<Vector3> possibleList = new List<Vector3>();
		for (int i = 0; i < Mathf.Sqrt(Generate.instance.currentRoom.roomLayout.Length); i++)
		{
			for (int j = 0; j < Mathf.Sqrt(Generate.instance.currentRoom.roomLayout.Length); j++)
			{
				if(Generate.instance.currentRoom.roomLayout[i,j] == -1 || 
					Generate.instance.currentRoom.roomLayout[i,j] == -2 ||
					Generate.instance.currentRoom.roomLayout[i,j] == -3
				)
				{
					possibleList.Add(new Vector3(Generate.instance.currentRoom.posX + i, Generate.instance.currentRoom.posY + j, 0));
				}
			}
		}
		return possibleList;
	} 


	private Vector3 Position(Vector3 basePositionInput)
	{
		int[] available = this.AvailableAdjacentSpace ();
		int direction = this.RandomDirection (available);

		Vector3 basePosition = basePositionInput;

		switch (direction) {

		case NORTH:
			basePosition.y += 1f;
			break;
		case SOUTH:
			basePosition.y -= 1f;
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

		return basePosition;
	}

	private int RandomDirection(int[] availableSpacesInput)
	{
		bool breakCondition = true;
		int direction = 0;

		bool atLeastOneAvailabe = false;

		for (int i = 0; i < availableSpacesInput.Length; i++) {
		
			if (availableSpacesInput[i] == 1) {	
				atLeastOneAvailabe = true;
			}
		}

        if (!atLeastOneAvailabe)
        {
            return -1;
        }

        while (breakCondition) {
			direction = Random.Range (0, 4);

			if (availableSpacesInput [direction] == 1) {
				breakCondition = false;
				break;
			}
		}

		return direction;
	}
		
	private int[] AvailableAdjacentSpace()
	{
		int[] availableSpace = new int[4];
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Fire");
		List<Vector3> borders = AvailablePosition ();

		for (int i = 0; i < availableSpace.Length; i++) 
		{

			availableSpace [i] = 1;
			Vector3 basePosition = this.gameObject.transform.position;

			switch (i) {

			case NORTH:
				basePosition.y += 1f;
				break;
			case SOUTH:
				basePosition.y -= 1f;
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

			foreach (GameObject go in gos) {
				if (go.transform.position == basePosition) {
					availableSpace [i] = 0;
					break;
				}
			}

            foreach (var border in borders)
            {
                if (border == basePosition)
                {
                    availableSpace[i] = 0;
                    break;
                }
            }

        }

		return availableSpace;
	}
}
 