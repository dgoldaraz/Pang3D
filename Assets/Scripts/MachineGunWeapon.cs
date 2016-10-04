using UnityEngine;
using System.Collections;

public class MachineGunWeapon : Weapon {

    public GameObject bullet;
    public float bulletSpacing = 1.0f;
    public float speed = 6.0f;
    public AudioClip machineGunSound;
    /// <summary>
    /// Create two bullets and set its velocity
    /// </summary>
    /// <param name="player"></param>
    /// <param name="initPos"></param>
    public override void Shoot(GameObject player, Vector3 initPos)
    {
        //Create two bullets and moveUp
        GameObject bullet1 = Instantiate(bullet, initPos, Quaternion.identity) as GameObject;
        bullet1.GetComponent<Bullet>().bulletSpacing = -bulletSpacing;

        GameObject bullet2 = Instantiate(bullet, initPos, Quaternion.identity) as GameObject;
        bullet2.GetComponent<Bullet>().bulletSpacing = bulletSpacing;

        Vector3 velocity = new Vector3(0.0f, speed, 0.0f);
        bullet1.GetComponent<Rigidbody>().velocity = velocity;
        bullet2.GetComponent<Rigidbody>().velocity = velocity;

        player.GetComponent<Player>().setAudioShoot(machineGunSound);
    }
}
