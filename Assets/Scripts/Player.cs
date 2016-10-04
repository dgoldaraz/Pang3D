using UnityEngine;
using System.Collections;
/// <summary>
/// Player Behaviour
/// </summary>
/// 

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{

    public delegate void PlayerHit(GameObject player);
    public static event PlayerHit onPlayerHit;

    public delegate void PlayerAddLive(GameObject player);
    public static event PlayerAddLive onPlayerAddLive;

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

    public int m_lives = 3;

    public GameObject weapon;
    public GameObject peepHole;

    private bool m_canShoot = true;

    public GameObject beltModel;
    public GameObject shield;

    public Color playerColor = Color.blue;

    private bool m_shieldOn = false;
    //We need to use this variable to avoid multiple collisions at the same time
    private bool m_isDestroyed = false;

    private enum Direction { Left, Right }
    private Direction m_lastDirection = Direction.Left;

    public float diagonalOffset = 0.2f;
    public float diagonalAngle = 45.0f;

    public ParticleSystem shootParticleSystem;
    private AudioSource m_audiSource;
    public AudioClip basicShootSound;


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

        beltModel.GetComponent<MeshRenderer>().material.color = playerColor;
        MeshRenderer shieldMeshRender = shield.GetComponent<MeshRenderer>();
        Color shieldColor = Color.Lerp(playerColor, Color.white, 0.3f);
        shieldColor.a = shieldMeshRender.material.color.a;
        shieldMeshRender.material.color = shieldColor;
        shieldMeshRender.enabled = false;
        m_isDestroyed = false;
        m_audiSource = GetComponent<AudioSource>();
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
            m_lastDirection = Direction.Right;
        }
        else if (Input.GetKey(leftKey))
        {
            //Left
            movement = true;
            newPos += Vector3.left * speed * Time.deltaTime;
            m_lastDirection = Direction.Left;
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

        if (Input.GetKeyDown(shootKey) && m_canShoot)
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
            GameObject go = Instantiate(weapon, peepHole.transform.position, weapon.transform.rotation) as GameObject;
            go.GetComponent<Weapon>().Shoot(gameObject, peepHole.transform.position);

            if (shootParticleSystem && shootParticleSystem.isStopped)
            {
                shootParticleSystem.Play();
            }
            m_audiSource.Play();
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
        if (onPlayerAddLive != null)
        {
            onPlayerAddLive(gameObject);
        }
    }
    /// <summary>
    /// Deals with the collisions. Mainly it can be or an item or a ball or elevator
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Ball") && !m_isDestroyed)
        {
            if(m_shieldOn)
            {
                //Avoid the hit by the shield!
                setShield(false);

            }
            else
            {
                
                m_isDestroyed = true;
                m_lives--;
                Debug.Log("Dead, you have " + m_lives);
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
        if (coll.gameObject.CompareTag("Paddle") && m_elevator && !m_elevator.isMoving())
        {
            //Get the elevator
            m_elevator = null;
        }
    }

    public void setCanShoot(bool s)
    {
        m_canShoot = s;
    }
    /// <summary>
    /// Sets the weapon that the user will use
    /// </summary>
    /// <param name="w"></param>
    public void setWeapon(GameObject w)
    {
        weapon = w;
    }

    /// <summary>
    /// Return the colour of the player
    /// </summary>
    /// <returns></returns>
    public Color getPlayerColor()
    {
        return playerColor;
    }

    /// <summary>
    /// Set Shield method
    /// </summary>
    /// <param name="s"></param>
    public void setShield(bool s)
    {
        if(m_shieldOn != s)
        {
            shield.GetComponent<MeshRenderer>().enabled = s;
            m_shieldOn = s;
        }
    }

    public float getDiagonalOffset()
    {
        if(m_lastDirection == Direction.Right)
        {
            //Return right PeepHole
            return diagonalOffset;
        }
        else
        {
            //Return left PeepHole
            return -diagonalOffset;
        }
    }

    public float getDiagonalAngle()
    {
        if (m_lastDirection == Direction.Right)
        {
            //Return right PeepHole
            return -diagonalAngle;
        }
        else
        {
            //Return left PeepHole
            return diagonalAngle;
        }
    }
    /// <summary>
    /// Set the sound to the shooting
    /// </summary>
    /// <param name="sound"></param>
    public void setAudioShoot(AudioClip sound)
    {
        m_audiSource.clip = sound;
    }
    /// <summary>
    /// Set a basic sound for shooting
    /// </summary>
    public void resetShootSound()
    {
        m_audiSource.clip = basicShootSound;
    }
    /// <summary>
    /// Play the given sound
    /// </summary>
    /// <param name="sound"></param>
    public void playSound(AudioClip sound)
    {
        m_audiSource.PlayOneShot(sound);
    }
}
