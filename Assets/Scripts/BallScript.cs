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

    private Vector3 cVelocity;
    private Vector3 cAngularVelocity;

    public float blinkingTime = 1.0f;

    private new MeshRenderer renderer;
    private bool m_isOnPause = false;

    private bool m_OnDinamite = false;
    private float m_timeToSplit = 0.0f;
    private float m_time = 0.0f;
    private float m_pauseTime = 0.0f;

    private bool m_restartWithForce = false;

    public GameObject ItemWhenSplit;
    [Range(0f,1f)]
    public float randomPercentage = 0.3f; //30% times an item appears
    public Vector3 ballGravity;
    private bool m_useGravity = true;

    public ParticleSystem splitPS;


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

        if(startAngle != 0f && !m_isOnPause)
        {
            ApplyForceInAngle(startAngle, forceMultiplier);
        }

        renderer = GetComponent<MeshRenderer>();
        if(splitPS == null)
        {
            splitPS = GetComponentInChildren<ParticleSystem>();
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(m_useGravity)
        {
            m_rigidBody.AddForce(ballGravity * m_rigidBody.mass);
        }
        
    }

    void Update()
    {

    }
    /// <summary>
    /// Split the ball in two small balls
    /// Adds some forces to the new ones
    /// </summary>
    public void Split()
    {
        //Split the ball in two small ones and dissapear
        if (this.transform.localScale.x == 0.25)
        {
            //Destroy the ball
            if(!m_OnDinamite)
            {
                //Destroy the object if we are not in dinamiteState
                DestroyBall();
                //Ask the gameManager to check if there are more balls
                FindObjectOfType<GameManager>().CheckEndGame();
            }
            else
            {
                m_OnDinamite = false;
            }
        }
        else
        {
            //Stop current coroutines

            StopAllCoroutines();
            
            float nextScale = this.transform.localScale.x * 0.5f;

            if(nextScale > 1.0f)
            {
                nextScale = Mathf.Ceil(nextScale);
            }

            //Calculate new directions
            float randomAngle = Random.Range(35.0f,55.0f);
            float inverseAngle = -randomAngle;

            //We move the next pair of balls up a little
            Vector3 offset = new Vector3(0.0f, nextScale, 0.0f);

            GameObject ch1 = Instantiate(gameObject, transform.position + offset, Quaternion.identity) as GameObject;
            GameObject ch2 = Instantiate(gameObject, transform.position + offset, Quaternion.identity) as GameObject;

            ch1.transform.localScale = new Vector3(nextScale, nextScale, nextScale);
            ch2.transform.localScale = new Vector3(nextScale, nextScale, nextScale);

            BallScript ch1BallScript = ch1.GetComponent<BallScript>();
            BallScript ch2BallScript = ch2.GetComponent<BallScript>();

            DestroyBall();

            if (m_isOnPause)
            {
                Vector3 offsetPos = new Vector3(nextScale, 0.0f, 0.0f);

                //Set a new position with and object dependant of the scale
                Vector3 ball1Position = transform.position + offsetPos;
                Vector3 ball2Position = transform.position - offsetPos;
                ch1.transform.position = ball1Position;
                ch2.transform.position = ball2Position;

                //get the next time and pause the balls
                float remainTime = Time.time - m_time;
                remainTime = remainTime % 60.0f; //seconds
                remainTime = m_pauseTime - remainTime;
                ch1BallScript.PauseWithForce(remainTime, randomAngle);
                ch2BallScript.PauseWithForce(remainTime, inverseAngle);
            }
            else
            {
                //Normal split
                ch1BallScript.startAngle = randomAngle;
                ch2BallScript.startAngle = inverseAngle;
                ch1BallScript.ballGravity = ballGravity + new Vector3(0.0f, -1.0f, 0.0f);
                ch2BallScript.ballGravity = ballGravity + new Vector3(0.0f, -1.0f, 0.0f);
            }

            if (m_OnDinamite)
            {
                ch1BallScript.Dynamite(m_timeToSplit);
                ch2BallScript.Dynamite(m_timeToSplit);
            }


            if(Random.value <= randomPercentage)
            {
                Instantiate(ItemWhenSplit, transform.position, Quaternion.identity);
            }

        }

        FindObjectOfType<GameManager>().addPoints(25);
       
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

        m_time = Time.time;
        m_pauseTime = time;

        if (m_isOnPause)
        {
            //We are on pause yet, stop coroutines and restart
            StopAllCoroutines();
            CancelInvoke("Blink");
            renderer.enabled = true;
        }
        else
        {
            m_isOnPause = true;
            cVelocity = m_rigidBody.velocity;
            m_rigidBody.velocity = Vector3.zero;
            cAngularVelocity = m_rigidBody.angularVelocity;
            m_rigidBody.angularVelocity = Vector3.zero;
            m_useGravity = false;
        }

        if(time != -1)
        {
            StartCoroutine(RestartBlinking(time));
        }
    }

    /// <summary>
    /// This Pause mthod is only use when we want to apply an specific force when the object restarts
    /// </summary>
    /// <param name="time"></param>
    /// <param name="angle"></param>
    void PauseWithForce(float time, float angle)
    {
        m_restartWithForce = true;
        startAngle = angle;
        Pause(time);
    }
    /// <summary>
    /// Start to blinck
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator RestartBlinking(float time)
    {
        yield return new WaitForSeconds(time - blinkingTime);
        InvokeRepeating("Blink", 0, 0.2f);
        StartCoroutine(Restart(blinkingTime));
    }

    /// <summary>
    /// Make Blink effect
    /// </summary>
    void Blink()
    {
        if(!renderer)
        {
            renderer = GetComponent<MeshRenderer>();
        }
        renderer.enabled = !renderer.enabled;
    }

    /// <summary>
    /// Start to moving again
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Restart(float time)
    {
        yield return new WaitForSeconds(time);

        m_useGravity = true;
        if (!m_restartWithForce)
        {
            m_rigidBody.velocity = cVelocity * 1.5f;
            m_rigidBody.angularVelocity = cAngularVelocity;
        }
        else
        {
            m_rigidBody.AddForce(new Vector3(Mathf.Sin(startAngle) * forceMultiplier, Mathf.Cos(startAngle) * forceMultiplier, 0.0f));
        }
        CancelInvoke("Blink");
        m_isOnPause = false;
    }

    /// <summary>
    /// Set the dynamite mode
    /// </summary>
    /// <param name="timeToSplit"></param>
    public void Dynamite(float timeToSplit)
    {
        m_timeToSplit = timeToSplit;
        m_OnDinamite = true;
        StartCoroutine(SplitOnTime());
    }

    IEnumerator SplitOnTime()
    {
        yield return new WaitForSeconds(m_timeToSplit);
        Split();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Shield"))
        {
            RaycastHit hit;
            if(m_rigidBody)
            {
                if (Physics.Raycast(transform.position, m_rigidBody.velocity.normalized, out hit))
                {
                    m_rigidBody.AddForce(hit.point.normalized * 5.0f, ForceMode.Impulse);
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        Vector3 pos = transform.position;
        float angle = startAngle;
        angle *= Mathf.Deg2Rad;

        Vector3 dest = new Vector3(Mathf.Sin(angle) * forceMultiplier, Mathf.Cos(angle) * forceMultiplier, 0.0f);

        Gizmos.DrawLine(pos, dest);
    }

    void SplitParticles()
    {
        if (splitPS && splitPS.isStopped)
        {
            splitPS.gameObject.transform.position = transform.position;
            splitPS.gameObject.transform.localScale = transform.localScale;
            splitPS.startColor = GetComponent<Renderer>().material.color;
            splitPS.Play();
        }
    }

    void DestroyBall()
    {
        SplitParticles();
        GetComponent<MeshRenderer>().enabled = false;
        Destroy(this.gameObject, 0.5f);
    }
}
