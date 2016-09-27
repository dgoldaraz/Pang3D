using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {


    public Transform paddle;
    public Transform lowTransform;
    public Transform highTransform;

    public enum Position { Up, Down };
    public Position currentPosition;
    public float speed = 3.0f;

    private bool m_isMoving = false;
    private Position destination = Position.Down;

	// Use this for initialization
	void Start ()
    {
	    if(paddle == null || lowTransform == null || highTransform ==null)
        {
            foreach(Transform t in transform)
            {
                if(t.gameObject.name == "Paddle" && paddle == null)
                {
                    paddle = t;
                    continue;
                }
                if (t.gameObject.name == "HighPosition" && highTransform == null)
                {
                    highTransform = t;
                    continue;
                }
                if (t.gameObject.name == "LowPosition" && lowTransform == null)
                {
                    lowTransform = t;
                    continue;
                }
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public bool isMoving()
    {
        return m_isMoving;
    }

    /// <summary>
    /// Set the move of the paddle high
    /// </summary>
    public void GoUp()
    {
        if(currentPosition != Position.Up && !m_isMoving)
        {
            //Move up
            m_isMoving = true;
            destination = Position.Up;
        }
    }
    /// <summary>
    /// Set the move of the paddle low
    /// </summary>
    public void GoDown()
    {
        if(currentPosition != Position.Down && !m_isMoving)
        {
            //Move down
            m_isMoving = true;
            destination = Position.Down;
        }
    }
    /// <summary>
    /// Moves the paddle
    /// </summary>
    void Move()
    {
        //Update the paddle if neccesary
        if(m_isMoving)
        {
            Vector3 pos = paddle.position;
            switch (destination)
            {
                case Position.Up:
                    {
                        if(pos.y != highTransform.position.y)
                        {
                            //Move up
                            pos += Vector3.up * speed * Time.deltaTime;
                        }
                        else
                        {
                            m_isMoving = false;
                            currentPosition = Position.Up;
                        }
                        break;
                    }
                case Position.Down:
                    {
                        if (pos.y != lowTransform.position.y)
                        {
                            //Move Down
                            pos += Vector3.down * speed * Time.deltaTime;
                        }
                        else
                        {
                            m_isMoving = false;
                            currentPosition = Position.Down;
                        }
                        break;
                    }
            }

            pos.y = Mathf.Clamp(pos.y, lowTransform.position.y, highTransform.position.y);
            paddle.position = pos;
        }
    }
}
