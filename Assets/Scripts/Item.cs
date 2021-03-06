﻿using UnityEngine;
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

    private GameObject m_Preview;

    public AudioClip explosionSound;
    public AudioClip shieldSound;
    public AudioClip lifeSound;
    public AudioClip weaponSound;

    public bool setRandom = true;

    // Use this for initialization
    void Start ()
    {
        renderer = GetComponent<MeshRenderer>();
        if(setRandom)
        {
            setRandomItem();
        }
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
        if (objectsPreview[(int)type] != null)
        {
            m_Preview = Instantiate(objectsPreview[(int)type], gameObject.transform.position, objectsPreview[(int)type].transform.rotation) as GameObject;
            m_Preview.transform.parent = gameObject.transform;
            renderer = m_Preview.GetComponent<MeshRenderer>();
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
        float blinkingAmount = 0.2f;
        InvokeRepeating("Blink", 0, blinkingAmount);
        Destroy(gameObject, blinkingTime);
    }

    void Blink()
    {
        if(type == ItemType.Shield)
        {
            ParticleSystem ps = m_Preview.GetComponentInChildren<ParticleSystem>();
            if(ps.isPaused)
            {
                ps.Play();
            }
            else
            {
                ps.Pause();
            }
        }
        else
        {
            renderer.enabled = !renderer.enabled;
        }
       
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
        player.GetComponent<Player>().setWeapon(hookGO);
        FindObjectOfType<GameManager>().playSound(weaponSound);
    }

    void setDoubleHook(GameObject player)
    {
        player.GetComponent<Player>().setWeapon(doubleHookGO);
        FindObjectOfType<GameManager>().playSound(weaponSound);
    }

    void setGrabHook(GameObject player)
    {
        player.GetComponent<Player>().setWeapon(grabHookGO);
        FindObjectOfType<GameManager>().playSound(weaponSound);
    }

    void setMachineGun(GameObject player)
    {
        player.GetComponent<Player>().setWeapon(machineGunGO);
        FindObjectOfType<GameManager>().playSound(weaponSound);
    }

    void setShield(GameObject player)
    {
        player.GetComponent<Player>().setShield(true);
        FindObjectOfType<GameManager>().playSound(shieldSound);
    }

    void setLive(GameObject player)
    {
        player.GetComponent<Player>().addLives();
        FindObjectOfType<GameManager>().playSound(lifeSound);
    }

    void setDynamite()
    {
        BallScript[] balls = FindObjectsOfType<BallScript>();
        foreach(BallScript b in balls)
        {
            b.Dynamite( timeToExplode);
        }
        FindObjectOfType<GameManager>().playSound(explosionSound);
    }

    void setStopTime()
    {
        BallScript[] balls = FindObjectsOfType<BallScript>();
        foreach (BallScript b in balls)
        {
            b.Pause(pauseBallTime);
        }
        FindObjectOfType<GameManager>().playSound(weaponSound);
    }

    void setDiagonal(GameObject player)
    {
        player.GetComponent<Player>().setWeapon(diagonalGO);
        FindObjectOfType<GameManager>().playSound(weaponSound);
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
                    setDiagonal(player);
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
