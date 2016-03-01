using UnityEngine;
using System.Collections;

public class GenerateRoom : MonoBehaviour {

	public GameObject floorTile1;
	public GameObject wallTile1;
	private Vector3 scale;
	public int roomWidth;
	public int roomHeight;
	public int wallHeight;
	public Vector3 BottomCorner = new Vector3 ();

	public float floorRight;
	public float floorUp;
	public float wallRight;
	public float wallUp;

	// Use this for initialization
	void Start () {
		scale = new Vector3(0.25f, 0.25f, 1);
		CreateRoom (BottomCorner, roomHeight, roomWidth);
	}

	public void CreateRoom(Vector3 corner, int Height, int Width) {
		//Generates floor tiles
		for (int i = 0; i < Width; ++i) {
			for (int j = 0; j < Height; ++j)
			{
				GameObject newObject = GameObject.Instantiate (floorTile1);
				Vector3 newLocation = new Vector3(corner.x + ((float)j * floorRight - ((float)i * floorRight)),corner.y + ((float)j * floorUp + ((float)i * floorUp)), 1);
				newObject.transform.localScale = scale;
				newObject.transform.Translate(Vector3.Scale(newLocation, scale));
			}
		}
		//Generates wall tiles - generate 3 sets of wall tiles on the top of the outside floor rows
		for (int i = 0; i < wallHeight; i ++) {
			for (int j = 0; j < Height; ++j)
			{
				GameObject newObject = GameObject.Instantiate (wallTile1);
				//Vector3 newLocation = new Vector3(wallRight,wallUp,0) ;
				Vector3 newLocation = new Vector3(corner.x + (2 * ((float)Width * wallRight) - wallRight) - ((float)j * wallRight * 2), corner.y + (((float)Width -1 + j) * wallUp * .67f) + wallUp + (i * 5.4f) , 1) ;
				newObject.transform.localScale = scale;
				newObject.transform.Translate(Vector3.Scale(newLocation, scale));
			}
		}



	}
}
