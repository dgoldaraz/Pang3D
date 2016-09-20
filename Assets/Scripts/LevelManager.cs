using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	
	private Manager mPlayer;
	
	void Start()
	{
		mPlayer = GameObject.FindObjectOfType<Manager>();
	}	
	
	public void LoadLevel(string name)
	{
		if(mPlayer)
		{
			mPlayer.setLastLevel( SceneManager.GetActiveScene().buildIndex);
		}
		Debug.Log ("Load this level: " + name);
        SceneManager.LoadScene(name);
	}
	
	public void QuitRequest()
	{
		Debug.Log ("Quit Game Requested");
		Application.Quit ();
	}

    public void LoadNextLevel()
    {
        if (mPlayer)
        {
            mPlayer.setLastLevel(SceneManager.GetActiveScene().buildIndex);
        }
        print(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
	
	public void LoadLastLevel()
	{
        if (mPlayer)
		{
			int lastLevel = mPlayer.getLastLevel();
			print (lastLevel);
			SceneManager.LoadScene(lastLevel);
		}
	}
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
