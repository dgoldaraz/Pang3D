using UnityEngine;
using System.Collections;

public class HookWeapon : Weapon {

    private bool m_move = false;
    private GameObject m_player;
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

    // Use this for initialization
    void Start ()
    {
        m_lastPart = gameObject;
        m_capsuleCollider = GetComponent<CapsuleCollider>();
        m_partHeight = m_capsuleCollider.height * transform.localScale.y;
        m_initialHeight = m_capsuleCollider.height;
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

    void Move()
    {
        if(m_move)
        {
            Vector3 newPos = transform.position;
            Vector3 movement = Vector3.up * speed * Time.deltaTime;
            newPos += movement;
            m_amount += movement.y;
            //if the amount of movement it's bigger than our internal meusre, create a new part
            if(m_amount >= m_partHeight)
            {
                CreatePart();
            }
            this.transform.position = newPos;
        }
    }

    void CreatePart()
    {
        //Create a new object and reparent to the last part created
        GameObject newPart = Instantiate(part, m_initialPosition, Quaternion.identity) as GameObject;
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

    void OnCollisionEnter(Collision coll)
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
}
