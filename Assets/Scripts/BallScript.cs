using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {


    private Rigidbody m_rigidBody;
	// Use this for initialization
	void Start () {

        //Start whith a random left/right movement
        m_rigidBody = this.GetComponent<Rigidbody>();
        float velocityX = Random.Range(-5.0f, 5.0f);
        float velocityY = Random.Range(-3.0f, 3.0f);
        m_rigidBody.velocity = new Vector3(velocityX, velocityY, 0.0f);
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Check the velocity to not exceed some values
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Split();
        }
	}

    void Split()
    {
        //Split the ball in two small ones and dissapear
        if (this.transform.localScale.x == 0.5)
        {
            //Destroy the ball
            Destroy(this.gameObject);
        }
        else
        {
            
            float nextScale = this.transform.localScale.x * 0.5f;
            if(nextScale > 1.0f)
            {
                nextScale = Mathf.Ceil(nextScale);
            }
            //Calculate new directions
            float xDir = Random.Range(1.0f, 5.0f);
            float yDir = Random.Range(10.0f, 15.0f);

            GameObject ch1 = Instantiate(this.gameObject, this.transform.position, Quaternion.identity) as GameObject;
            GameObject ch2 = Instantiate(this.gameObject, this.transform.position, Quaternion.identity) as GameObject;

            ch1.transform.localScale = new Vector3(nextScale, nextScale, nextScale);
            ch2.transform.localScale = new Vector3(nextScale, nextScale, nextScale);

            ch1.GetComponent<Rigidbody>().velocity = new Vector3(-xDir, yDir, 0.0f);
            ch2.GetComponent<Rigidbody>().velocity = new Vector3(xDir, yDir, 0.0f);

            Destroy(this.gameObject);
        }
    }
}
