using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GrabHookWeapon : HookWeapon
{


    public AudioClip hookSound;
    public AudioClip hookCollideSound;
    /// <summary>
    /// Override of the collision enter to stop the hook
    /// </summary>
    /// <param name="coll"></param>
    protected override void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Wall"))
        {
            setMovement(false);
            StartCoroutine(ChangeColourOnTIme());
            FindObjectOfType<GameManager>().playSound(hookCollideSound);
        }
        else
        {
            base.OnCollisionEnter(coll);
        }
    }

    /// <summary>
    /// Change color to advise it's close to dissappear
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColourOnTIme()
    {
        yield return new WaitForSeconds(1);
        List<GameObject> parts = getParts();
        foreach(GameObject g in parts)
        {
            g.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        //Change colour and call destroy with time
        StartCoroutine(DestroyOnTime());
    }

    /// <summary>
    /// destroy the grabhook and allow to whoot again
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(1);
        //Change colour and call destroy with time
        Destroy(gameObject);
        m_player.GetComponent<Player>().setCanShoot(true);
    }

    public override void Shoot(GameObject player, Vector3 initPos)
    {
        base.Shoot(player, initPos);
        player.GetComponent<Player>().setAudioShoot(hookSound);
    }
 }
