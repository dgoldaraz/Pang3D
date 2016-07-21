using UnityEngine;
using System.Collections;
/// <summary>
/// Player Behaviour
/// </summary>
/// 
public class Player : MonoBehaviour
{

    public delegate void PlayerHit(GameObject player);
    public static event PlayerHit onPlayerHit;

    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode shootKey;

    public float speed = 6.0f;
    public float padding = 2.5f;


    private float m_xmax;
    private float m_xmin;

    private Elevator m_elevator = null;

    private int m_lives = 3;

    public GameObject weapon;
    public GameObject peepHole;


    // Use this for initialization
    void Start()
    {

        //Calculate the limits of the viewport depending on the camera
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        //Apply a padding of the walls
        m_xmax = rightMost.x - padding;
        m_xmin = leftMost.x + padding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    /// <summary>
    /// Function that deals with the movement of the player
    /// </summary>
    void Move()
    {
        Vector3 newPos = transform.position;
        bool movement = false;

        if (Input.GetKey(rightKey))
        {
            //Right
            movement = true;
            newPos += Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetKey(leftKey))
        {
            //Left
            movement = true;
            newPos += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(upKey))
        {
            if (m_elevator != null)
            {
                m_elevator.GoUp();
            }
        }
        else if (Input.GetKey(downKey))
        {
            if (m_elevator != null)
            {
                m_elevator.GoDown();
            }
        }

        if (movement)
        {
            newPos.x = Mathf.Clamp(newPos.x, m_xmin, m_xmax);
            this.transform.position = newPos;
        }

        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
        }
    }
    /// <summary>
    /// This method handles with the Shoot actions
    /// Decide if the player can shoot, and wich weapon it's using
    /// </summary>
    void Shoot()
    {
        if(weapon)
        {
            GameObject go = Instantiate(weapon, peepHole.transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<Weapon>().Shoot(gameObject);
        }
       
    }
    /// <summary>
    /// set the number of lives for the player
    /// </summary>
    /// <param name="nLives"></param>
    public void setLives(int nLives)
    {
        m_lives = nLives;
    }
    /// <summary>
    /// Get the number of livesin the player
    /// </summary>
    /// <returns></returns>
    public int getLives()
    {
        return m_lives;
    }
    /// <summary>
    /// Add one live to the player
    /// </summary>
    public void addLives()
    {
        m_lives++;
    }
    /// <summary>
    /// Deals with the collisions. Mainly it can be or an item or a ball or elevator
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Dead, you have " + m_lives);
            m_lives--;
            if (onPlayerHit != null)
            {
                onPlayerHit(gameObject);
            }

            if (m_lives == 0)
            {
                //disappear
                Destroy(gameObject);
            }
        }
        if (coll.gameObject.CompareTag("Paddle"))
        {
            //Get the elevator
            m_elevator = coll.gameObject.GetComponentInParent<Elevator>();
        }
    }

    /// <summary>
    ///  Deals with the exit state of a collision
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.CompareTag("Paddle"))
        {
            //Get the elevator
            m_elevator = null;
        }
    }
}
