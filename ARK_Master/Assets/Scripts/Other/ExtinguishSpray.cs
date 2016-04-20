using UnityEngine;
using System.Collections;
using System;

public class ExtinguishSpray : MonoBehaviour {
	public float speed = 0.1f;
	private Rigidbody2D rb;
	public float angle = 0f;
    private float timer = 0f;
    public float lifeSpan = 2f;

	Vector3 start;
	Vector3 direction;

    //public Text output;
    bool distanceFlag = false;


	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		start = this.transform.position;
        ExtinguisherSound.instance.PlaySound();

    }

	///
	/// \brief <b>Brief Description:</b> Add force to the laser on the y-axis.
	///
	void Update()
	{
        this.timer += Time.deltaTime;

        if(Math.Round(this.timer, 1) >= this.lifeSpan)
        {
            Destroy(this.gameObject);
        }

        if (!distanceFlag)
        {
            Vector3 direction = this.direction - this.start;
            direction.Normalize();

            this.rb.velocity = direction * this.speed;
            distanceFlag = true;
        }

        else
        {
            Vector3 direction = this.direction - this.start;
            direction.Normalize();
            this.rb.velocity = direction * 0;
        }
    }


	public void Trajectory(Vector3 v)
	{
		this.direction = v;
	}
}