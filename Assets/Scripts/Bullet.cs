using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{

    private bool isDestroyed = false;
    void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
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
