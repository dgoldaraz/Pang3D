using UnityEngine;
using System.Collections;

public class HookWeapon : Weapon {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	} 

    public override void Shoot(GameObject player)
    {
        Debug.Log(player.name + " Shoots hook");
    }
}
