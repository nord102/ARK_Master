using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public static Player instance = null;
    private const int SPEED = 10;
    public int laserQty = 100;
    private GameObject laserClone;
    public GameObject laser;
    public Text output;
    public Camera cam;

    public Text positionOutput;

    private int health;
    private int mana;

    public Animator animator;
    private float lastDirection = 0;

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
        health = 10;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = 0;
        float vertical = 0;


        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        if (horizontal != lastDirection && horizontal != 0)
        {
            animator.SetTrigger("direction");
            lastDirection = horizontal;
        }


        vertical = (int)(Input.GetAxisRaw("Vertical"));


        Vector3 pos = new Vector3(this.gameObject.transform.position.x + horizontal / SPEED, this.gameObject.transform.position.y + vertical / SPEED);


        this.gameObject.transform.position = pos;

        if (Input.GetButtonDown("Fire1"))
        {
            FireTheLaser();
        }

        this.positionOutput.text = this.transform.position.ToString();


        
    }

    public void Damage(int amount)
    {
        health -= amount;

        if (health == 0)
        {
            gameObject.SetActive(false);
        }
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
        if (laserQty > 0)
        {
            //Destroy(laserClone);
            Vector3 temp = Input.mousePosition;
            temp.z = -10;

            Vector3 c = cam.ScreenToWorldPoint(temp);

            c.x *= -1;
            c.y *= -1;
            c.z = 0;
            output.text = c.ToString();


            laserQty--;
            //laserText.text = "Laser : " + laserQty.ToString();
            laserClone = Instantiate(laser, this.gameObject.transform.position, Quaternion.identity) as GameObject;
            laserClone.SendMessage("Trajectory", c);
            //laserClone.SendMessage("Trajectory", cam);


        }
    }


}
