using UnityEngine;
using System.Collections;

public class SinisterLaser : MonoBehaviour
{

    public float speed = 30f;

    private Rigidbody2D rb;
    public float angle = 0f;

    Vector3 start;
    Vector3 direction;

    //public Text output;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Debug.Log(this.transform.position);
        start = this.transform.position;

        //Physics2D.IgnoreLayerCollision(9, 10);
        LaserSound.instance.PlaySound();

    }
    ///
    /// \brief <b>Brief Description:</b> Destory the object if it hits the roof.
    ///
    void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag != "Player" && coll.gameObject.name.Contains("Sinister") == false && coll.gameObject.name.Contains("Laser") == false)
        {
            Destroy(this.gameObject);
        }

        else if (coll.gameObject.tag == "Player")
        {
            //Debug.Log("DAMAGE");
            Player.instance.Damage(10f);       
        }

        //Debug.Log(coll.gameObject.name);
    }

    ///
    /// \brief <b>Brief Description:</b> Add force to the laser on the y-axis.
    ///
    void Update()
    {
        Vector3 direction = this.direction - this.start;
        direction.Normalize();

        this.rb.velocity = direction * this.speed;
    }


    public void Trajectory(Vector3 v)
    {
        this.direction = v;
    }
}
