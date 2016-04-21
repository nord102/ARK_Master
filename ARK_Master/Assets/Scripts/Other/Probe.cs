using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class Probe : MonoBehaviour {

    public float speed = 30f;

    private Rigidbody2D rb;
    public float angle = 0f;

    Vector3 start;
    Vector3 direction;

    private float timer = 0f;

    //public Text output;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Debug.Log(this.transform.position);
        start = this.transform.position;
    }

    ///
    /// \brief <b>Brief Description:</b> Destory the object if it hits the roof.
    ///
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name.Contains("Player"))
        {
            AI.inSight = true;
            //Debug.Log("Hit");
            Destroy(this.gameObject);
        }

        else if (coll.gameObject.name.Contains("Sinister") == false)
        {
            AI.inSight = false;
            Destroy(this.gameObject);
        }
    }

    ///
    /// \brief <b>Brief Description:</b> Add force to the laser on the y-axis.
    ///
    void Update()
    {
        this.timer += Time.deltaTime;

        Vector3 direction = this.direction - this.start;
        direction.Normalize();
        this.rb.velocity = direction * this.speed;

        if (this.timer >= 5)
        {
            Destroy(this.gameObject);
        }
    }


    public void Trajectory(Vector3 v)
    {
        this.direction = v;
    }
}
