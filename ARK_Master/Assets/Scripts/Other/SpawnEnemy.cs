using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

	public GameObject alien;
	private GameObject alienClone;

	public SpawnEnemy(int typeOfEnemy, int amount)
	{
		spawnEnemy (typeOfEnemy, amount);
	}

	public static void spawnEnemy(int typeOfEnemy, int amount)
	{ 
		//GameObject temp;
		for (int i = 0; i < amount; i++)
		{
			Vector3 pos = findValidPosition ();
			//Instantiate(alien, this.gameObject.transform.position, Quaternion.identity);
			//alienClone = Instantiate(alien, pos, Quaternion.identity);
		}

	}


	static Vector3 findValidPosition()
	{
		Vector3 pos = new Vector3 (0, 0, -10);
		return pos;
	}
}
