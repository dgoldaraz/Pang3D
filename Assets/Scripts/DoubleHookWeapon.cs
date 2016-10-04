using UnityEngine;
using System.Collections;

public class DoubleHookWeapon : HookWeapon {

    /// <summary>
    /// Only allows the user to shoot if there are less than 2 hooks in the scene
    /// </summary>
    /// <param name="player"></param>
    /// <param name="initPos"></param>
    public override void Shoot(GameObject player, Vector3 initPos)
    {
        base.Shoot(player, initPos);
        //Count how many hooks are in the scene to avoid more than 2
        HookWeapon[] h = FindObjectsOfType<HookWeapon>();
        player.GetComponent<Player>().setCanShoot(h.Length < 2);
        m_player.GetComponent<Player>().resetShootSound();
    }
}
