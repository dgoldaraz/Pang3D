using UnityEngine;
using System.Collections;

public class MachineGunWeapon : Weapon {

    public GameObject bullet;
    public float bulletSpacing = 1.0f;
    public float speed = 6.0f;

    /// <summary>
    /// Create two bullets and set its velocity
    /// </summary>
    /// <param name="player"></param>
    /// <param name="initPos"></param>
    public override void Shoot(GameObject player, Vector3 initPos)
    {
        //Create two bullets and moveUp
        Vector3 bPos1 = initPos;
        bPos1.x -= bulletSpacing * 0.5f;
        GameObject bullet1 = Instantiate(bullet, bPos1, Quaternion.identity) as GameObject;

        Vector3 bPos2 = initPos;
        bPos2.x += bulletSpacing * 0.5f;
        GameObject bullet2 = Instantiate(bullet, bPos2, Quaternion.identity) as GameObject;

        Vector3 velocity = new Vector3(0.0f, speed, 0.0f);
        bullet1.GetComponent<Rigidbody>().velocity = velocity;
        bullet2.GetComponent<Rigidbody>().velocity = velocity;
    }
}
