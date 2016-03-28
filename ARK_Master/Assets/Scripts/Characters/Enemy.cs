using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float speed = 25f;
    public bool stop;
    public GameObject playerLocation;
    public Animator animator;

    

    private bool lastX;
    private float range;
    public float health;


    // Sounds
    //public GameObject attackSound;

    // Use this for initialization
    void Start()
    {
        stop = false;
        health = 10;
        range = 10;
        lastX = false;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            Chase();
        }
    }



    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Player"))
        {

            animator.SetTrigger ("attack");
            Player.instance.Damage(5);
            AlienAttack.instance.PlaySound();

            if (!Player.instance.isActiveAndEnabled)
            {
                stop = true;
            }
        }

        else if (coll.gameObject.name.Contains("Laser"))
        {
            Damage(2);
        }
    }

    private void Damage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }

    }

    private bool CheckRange()
    {
        bool inRange = false;

        Vector3 playerPos = playerLocation.transform.position;
		//Vector3 playerPos = new Vector3(0,0,-10);

        float x = playerPos.x - transform.position.x;
        float y = playerPos.y - transform.position.y;

        if (x < 0)
            x *= -1;
        if (y < 0)
            y *= -1;

        if ( x <= range || y <= range)
        {
            inRange = true;
        }

        return inRange;
    }
     

    private void Chase()
    {
        Vector3 vel = new Vector3();
        bool currentX = false;

        Vector3 playerPos = playerLocation.transform.position;
		//Vector3 playerPos = new Vector3(0,0,-10);


        float x = playerPos.x - transform.position.x;
        float y = playerPos.y - transform.position.y;

        if (CheckRange())
        {
            if (x < 0)
            {
                vel.x -= speed;
                currentX = true;
				animator.SetBool("runDirection", true);

            }

            else if (x > 0)
            {
                vel.x += speed;
                currentX = false;
				animator.SetBool("runDirection", false);

            }

            if (y < 0)
            {
                vel.y -= speed;
            }

            else if (y > 0)
            {
                vel.y += speed;
                
            }

            //if (currentX  != lastX)
            //{
              //  lastX = currentX;
                //animator.SetBool("runDirection", false);
            //}


            vel.z = 0;
            transform.Translate(vel);
            
        }

    }
}
