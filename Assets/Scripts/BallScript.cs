using UnityEngine;
using System.Collections;

/// <summary>
/// Ball Behaviour
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BallScript : MonoBehaviour {

    private Rigidbody m_rigidBody;

    public float startAngle = -1.0f;

    public float forceMultiplier = 100.0f;
    public float startForce = 100.0f;

    public Vector3 cVelocity;
    public Vector3 cAngularVelocity;

    public float blinkingTime = 1.0f;


    // Use this for initialization
    void Start ()
    {
        //Start whith a random left/right movement
        m_rigidBody = this.GetComponent<Rigidbody>();
           
        if(startAngle == -1.0f)
        {
            //Get a random angle
            startAngle = Random.Range(30.0f, 80.0f);
        }
        ApplyForceInAngle(startAngle, startForce);
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Check the velocity to not exceed some values and it's not completely vertical
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Split();
        //}
	}
    /// <summary>
    /// Split the ball in two small balls
    /// Adds some forces to the new ones
    /// </summary>
    public void Split()
    {
        //Split the ball in two small ones and dissapear
        if (this.transform.localScale.x == 0.5)
        {
            //Destroy the ball
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            float nextScale = this.transform.localScale.x * 0.5f;
            if(nextScale > 1.0f)
            {
                nextScale = Mathf.Ceil(nextScale);
            }
            //Calculate new directions
            float randomAngle = Random.Range(45.0f, 60.0f);
            float inverseAngle = 360.0f - randomAngle;

            randomAngle *= Mathf.Deg2Rad;
            inverseAngle *= Mathf.Deg2Rad;

            GameObject ch1 = Instantiate(gameObject, transform.position, Quaternion.identity) as GameObject;
            GameObject ch2 = Instantiate(gameObject, transform.position, Quaternion.identity) as GameObject;

            ch1.transform.localScale = new Vector3(nextScale, nextScale, nextScale);
            ch2.transform.localScale = new Vector3(nextScale, nextScale, nextScale);

            ch1.GetComponent<Rigidbody>().AddForce( new Vector3(Mathf.Sin(randomAngle) * forceMultiplier, Mathf.Cos(randomAngle) * forceMultiplier, 0.0f));
            ch2.GetComponent<Rigidbody>().AddForce( new Vector3(Mathf.Sin(inverseAngle) * forceMultiplier, Mathf.Cos(inverseAngle) * forceMultiplier, 0.0f));

            
        }
    }
    /// <summary>
    /// Method to add a force to the ball in an specific angle in degrees
    /// </summary>
    /// <param name="degreeAngle"></param>
    /// <param name="force"></param>
    public void ApplyForceInAngle(float degreeAngle, float force)
    {
        degreeAngle *= Mathf.Deg2Rad;
        if(m_rigidBody == null)
        {
            m_rigidBody = GetComponent<Rigidbody>();
        }
        m_rigidBody.AddForce(new Vector3(Mathf.Sin(degreeAngle) * force, Mathf.Cos(degreeAngle) * force, 0.0f));
    }
    /// <summary>
    /// Pause the ball
    /// </summary>
    public void Pause(float time)
    {
        if(!m_rigidBody)
        {
            m_rigidBody = this.GetComponent<Rigidbody>();
        }
        cVelocity = m_rigidBody.velocity;
        m_rigidBody.velocity = Vector3.zero;
        cAngularVelocity = m_rigidBody.angularVelocity;
        m_rigidBody.angularVelocity = Vector3.zero;
        m_rigidBody.useGravity = false;

        if(time != -1)
        {
            StartCoroutine(RestartBlinking(time));
        }
    }

    IEnumerator RestartBlinking(float time)
    {
        yield return new WaitForSeconds(time - blinkingTime);
        InvokeRepeating("Blink", 0, 0.2f);
        StartCoroutine(Restart(blinkingTime));
    }

    IEnumerator Restart(float time)
    {
        yield return new WaitForSeconds(time);
        m_rigidBody.velocity = cVelocity;
        m_rigidBody.angularVelocity = cAngularVelocity;
        m_rigidBody.useGravity = true;
    }

    public void Dynamite()
    {
        Debug.Log("BOOOOM");
    }
}
