using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	private static Manager mpInstance = null;
	private static int lastLevel = 1;
	// Use this for initialization
	
	void Awake()
	{
		if( mpInstance != null)
		{
			Destroy (gameObject);
		}
		else
		{
			mpInstance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
	
	public int getLastLevel()
	{
		return lastLevel;
	}
	
	public void setLastLevel(int level)
	{
		lastLevel = level;
	}
}
