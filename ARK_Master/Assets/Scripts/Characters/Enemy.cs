using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class Enemy : MonoBehaviour {

    public float speed = 25f;
    public bool stop;
    public GameObject playerLocation;
    public Animator animator;

    Room currentRoom;
    Events currentEvent;

    private bool lastX;
    private float range;
    public float health;
	public float timer;

	const float ATTACK_DELAY = 1.5f;
	const double DAMAGE_AMOUNT = 10;


    // Sounds
    //public GameObject attackSound;

    // Use this for initialization
    void Start()
	{
		timer = 1.5f;
        stop = false;
        health = 10;
        range = 10;
        lastX = false;
        animator = GetComponent<Animator>();
        currentEvent    = Generate.instance.currentRoom.roomEvent;
    }

    // Update is called once per frame
    void Update()
    {
        this.timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X))
        {
            Damage(100);
        }

		if (!stop)
        {
			Chase ();
		}

        else
        {
			Attack ();	
		}
    }

	private void Attack()
	{
        //this.timer += Time.deltaTime;

        if (Math.Round(this.timer, 1) >= ATTACK_DELAY)
        {
            animator.SetTrigger("attack");
            Player.instance.Damage(DAMAGE_AMOUNT);
            AlienAttack.instance.PlaySound();
            this.timer = 0;
        }
	}


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Player"))
        {
			Attack ();
            stop = true;
        }

        else if (coll.gameObject.name.Contains("Laser"))
        {
            Damage(2);
        }
    }

	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.name.Contains("Player"))
		{
			stop = false;
            this.timer = 0;
        }
	}	

    public void Damage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            currentEvent.Enemies.Remove(2);
            
            //Check if this was the last enemy alive - if so, end the event
            if (currentEvent.Enemies.Count <= 0)
            {
                StateMachine.instance.EndEvent(currentEvent);
            }
			Destroy(this.gameObject);
        }

    }

    private bool CheckRange()
    {
        bool inRange = false;

        Vector3 playerPos = playerLocation.transform.position;
		//Vector3 playerPos = new Vector3(0,0,-10);

		double x = Math.Round((double)(playerPos.x - transform.position.x), 1);
		double y = Math.Round((double)(playerPos.y - transform.position.y), 1);

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


		double x = Math.Round((double)(playerPos.x - transform.position.x), 1);
		double y = Math.Round((double)(playerPos.y - transform.position.y), 1);

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

            vel.z = 0;
            transform.Translate(vel);
            
        }

    }
}
