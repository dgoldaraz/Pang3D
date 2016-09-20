using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIAndInputManager : MonoBehaviour {


    //   private Button[] buttons;
    //   public Text LeftFuel;
    //   public Text RightFuel;
    //   public Text CenterFuel;
    //   public Text speedText;
    //   public Text winTextPanel;
    //   public Text countDownText;

    private bool m_onPause = false;
    private float m_timeScale;


    //   // Use this for initialization
    //   void Start ()
    //   {
    //       //buttons = GameObject.FindObjectsOfType<Button>();
    //       //hideButtons();

    //       //RocketEngine.onFuelUpdate += UpdateText;
    //       //GameObject.FindObjectOfType<RocketEngine>().UpdateFuelGUI();
    //       //InvokeRepeating("UpdateSpeed", 0.0f, 1.0f);
    //       //winTextPanel.gameObject.SetActive(false);
    //       //countDownText.gameObject.SetActive(false);
    //}

    //   void OnDestroy()
    //   {
    //       //RocketEngine.onFuelUpdate -= UpdateText;
    //   }

    // Update is called once per frame
    void Update()
    {
        //Pause Menu
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!m_onPause)
            {
                //Show Menu
                m_timeScale = Time.timeScale;
                Time.timeScale = 0.0f;
                m_onPause = true;
            }
            else
            {
                //hideMenu
                Time.timeScale = m_timeScale;
                m_onPause = false;
            }

        }
    }

    //   void hideButtons()
    //   {
    //       foreach( Button b in buttons)
    //       {
    //           b.gameObject.SetActive(false);
    //       }
    //   }

    //   public void showButtons()
    //   {
    //       foreach (Button b in buttons)
    //       {
    //           b.gameObject.SetActive(true);
    //       }
    //   }

    //   public void showWinPanel()
    //   {
    //       winTextPanel.gameObject.SetActive(true);
    //       countDownText.text = "3";
    //       countDownText.gameObject.SetActive(true);
    //       InvokeRepeating("DecreaseCountDown", 0.0f, 1.0f);
    //   }

    //   void DecreaseCountDown()
    //   {
    //       int countDown = int.Parse(countDownText.text);
    //       if(countDown == 0)
    //       {
    //           GameObject.FindObjectOfType<LevelManager>().LoadNextLevel();
    //       }
    //       else
    //       {
    //           countDown--;
    //           countDownText.text = countDown.ToString();
    //       }
    //   }

    //   void UpdateText(string name, float value)
    //   {
    //       if(name == "Left")
    //       {
    //           LeftFuel.text = "Left Fuel: " + value.ToString();
    //           if(value == 0f)
    //           {
    //               //No Fuel!!
    //               LeftFuel.color = Color.red;
    //           }
    //       }
    //       if (name == "Right")
    //       {
    //           RightFuel.text = "Right Fuel: " + value.ToString();
    //           if (value == 0f)
    //           {
    //               //No Fuel!!
    //               RightFuel.color = Color.red;
    //           }
    //       }
    //       if (name == "Up")
    //       {
    //           CenterFuel.text = "Center Fuel: " + value.ToString();
    //           if (value == 0f)
    //           {
    //               //No Fuel!!
    //               CenterFuel.color = Color.red;
    //           }
    //       }
    //   }
}
