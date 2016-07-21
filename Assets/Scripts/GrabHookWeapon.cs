using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GrabHookWeapon : HookWeapon
{

    protected override void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Wall"))
        {
            setMovement(false);
            StartCoroutine(ChangeColourOnTIme());
        }
        else
        {
            base.OnCollisionEnter(coll);
        }
    }

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

    IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(1);
        //Change colour and call destroy with time
        Destroy(gameObject);
        m_player.GetComponent<Player>().setCanShoot(true);
    }
}
