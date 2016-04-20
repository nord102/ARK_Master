using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// \class FireTheLasers
/// \brief This class is responsible for management of firing the laser.
///
public class FireTheLasers : MonoBehaviour
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

        Physics2D.IgnoreLayerCollision(9, 10);
		LaserSound.instance.PlaySound ();

    }

    ///
    /// \brief <b>Brief Description:</b> Destory the object if it hits the roof.
    ///
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }

    ///
    /// \brief <b>Brief Description:</b> Add force to the laser on the y-axis.
    ///
    void Update()
    {    
		Vector3 direction = this.direction - this.start;
		direction.Normalize ();

		this.rb.velocity = direction * this.speed;
    }


    public void Trajectory(Vector3 v)
    {
        this.direction = v;
    }

}

