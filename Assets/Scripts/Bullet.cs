using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    private bool isDestroyed = false;
    public float bulletSpacing = 0.0f;
    private bool needToMove = true;
    private float amountOfMovement = 0.0f;

    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;

        if(bulletSpacing == 0.0f)
        {
            needToMove = false;
        }
    }

    void Update()
    {
        if(needToMove)
        {
            MoveIfNecessary();
        }
        
    }

    void MoveIfNecessary()
    {
        Vector3 cPosition = transform.position;
        if(bulletSpacing > 0)
        {
            //right
            cPosition += Vector3.right * Time.deltaTime;
        }
        else
        {
            //left
            cPosition += Vector3.left * Time.deltaTime;
        }
        amountOfMovement += Time.deltaTime;
        if(amountOfMovement >= Mathf.Abs(bulletSpacing))
        {
            needToMove = false;
        }

        transform.position = cPosition;
        
    }
    /// <summary>
    /// Deals with collisions
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("Ball") && !isDestroyed)
        {
            coll.gameObject.GetComponent<BallScript>().Split();
        }

        isDestroyed = true;
        Destroy(gameObject);
    }


}
