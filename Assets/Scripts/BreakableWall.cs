using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour
{

    /// <summary>
    /// This class handles what happend when the hook hits a wall that can be broken
    /// </summary>

    public GameObject ItemWhenSplit;
    [Range(0f, 1f)]
    public float randomPercentage = 0.3f; //30% times an item appears
    bool isDestroyed;

    public AudioClip wallBreak;


    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Bullet") && !isDestroyed)
        {
            isDestroyed = true;
            FindObjectOfType<GameManager>().playSound(wallBreak);
            if(Random.value <= randomPercentage)
            {
                Instantiate(ItemWhenSplit, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}