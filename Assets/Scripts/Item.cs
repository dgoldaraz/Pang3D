using UnityEngine;
using System.Collections;
using System;

public class Item : MonoBehaviour {

    public enum ItemType { Hook, DoubleHook, GrabHook, MachineGun, Shield, Dynamite, StopTime, Diagonal, Live}

    public ItemType type;

    public GameObject hookGO;
    public GameObject doubleHookGO;
    public GameObject grabHookGO;
    public GameObject machineGunGO;
    public GameObject diagonalGO;
    public float pauseBallTime = 4.0f;
    public float timeToExplode = 0.5f;
    public float blinkingTime = 2.0f;

    public float m_lifeTime = 5.0f;

    private new MeshRenderer renderer;

    public GameObject[] objectsPreview;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<MeshRenderer>();
        //setRandomItem();
        init();
        setTime(m_lifeTime);
        int numTypes = System.Enum.GetValues(typeof(ItemType)).Length;
        if (objectsPreview.Length < numTypes)
        {
            objectsPreview = new GameObject[numTypes];
        }
	}


    /// <summary>
    /// Init the object depending on the type
    /// </summary>
    void init()
    {
        GameObject go = null;
        if (objectsPreview[(int)type] != null)
        {
            go = objectsPreview[(int)type];
            gameObject.GetComponent<MeshFilter>().mesh = go.GetComponent<MeshFilter>().mesh;
        }

        switch (type)
        {
            case ItemType.Hook:
                {
                    renderer.material.color = Color.black;
                    break;
                }
            case ItemType.DoubleHook:
                {
                    renderer.material.color = Color.blue;
                    break;
                }
            case ItemType.GrabHook:
                {
                    renderer.material.color = Color.cyan;
                    break;
                }
            case ItemType.MachineGun:
                {
                    renderer.material.color = Color.gray;
                    break;
                }
            case ItemType.Shield:
                {
                    renderer.material.color = Color.green;
                    break;
                }
            case ItemType.Dynamite:
                {
                    renderer.material.color = Color.red;
                    break;
                }
            case ItemType.StopTime:
                {
                    renderer.material.color = Color.magenta;
                    break;
                }
            case ItemType.Diagonal:
                {
                    renderer.material.color = Color.white;
                    break;
                }
            case ItemType.Live:
                {
                    //renderer.material.color = Color.yellow;
                    break;
                }
        }
    }
    /// <summary>
    /// Sets the type of the item randomly
    /// </summary>
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
            Destroy(gameObject);
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
        player.GetComponent<Player>().setShield(true);
    }

    void setLive(GameObject player)
    {
        player.GetComponent<Player>().addLives();
    }

    void setDynamite()
    {
        BallScript[] balls = FindObjectsOfType<BallScript>();
        foreach(BallScript b in balls)
        {
            b.Dynamite( timeToExplode);
        }
    }

    void setStopTime()
    {
        BallScript[] balls = FindObjectsOfType<BallScript>();
        foreach (BallScript b in balls)
        {
            b.Pause(pauseBallTime);
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
            case ItemType.Live:
                {
                    setLive(player);
                    break;
                }
        }
    }
}
