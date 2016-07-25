using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public int numLives = 3;

    static private GameManager instance = null;

    struct PlayerInfo
    {
        public string playerName;
        public int lives;
    }

    
    private List<PlayerInfo> m_playerInfo = new List<PlayerInfo>();
    /// <summary>
    /// Singleton implmentation
    /// </summary>

    void Awake()
    {
        if (instance != null)
        {
            //Destroy duplicate Instances
            Destroy(this.gameObject);
            instance.Initialize();
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
        Initialize();
    }
	
    void Initialize()
    {
        //Add a listerner to the player hit
        Player.onPlayerHit += updatePlayersHit;
        Player.onPlayerAddLive += updatePlayerLives;


        if (m_playerInfo.Count == 0)
        {
            //Save all the players on the level
            Player[] m_players = FindObjectsOfType<Player>();
            //Init the list
            foreach (Player p in m_players)
            {
                PlayerInfo pStruct;
                pStruct.playerName = p.gameObject.name;
                pStruct.lives = numLives;
                m_playerInfo.Add(pStruct);
            }
        }
        else
        {
            Player[] m_players = FindObjectsOfType<Player>();
            //Init the list
            foreach (Player p in m_players)
            {
                foreach(PlayerInfo pp in m_playerInfo)
                {
                    if(pp.playerName == p.gameObject.name)
                    {
                        p.setLives(pp.lives);
                    }
                }
            }
        }
        
    }

    /// <summary>
    /// Updates the state of the player passed when it's hit by a ball
    /// </summary>
    /// <param name="player"></param>
    void updatePlayersHit(GameObject player)
    {
        
        //check if the game ends
        bool endGame = true;
        for(int i= 0; i < m_playerInfo.Count; ++i)
        {
            PlayerInfo pp = m_playerInfo[i];
            if (pp.playerName == player.gameObject.name)
            {
                pp.lives = m_playerInfo[i].lives - 1;
                m_playerInfo[i] = pp;
                if (pp.lives > 0)
                {
                    endGame = false;
                    break;
                }
            }
            else
            {
                if(pp.lives > 0)
                {
                    endGame = false;
                }
            }
        }

        if(endGame || numLives == 0)
        {
            //
            Debug.Log("Game Ends!! You Lost");
            //Return to main level
        }
        else
        {
            //Pause Everything!
            //Debug.Log("Pause Things!");
            BallScript[] balls = FindObjectsOfType<BallScript>();
            foreach(BallScript b in balls)
            {
                b.Pause(-1);
            }
            Player.onPlayerHit -= updatePlayersHit;
            Player.onPlayerAddLive -= updatePlayerLives;
            StartCoroutine(Restart());
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void updatePlayerLives(GameObject player)
    {
        for(int i = 0; i < m_playerInfo.Count; ++i)
        {
            if(m_playerInfo[i].playerName == player.name)
            {
                PlayerInfo pp = m_playerInfo[i];
                pp.lives = m_playerInfo[i].lives + 1;
                m_playerInfo[i] = pp;
            }
        }
        
    }
}
