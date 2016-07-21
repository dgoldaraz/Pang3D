using UnityEngine;
using System.Collections;

public class DoubleHookWeapon : HookWeapon {

    public override void Shoot(GameObject player, Vector3 initPos)
    {
        base.Shoot(player, initPos);
        //Count how many hooks are in the scene to avoid more than 2
        HookWeapon[] h = FindObjectsOfType<HookWeapon>();
        player.GetComponent<Player>().setCanShoot(h.Length < 2);
    }
}
