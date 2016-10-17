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
    private int m_initialScore = 100;

    public delegate void ScoreChanged(int score);
    public static event ScoreChanged onScoreChanged;

    public delegate void CountDownChanged(int countDown);
    public static event CountDownChanged onCountDownChanged;
    private AudioSource m_audioSource;
    public AudioClip winSound;
    public AudioClip[] bckSongs;

    private int m_countDown;
    private bool m_initialize = false;

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
        setRandomSong();
    }

    public bool isInitialize()
    {
        return m_initialize;
    }
	
    void Initialize()
    {
        m_initialize = true;
        setLastLevel(SceneManager.GetActiveScene().buildIndex);
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

        m_countDown = secondsToEnd +1;
        m_audioSource = GetComponent<AudioSource>();
        restartCountDown();
        m_initialScore = m_score;
    }

    void OnDestroy()
    {
        Player.onPlayerHit -= updatePlayersHit;
        Player.onPlayerAddLive -= updatePlayerLives;
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
                Debug.Log("Player " + pp.playerName + " hit, lives: " + pp.lives);
            }
            else
            {
                if(pp.lives > 0)
                {
                    endGame = false;
                }
            }
        }


        Player.onPlayerHit -= updatePlayersHit;
        Player.onPlayerAddLive -= updatePlayerLives;

        StopAllCoroutines();
        if (endGame || maxNumLives == 0)
        {
            FindObjectOfType<GUIAndInputManager>().showLosePanel();
            //Return to main level
            m_score = 0;
        }
        else
        {
            //Pause Everything!
            BallScript[] balls = FindObjectsOfType<BallScript>();
            foreach(BallScript b in balls)
            {
                b.Pause(-1);
            }

           
            StartCoroutine(RestartLevel());
        }
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1);
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
                Debug.Log("Player " + pp.playerName + " add live: " + pp.lives);
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
            CancelInvoke("decreaseCountDown");
            FindObjectOfType<GUIAndInputManager>().showWinPanel();
            //Add 500 points for wining
            addPoints(500);
            playSound(winSound);
            m_initialScore = m_score;
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
    /// <summary>
    /// Method to start Count Down
    /// </summary>
    public void startCountDown()
    {
        InvokeRepeating("decreaseCountDown", 0.0f, 1.0f);
    }

    /// <summary>
    /// Method that decrease the count down of the game
    /// </summary>
    void decreaseCountDown()
    {
        
        if (m_countDown == 0)
        {
            //Finish the game if the countdown is 0
            FindObjectOfType<GUIAndInputManager>().showLosePanel();
            //Return to main level
            m_score = m_initialScore;
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

    public void restartCountDown()
    {
        CancelInvoke("decreaseCountDown");
        m_countDown = secondsToEnd +1;
        startCountDown();
    }
    /// <summary>
    /// Set a sound in the audioSource and Play it
    /// </summary>
    /// <param name="sound"></param>
    public void setSound(AudioClip sound)
    {
        m_audioSource.Stop();
        m_audioSource.clip = sound;
        m_audioSource.Play();

    }
    /// <summary>
    /// Play an instant sound
    /// </summary>
    /// <param name="sound"></param>
    public void playSound(AudioClip sound)
    {
        m_audioSource.PlayOneShot(sound);
    }
    /// <summary>
    /// True if the player have 0 lives
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool shouldPlayerDestroy(GameObject player)
    {
        //Check if the player has lives
        for (int i = 0; i < m_playerInfo.Count; ++i)
        {
            if (m_playerInfo[i].playerName == player.name)
            {
               if(m_playerInfo[i].lives == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Return the lives of the player
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int getLivesFrom(GameObject player)
    {
        for (int i = 0; i < m_playerInfo.Count; ++i)
        {
            if (m_playerInfo[i].playerName == player.name)
            {
                return m_playerInfo[i].lives;
            }
        }
        return -1; // Error
    }

    public void restartGame()
    {
        for (int i = 0; i < m_playerInfo.Count; ++i)
        {
            PlayerInfo pp = m_playerInfo[i];
            pp.lives = maxNumLives;
            m_playerInfo[i] = pp;
        }
        removeObservers();
    }

    public void setRandomSong()
    {
        //Select a random theme and play it
        AudioClip s = bckSongs[Random.Range(0, bckSongs.Length - 1)];
        setSound(s);
    }

    public void removeObservers()
    {
        Player.onPlayerHit -= updatePlayersHit;
        Player.onPlayerAddLive -= updatePlayerLives;
    }
}
