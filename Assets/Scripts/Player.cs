using UnityEngine;
using System.Collections;
/// <summary>
/// Player Behaviour
/// </summary>
/// 
public class Player : MonoBehaviour {

    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode shootKey;

    public float speed = 6.0f;
    public float padding = 2.5f;


    private float m_xmax;
    private float m_xmin;

    enum LadderState {Top, Bottom, Middle, None };
    private LadderState m_ladderState = LadderState.None;


	// Use this for initialization
	void Start () {

        //Calculate the limits of the viewport depending on the camera
        float distance = transform.position.z - Camera.main.transform.position.z; 
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        //Apply a padding of the walls
        m_xmax = rightMost.x - padding;
        m_xmin = leftMost.x + padding;
    }
	
	// Update is called once per frame
	void Update ()
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

        if(m_ladderState != LadderState.Middle && Input.GetKey(rightKey))
        {
            //Right
            movement = true;
            newPos += Vector3.right * speed * Time.deltaTime;
        }
        else if(m_ladderState != LadderState.Middle && Input.GetKey(leftKey))
        {
            //Left
            movement = true;
            newPos += Vector3.left * speed * Time.deltaTime;
        }
        else if(m_ladderState != LadderState.None)
        {
            //We are in a ladder
            if (m_ladderState != LadderState.Top && Input.GetKey(upKey))
            {
                //Go up 
                movement = true;
                newPos += Vector3.up * speed * Time.deltaTime;
            }
            else if (m_ladderState != LadderState.Bottom && Input.GetKey(downKey))
            {
                //Gop down
                movement = true;
                newPos += Vector3.down * speed * Time.deltaTime;
            }
        }
       
        if(movement)
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
        Debug.Log("Shoot");
    }
}
