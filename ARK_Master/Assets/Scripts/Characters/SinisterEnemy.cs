using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public static class AI
{
    public static bool inSight = false;
}

public class SinisterEnemy : MonoBehaviour
{

    public float speed = 25f;
    public bool stop;
    public GameObject playerLocation;

    private GameObject laserClone;
    public GameObject laser;

    private GameObject probeClone;
    public GameObject probe;


    public Animator animator;

    Room currentRoom;
    Events currentEvent;

    private bool lastX;
    private float range;
    public float health;
    public float timer;

    public float probeTimer;

    const float ATTACK_DELAY = 1.5f;
    const double DAMAGE_AMOUNT = 10;

    const float PROBE_TIME = 1.0f;


    // Sounds
    //public GameObject attackSound;

    // Use this for initialization
    void Start()
    {
        timer = 1.5f;
        stop = false;
        health = 20;
        range = 1;
        lastX = false;
        animator = GetComponent<Animator>();
        currentEvent = Generate.instance.currentRoom.roomEvent;

        probeTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.timer += Time.deltaTime;


        ProbeInRange();

        if (AI.inSight)
        {
            FireTheLaser();
            AI.inSight = false;
        }

        if (!stop)
        {
            Chase();
        }
    }

    private void ProbeInRange()
    {
        this.probeTimer += Time.deltaTime;

        if (Math.Round(this.probeTimer, 1) >= PROBE_TIME)
        {
            Vector3 c = playerLocation.transform.position;
            c.z = 0;

            this.probeTimer = 0f;

            probeClone = Instantiate(probe, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            probeClone.SendMessage("Trajectory", c);
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Contains("Player"))
        {
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
            currentEvent.Enemies.Remove(3);

            //Check if this was the last enemy alive - if so, end the event
            if (currentEvent.Enemies.Count <= 0)
            {
                StateMachine.instance.EndEvent(currentEvent);
            }
            Destroy(this.gameObject);
        }

    }

    internal void FireTheLaser()
    {
        Vector3 c = playerLocation.transform.position;
        c.z = 0;
        laserClone = Instantiate(laser, this.gameObject.transform.position, Quaternion.identity) as GameObject;
        laserClone.SendMessage("Trajectory", c);
    }


    private void Chase()
    {
        Vector3 vel = new Vector3();
        bool currentX = false;

        Vector3 playerPos = playerLocation.transform.position;
        //Vector3 playerPos = new Vector3(0,0,-10);


        double x = Math.Round((double)(playerPos.x - transform.position.x), 1);
        double y = Math.Round((double)(playerPos.y - transform.position.y), 1);

            if (x < 0)
            {
                vel.x -= speed;
                currentX = true;
                animator.SetBool("sinisterDirection", true);
            }

            else if (x > 0)
            {
                vel.x += speed;
                currentX = false;
                animator.SetBool("sinisterDirection", false);

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
