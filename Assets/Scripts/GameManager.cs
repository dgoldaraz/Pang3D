using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public int numLives = 3;

    static private GameManager instance = null;
    private Player[] m_players;

    /// <summary>
    /// Singleton implmentation
    /// </summary>

    void Awake()
    {
        if (instance != null)
        {
            //Destroy duplicate Instances
            Destroy(this.gameObject);
        }
        else
        {
            //Assign the instance and don't destroy
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {

        //Add a listerner to the player hit
        Player.onPlayerHit += updatePlayersHit;

        //Save all the players on the level
        m_players = FindObjectsOfType<Player>();
        foreach(Player p in m_players)
        {
            p.setLives(numLives);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// Updates the state of the player passed when it's hit by a ball
    /// </summary>
    /// <param name="player"></param>
    void updatePlayersHit(GameObject player)
    {
        
        //check if the game ends
        bool endGame = true;
        foreach(Player p in m_players)
        {
            if(p.getLives() > 0)
            {
                endGame = false;
                break;
            }
        }

        if(endGame)
        {
            //
            Debug.Log("Game Ends!! You Lost");
           
        }
        else
        {
            //Pause Everything!
            Debug.Log("Pause Things!");
            BallScript[] balls = FindObjectsOfType<BallScript>();
            foreach(BallScript b in balls)
            {
                b.Pause();
            }
            StartCoroutine(Restart());
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
