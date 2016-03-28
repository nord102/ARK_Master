using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float divideTime = 1;
	private int hitPoints;
	private float timer;

	const int NORTH = 0;
	const int SOUTH = 1;
	const int WEST = 2;
	const int EAST = 3;


	
	public GameObject fire;
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
		}

	}

	void Damage(int amount)
	{
		this.hitPoints -= amount;

		if (this.hitPoints <= 0)
			Destroy (this.gameObject);
	}


	// Update is called once per frame
	void Update () 
	{

		this.timer += Time.deltaTime;
		//Debug.Log (this.timer);

		//this.gameObject.transform.position.
		if (this.timer > this.divideTime && !this.flag)
		{
			
			Vector3 v = this.gameObject.transform.position;
			Vector3 newPosition = Postion (v);

			if (newPosition != v) {
				cloneFire = Instantiate (fire, newPosition, Quaternion.identity) as GameObject;
				cloneFire.SetActive (true);
				this.flag = true;
			}
			this.flag = true;
		}

		//Debug.Log (this.timer);
		//this.gameObject.renderer.size;
	}


	private Vector3 Postion(Vector3 basePositionInput)
	{
		int[] available = this.AvailableAdjacentSpace ();
		int direction = this.RandomDirection (available);

		Vector3 basePosition = basePositionInput;

		switch (direction) {

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

		return basePosition;
	}

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

		while (breakCondition && atLeastOneAvailabe) {
			direction = Random.Range (0, 3);

			if (availableSpacesInput [direction] == 1) {
				breakCondition = false;
			}
		}

		//Debug.Log (direction);
		return direction;
	}
		
	private int[] AvailableAdjacentSpace()
	{
		int[] availableSpace = new int[4];
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Fire");

		for (int i = 0; i < availableSpace.Length; i++) {

			availableSpace [i] = 1;
			Vector3 basePosition = this.gameObject.transform.position;

			switch (i) {

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

			foreach (GameObject go in gos) {
				if (go.transform.position == basePosition) {
					availableSpace [i] = 0;
					break;
				}
			}

		}

		return availableSpace;
	}

	GameObject FindClosestEnemy() {
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Fire");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}








}
 