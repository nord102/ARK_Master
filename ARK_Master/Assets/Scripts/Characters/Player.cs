using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
	const int LASER_EQUIPED = 1;
	const int FIRE_EXTINGUSHER_EQUIPED = 2;

    public static Player instance = null;
    private const int SPEED = 10;
    public int laserQty = 100;
    private GameObject laserClone;
    public GameObject laser;
    //public Text output;
    public Camera cam;

    //public Text positionOutput;
    public GameObject fireExtingusiher;
    private GameObject fireExtingusiherClone;


    private int health;
    private int mana;

    public Animator animator;
    private float lastDirection = 0;

	public int equiped = LASER_EQUIPED;

    //public GameObject alien;
    //public GameObject alienClone;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }


    // Use this for initialization
    void Start()
    {
        health = 100;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if(StateMachine.instance != null && !StateMachine.instance.PlayerControl)
        {
			return;
        }

        float horizontal = 0;
        float vertical = 0;


        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
		vertical = (int)(Input.GetAxisRaw("Vertical"));

		if (horizontal == 0 && vertical == 0) {
			animator.SetBool ("standStatus", true);
		} else {
			animator.SetBool ("standStatus", false);

			if (horizontal < 0) {
				animator.SetBool ("direction", true);
			} else if (horizontal > 0){
				animator.SetBool ("direction", false);
			}
		
		}
			
        Vector3 pos = new Vector3(this.gameObject.transform.position.x + horizontal / SPEED, this.gameObject.transform.position.y + vertical / SPEED);
        this.gameObject.transform.position = pos;

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			equiped = LASER_EQUIPED;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			equiped = FIRE_EXTINGUSHER_EQUIPED;
		}

		if (Input.GetButtonDown ("Fire1")) {

			switch (equiped) {

			case LASER_EQUIPED:
				FireTheLaser ();
				break;

			case FIRE_EXTINGUSHER_EQUIPED:
				UseExtinguisher();
				break;
			}
		}
    }

    private void UseExtinguisher()
    {
        //Destroy(laserClone);
        Vector3 temp = Input.mousePosition;
        temp.z = -10;

        Vector3 c = cam.ScreenToWorldPoint(temp);

        //c.x *= -1;
        //c.y *= -1;
        c.z = 0;

        //Vector3 position = ;
        float angle = Vector3.Angle (this.gameObject.transform.position, c);
       // float angle = Quaternion.Angle(transform.rotation, );
        //Vector3 position = (Vector3)Quaternion.AngleAxis (angle, this.gameObject.transform.position);
        //Vector3.

        fireExtingusiherClone = Instantiate(fireExtingusiher,  this.gameObject.transform.position, Quaternion.identity) as GameObject;
        //fireExtingusiherClone.SendMessage("Trajectory", angle);
        fireExtingusiherClone.SetActive(true);

        //fireExtingusiherClone = null;
    }

    public void Damage(double amount)
    {
		//health -= amount;
		StateMachine.instance.pInfo.SetHealth(-amount);
    }

    ///
    /// \brief <b>Brief Description:</b> This method takes the position of the paddle as a method and then fires the laser if there
    ///  is enough in stock. 
    ///
    /// \param Vector3<b>pos</b> - The position of the paddle.
    ///
    ///
    internal void FireTheLaser()
    {
        //if (laserQty > 0)
        //{
            //Destroy(laserClone);
            Vector3 temp = Input.mousePosition;
            temp.z = -10;

            Vector3 c = cam.ScreenToWorldPoint(temp);

            //c.x *= -1;
            //c.y *= -1;
            c.z = 0;
            // output.text = c.ToString();


            laserQty--;
            //laserText.text = "Laser : " + laserQty.ToString();
            laserClone = Instantiate(laser, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            laserClone.SendMessage("Trajectory", c);
            //laserClone.SendMessage("Trajectory", cam);


        //}
    }


}
