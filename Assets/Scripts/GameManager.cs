using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public int maxNumLives = 3;
    public int secondsToEnd = 70;
    private static GameManager instance = null;
    private static int lastLevel = 1;

    private int m_score = 0;

    public delegate void ScoreChanged(int score);
    public static event ScoreChanged onScoreChanged;


    public delegate void CountDownChanged(int countDown);
    public static event CountDownChanged onCountDownChanged;

    private int m_countDown;

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
                pStruct.lives = maxNumLives;
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
        m_countDown = secondsToEnd;
        StartCountDown();
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

        if(endGame || maxNumLives == 0)
        {
            //
            FindObjectOfType<GUIAndInputManager>().showLosePanel();
            //Return to main level
            m_score = 0;
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

    /// <summary>
    /// Update the player lives depending on the hits
    /// </summary>
    /// <param name="player"></param>
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

    /// <summary>
    /// Checks if the game has ended because there are no more balls
    /// </summary>
    public void CheckEndGame()
    {
        //Check if there are still balls in the scene
        BallScript[] balls = FindObjectsOfType<BallScript>();
        if(balls.Length == 1)
        {
            //Done
            FindObjectOfType<GUIAndInputManager>().showWinPanel();
            //Add 500 points for wining
            addPoints(500);
        }
    }
    /// <summary>
    /// return the current level
    /// </summary>
    /// <returns></returns>
    public int getLastLevel()
    {
        return lastLevel;
    }

    /// <summary>
    /// Change the current levekl
    /// </summary>
    /// <param name="level"></param>
    public void setLastLevel(int level)
    {
        lastLevel = level;
    }
    /// <summary>
    /// Return the current Score
    /// </summary>
    /// <returns></returns>
    public int getScore()
    {
        return m_score;
    }

    /// <summary>
    /// Add points to the score
    /// </summary>
    /// <param name="points"></param>
    public void addPoints(int points)
    {
        m_score += points;
        if(onScoreChanged != null)
        {
            onScoreChanged(m_score);
        }
    }

    public void StartCountDown()
    {
        InvokeRepeating("DecreaseCountDown", 0.0f, 1.0f);
    }

    void DecreaseCountDown()
    {
        
        if (m_countDown == 0)
        {
            //Finish the game if the countdown is 0
            FindObjectOfType<GUIAndInputManager>().showLosePanel();
            //Return to main level
            m_score = 0;
        }
        else
        {
            m_countDown--;
            if (onCountDownChanged != null)
            {
                onCountDownChanged(m_countDown);
            }
        }
    }
}
