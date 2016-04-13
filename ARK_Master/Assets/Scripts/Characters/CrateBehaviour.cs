using UnityEngine;
using System.Collections;

public class CrateBehaviour : MonoBehaviour {

    public int health = 10;
    public int roomNumber = 0;
    public int posX = 0;
    public int posY = 0;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Laser"))
        {
            SetHealth(-2);
        }
    }

    void SetHealth(int healthChange)
    {
        health += healthChange;
        if (health <= 0)
        {
            //Kill the crate, award the player with resources
            int resourcesGained = Random.Range(10, 31);
            StateMachine.instance.sInfo.SetResources(resourcesGained);
            
            //Show some popup text that says how much money was gained?

            //Kill the game object - set tile to 0, change image to broken crate
            Renderer rend = gameObject.GetComponent<Renderer>();
            rend.material = Resources.Load<Material>("CrateBroken");
            Generate.instance.GetRoomGameObjectList()[roomNumber - 1].GetComponent<Room>().roomLayout[posX, posY] = 0;
            Destroy(gameObject.GetComponent<CrateBehaviour>());
            Destroy(gameObject.GetComponent<Collider2D>());
        }
    }
}
