using UnityEngine;
using System.Collections;
using System;

public class Item : MonoBehaviour {

    public enum ItemType { Hook, DoubleHook, GrabHook, MachineGun, Shield, Dynamite, StopTime, Diagonal}

    public ItemType type;

    public GameObject hookGO;
    public GameObject doubleHookGO;
    public GameObject grabHookGO;
    public GameObject machineGunGO;
    public GameObject shieldGO;
    public GameObject diagonalGO;
    public float pauseTime = 2.0f;

    public float blinkingTime = 2.0f;

    private float m_lifeTime = 5.0f;

    private new MeshRenderer renderer;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<MeshRenderer>();
        setTime(m_lifeTime);
	}


    void setRandomItem()
    {
        type = (ItemType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemType)).Length);
    }

    /// <summary>
    /// Set the time of life
    /// </summary>
    /// <param name="t"></param>
    void setTime(float t)
    {
        m_lifeTime = t;
        StartCoroutine(DestroyOnTime());
    }

    /// <summary>
    /// Waits for some seconds and the start blinking and die
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(m_lifeTime - blinkingTime);
        InvokeRepeating("Blink", 0, 0.2f);
        Destroy(gameObject, blinkingTime);
    }

    void Blink()
    {
        renderer.enabled = !renderer.enabled;
    }

    /// <summary>
    /// Deals with collisions, baisicly, if the collision it's with the player the "item type' will be used
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            ApplyEffect(coll.gameObject);
        }
    }


    void setHook(GameObject player)
    {
        player.GetComponent<Player>().SetWeapon(hookGO);
    }

    void setDoubleHook(GameObject player)
    {
        player.GetComponent<Player>().SetWeapon(doubleHookGO);
    }

    void setGrabHook(GameObject player)
    {
        player.GetComponent<Player>().SetWeapon(grabHookGO);
    }

    void setMachineGun(GameObject player)
    {
        player.GetComponent<Player>().SetWeapon(machineGunGO);
    }

    void setShield(GameObject player)
    {

    }

    void setDynamite()
    {
        BallScript[] balls = FindObjectsOfType<BallScript>();
        foreach(BallScript b in balls)
        {
            b.Dynamite();
        }
    }

    void setStopTime()
    {
        BallScript[] balls = FindObjectsOfType<BallScript>();
        foreach (BallScript b in balls)
        {
            b.Pause(pauseTime);
        }
    }

    void setDiagonal()
    {

    }

    /// <summary>
    /// Set the effect of the item depending on the Type
    /// </summary>
    /// <param name="player"></param>
    void ApplyEffect(GameObject player)
    {
        switch (type)
        {
            case ItemType.Hook:
                {
                    setHook(player);
                    break;
                }
            case ItemType.DoubleHook:
                {
                    setDoubleHook(player);
                    break;
                }
            case ItemType.GrabHook:
                {
                    setGrabHook(player);
                    break;
                }
            case ItemType.MachineGun:
                {
                    setMachineGun(player);
                    break;
                }
            case ItemType.Shield:
                {
                    setShield(player);
                    break;
                }
            case ItemType.Dynamite:
                {
                    setDynamite();
                    break;
                }
            case ItemType.StopTime:
                {
                    setStopTime();
                    break;
                }
            case ItemType.Diagonal:
                {
                    setDiagonal();
                    break;
                }
        }
    }
}
