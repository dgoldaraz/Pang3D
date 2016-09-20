using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(CapsuleCollider))]
public class HookWeapon : Weapon {

    protected bool m_move = false;
    protected GameObject m_player;
    private float m_amount = 0.0f;

    private GameObject m_lastPart = null;
    private float m_partHeight;
    private Vector3 m_initialPosition;
    private CapsuleCollider m_capsuleCollider;

    public float speed = 3.0f;
    public GameObject part = null;/// Object part to instiantiate

    public float spacing = 1;

    private float m_initialHeight;

    private bool isDestroyed = false;

    List<GameObject> m_partList;

    private Vector3 m_shootDirection = Vector3.up;

    // Use this for initialization
    void Start ()
    {
        m_lastPart = gameObject;
        m_capsuleCollider = GetComponent<CapsuleCollider>();
        m_partHeight = m_capsuleCollider.height * transform.localScale.y;
        m_initialHeight = m_capsuleCollider.height;
        m_partList = new List<GameObject>();
        GetComponent<Rigidbody>().useGravity = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
	} 

    /// <summary>
    /// Start to move/shoot the object
    /// </summary>
    /// <param name="player"></param>
    public override void Shoot(GameObject player, Vector3 initPos)
    {
        Debug.Log(player.name + " Shoots hook");
        m_move = true;
        m_player = player;
        m_initialPosition = initPos;
        m_player.GetComponent<Player>().setCanShoot(false);
    }

    /// <summary>
    /// sets if the hook is moving or not
    /// </summary>
    /// <param name="m"></param>
    public void setMovement(bool m)
    {
        m_move = m;
    }

    /// <summary>
    /// Move the hook up
    /// </summary>
    public void Move()
    {
        if(m_move)
        {
            Vector3 newPos = transform.position;
            Vector3 movement = m_shootDirection * speed * Time.deltaTime;
            newPos += movement;
            PartMovement(movement);
            this.transform.position = newPos;
        }
    }

    private void PartMovement(Vector3 movement)
    {
        m_amount += movement.y;
        //if the amount of movement it's bigger than our internal meusre, create a new part
        if (m_amount >= m_partHeight)
        {
            CreatePart();
        }
    }

    protected void setShootDirection(Vector3 newDirection)
    {
        m_shootDirection = newDirection;
    }

    /// <summary>
    /// Create a part to simulate a chain, 
    /// Updates the capculecollider to split balls
    /// </summary>
    void CreatePart()
    {
        //Create a new object and reparent to the last part created
        GameObject newPart = Instantiate(part, m_initialPosition, Quaternion.identity) as GameObject;
        m_partList.Add(newPart);
        newPart.transform.parent = m_lastPart.transform;
        m_lastPart = newPart;
        float heightToAdd = newPart.GetComponent<CapsuleCollider>().height;
        m_partHeight = heightToAdd * transform.localScale.y * spacing;
        float newHeight = Mathf.Abs(transform.position.y - m_initialPosition.y) * (1 / transform.localScale.y);
        m_capsuleCollider.height = newHeight + m_initialHeight * 0.5f + newPart.GetComponent<CapsuleCollider>().height * 0.5f;
        Vector3 newCenter = m_capsuleCollider.center;
        newCenter.y = -newHeight * 0.5f;
        m_capsuleCollider.center = newCenter;
        m_amount = 0.0f;
        
    }

    /// <summary>
    /// Deals with collisitons
    /// </summary>
    /// <param name="coll"></param>
    protected virtual void OnCollisionEnter(Collision coll)
    {
        if(m_player)
        {
            m_player.GetComponent<Player>().setCanShoot(true);
        }
        if (coll.gameObject.CompareTag("Ball") && !isDestroyed)
        {
            coll.gameObject.GetComponent<BallScript>().Split();
        }
        isDestroyed = true;
        Destroy(gameObject);
    }
    /// <summary>
    /// Returns the parts created
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getParts()
    {
        return m_partList;
    }
}
