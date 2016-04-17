using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class SpawnEnemy : MonoBehaviour {

	public GameObject alien;
	private GameObject cloneAlien;

	public GameObject fire;

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
            //cloneAlien = Instantiate(alien, pos, Quaternion.identity);
		}

	}
    

	static Vector3 findValidPosition()
	{
		Vector3 pos = new Vector3 (0, 0, -10);
		return pos;
	}
}


//Will probably need to make an instance instead
//of having the method static
//so it can return what it just cloned
//to a list
public class InstantiateEnemy : MonoBehaviour
{ 
    //Make a list of all the 1s in the array

    public static void spawnEnemy(List<int> enemies, Room currentRoom)
    {
        int tempNum = 0;
        GameObject cloneEnemy;

        Vector2 tempPoint = new Vector2(0,0);

        
        //Random number to get a spot
        //Spawn Enemy

        //Make list of possible spots in array
        List<Vector2> possibleList = new List<Vector2>();
        for (int i = 0; i < Mathf.Sqrt(currentRoom.roomLayout.Length); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(currentRoom.roomLayout.Length); j++)
            {
                if(currentRoom.roomLayout[i,j] == 1)
                {
                    possibleList.Add(new Vector2(i, j));
                }
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            tempNum = UnityEngine.Random.Range(0, (int)Mathf.Sqrt(possibleList.Count));

            tempPoint = possibleList[tempNum];

            switch (enemies[i])
            {
                case 0:
                    {
						cloneEnemy = Instantiate(StateMachine.instance.fire, new Vector3(currentRoom.posX + tempPoint.x, currentRoom.posY + tempPoint.y, 0f), Quaternion.identity) as GameObject;
						cloneEnemy.SetActive(true);
					//Debug.Log (cloneEnemy.transform);
                    }
                    break;
                case 1:
                    {
						cloneEnemy = Instantiate(StateMachine.instance.hole, new Vector3(currentRoom.posX + tempPoint.x, currentRoom.posY + tempPoint.y, 0f), Quaternion.identity) as GameObject;
						cloneEnemy.SetActive(true);
                    }
                    break;
                case 2:
                    {
						cloneEnemy = Instantiate(StateMachine.instance.alien, new Vector3(currentRoom.posX + tempPoint.x, currentRoom.posY + tempPoint.y, 0f), Quaternion.identity) as GameObject;
						cloneEnemy.SetActive(true);
					//Debug.Log (cloneEnemy.transform);
                    }
                    break;

            }

            
        }
    }
}
