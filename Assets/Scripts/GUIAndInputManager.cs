﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIAndInputManager : MonoBehaviour {


    public Text winTextPanel;
    public Text countDownText;

    public GameObject losePanel;
    public GameObject pausePanel;

    public Text levelText;
    public Text scoreText;
    public Text timeText;


    //Lives 
    public GameObject fPlayerLives;
    private Image firstLiveF;
    private Image secondLiveF;
    private Image thirdLiveF;
    private Text moreLivesF;

    public GameObject sPlayerLives;
    private Image firstLiveS;
    private Image secondLiveS;
    private Image thirdLiveS;
    private Text moreLivesS;


    private bool m_onPause = false;
    private float m_timeScale = 1f;
    private GameManager m_gm;


    // Use this for initialization
    void Start()
    {
        GameManager[] gms = GameObject.FindObjectsOfType<GameManager>();
        //get only the gameManager that contains info!
        foreach(GameManager g in gms)
        {
            if(g.isInitialize())
            {
                m_gm = g;
            }
        }
        m_timeScale = 1f;
        GameManager.onScoreChanged += updateScore;
        GameManager.onCountDownChanged += updateTime;
        Player.onPlayerAddLive += updateLives;
        if(pausePanel)
        {
            pausePanel.SetActive(false);
        }

        winTextPanel.transform.parent.gameObject.SetActive(false);
        if(losePanel)
        {
            losePanel.SetActive(false);
        }

        levelText.text = "Level " + m_gm.getLastLevel().ToString();
        scoreText.text = "Score " + m_gm.getScore().ToString();
        fPlayerLives = GameObject.Find("Lives");
        if (fPlayerLives != null)
        {
            //GetLives
            Image[] livesImages = fPlayerLives.GetComponentsInChildren<Image>();
            firstLiveF = livesImages[0];
            secondLiveF = livesImages[1];
            thirdLiveF = livesImages[2];
            moreLivesF = fPlayerLives.GetComponentInChildren<Text>();
            moreLivesF.gameObject.SetActive(false);
        }
        sPlayerLives = GameObject.Find("LivesSecond");
        if (sPlayerLives != null)
        {
            //GetLives
            Image[] livesImages = sPlayerLives.GetComponentsInChildren<Image>();
            firstLiveS = livesImages[0];
            secondLiveS = livesImages[1];
            thirdLiveS = livesImages[2];
            moreLivesS = sPlayerLives.GetComponentInChildren<Text>();
            moreLivesS.gameObject.SetActive(false);
        }
        //hideLives();

        Player[] pl = FindObjectsOfType<Player>();
        foreach(Player p in pl)
        {
            updateLives(p.gameObject);
        }
        if(pl.Length <2 && sPlayerLives)
        {
            sPlayerLives.SetActive(false);
        }
    }

    void OnDestroy()
    {
        GameManager.onScoreChanged -= updateScore;
        GameManager.onCountDownChanged -= updateTime;
        Player.onPlayerAddLive -= updateLives;
    }

    public void hideLives()
    {
        if(fPlayerLives)
        {
            firstLiveF.gameObject.SetActive(false);
            secondLiveF.gameObject.SetActive(false);
            thirdLiveF.gameObject.SetActive(false);
            moreLivesF.gameObject.SetActive(false);
        }
        if(sPlayerLives)
        {
            firstLiveS.gameObject.SetActive(false);
            secondLiveS.gameObject.SetActive(false);
            thirdLiveS.gameObject.SetActive(false);
            moreLivesS.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Pause Menu
        if (Input.GetKeyDown(KeyCode.P))
        {
            setPause();
        }
    }

    public void setPause()
    {
        if (!m_onPause)
        {
             if(!losePanel.activeInHierarchy && !winTextPanel.gameObject.activeInHierarchy)
            {
                //Show Menu
                m_timeScale = Time.timeScale;
                Time.timeScale = 0.0f;
                m_onPause = true;
                pausePanel.SetActive(true);
            }
        }
        else
        {
            //hideMenu
            Time.timeScale = m_timeScale;
            m_onPause = false;
            pausePanel.SetActive(false);
        }
    }

    public void restartGame()
    {
        //hideMenu
        if(m_timeScale != 0f)
        {
            Time.timeScale = m_timeScale;
        }
        m_onPause = false;
        m_gm.restartGame();
    }

    public void showWinPanel()
    {
        winTextPanel.transform.parent.gameObject.SetActive(true);
        countDownText.text = "5";
        InvokeRepeating("decreaseCountDownForWin", 0.0f, 1.0f);
    }

    void decreaseCountDownForWin()
    {
        int countDown = int.Parse(countDownText.text);
        if (countDown == 1)
        {
            GameObject.FindObjectOfType<LevelManager>().LoadNextLevel();
            GameObject.FindObjectOfType<GameManager>().setRandomSong();
            GameObject.FindObjectOfType<GameManager>().removeObservers();
        }
        else
        {
            countDown--;
            countDownText.text = countDown.ToString();
        }
    }

    /// <summary>
    /// Shows the LosePanel
    /// </summary>
    public void showLosePanel()
    {
        hideLives();
        losePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// Updates the score based on the GameManager
    /// </summary>
    /// <param name="newScore"></param>
    void updateScore(int newScore)
    {
        if(scoreText)
        {
            scoreText.text = "Score " + newScore.ToString();
        }
    }
    /// <summary>
    /// Updates the time UI based on the GameManager
    /// </summary>
    /// <param name="time"></param>
    void updateTime(int time)
    {
        if (timeText)
        {
            timeText.text = "Time " + time.ToString();
        }
    }

    void updateLives(GameObject player)
    {
        if(player.CompareTag("Player"))
        {
            //FirstPlayer
            int lives = m_gm.getLivesFrom(player);
            if (lives < 4)
            {
                //Images
                firstLiveF.gameObject.SetActive(false);
                secondLiveF.gameObject.SetActive(false);
                thirdLiveF.gameObject.SetActive(false);

                if (lives > 0)
                {
                    firstLiveF.gameObject.SetActive(true);
                }
                if (lives > 1)
                {
                    secondLiveF.gameObject.SetActive(true);
                }
                if (lives > 2)
                {
                    thirdLiveF.gameObject.SetActive(true);
                }
                moreLivesF.gameObject.SetActive(false);
                moreLivesF.GetComponent<Text>().gameObject.SetActive(false);
            }
            else
            {
                moreLivesF.gameObject.SetActive(true);
                moreLivesF.GetComponent<Text>().gameObject.SetActive(true);
                //number
                int extraLives = lives - 3;
                moreLivesF.text = "x" + extraLives.ToString();
            }
        }
        else
        {
            //SecondPlayer

            int lives = m_gm.getLivesFrom(player);
            if (lives < 4)
            {
                //Images
                firstLiveS.gameObject.SetActive(false);
                secondLiveS.gameObject.SetActive(false);
                thirdLiveS.gameObject.SetActive(false);

                if (lives > 0)
                {
                    firstLiveS.gameObject.SetActive(true);
                }
                if (lives > 1)
                {
                    secondLiveS.gameObject.SetActive(true);
                }
                if (lives > 2)
                {
                    thirdLiveS.gameObject.SetActive(true);
                }
                moreLivesS.gameObject.SetActive(false);
            }
            else
            {
                moreLivesS.gameObject.SetActive(false);
                //number
                int extraLives = lives - 3;
                moreLivesS.text = "x" + extraLives.ToString();
            }
        }
    }
}
