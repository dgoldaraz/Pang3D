using UnityEngine;
using System.Collections;

public class DoctorMovement : MonoBehaviour {


    public bool isMovingLeft = true;
    public Animator anim;


    // Use this for initialization
    void Start ()
    {
        anim.Play("Stop");
        //Decide randomly if the doctor is going to move
        if(Random.value > 0.5f)
        {
            //move
            if(isMovingLeft)
            {
                anim.SetBool("left", true);
            }
            else
            {
                anim.SetBool("right", true);
            }
            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
