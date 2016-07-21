using UnityEngine;
using System.Collections;

public class Peephole : MonoBehaviour {

    //This method draws a sphere for define a position
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
