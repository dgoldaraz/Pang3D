using UnityEngine;
using System.Collections;

public class DiagonalHook : HookWeapon
{
    /// <summary>
    /// In this shoot we basically call the base shotting method and change the direction of the shot (based on the correct peephole)
    /// </summary>
    /// <param name="player"></param>
    /// <param name="initPos"></param>
    public override void Shoot(GameObject player, Vector3 initPos)
    {
        

        base.Shoot(player, initPos);
        // Change direction of the shoot
        float newDirectionX = m_player.GetComponent<Player>().getDiagonalOffset();
        Vector3 newDirection = new Vector3(newDirectionX, 1.0f, 0.0f);
        setShootDirection(newDirection);

        float newAngle = m_player.GetComponent<Player>().getDiagonalAngle();
        setShootRotation(newAngle);
    }
}
