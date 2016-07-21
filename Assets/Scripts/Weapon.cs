using UnityEngine;
using System.Collections;
/// <summary>
/// This class defines a weapon type and how it shooths
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Weapon : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public virtual void Shoot(GameObject player, Vector3 initPos)
    {
        Debug.Log(player.name + " Shoots me");
    }
}
