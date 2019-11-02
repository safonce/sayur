using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
	public float IntroFadeDuration = 1;
	public float OutroFadeDuration = 1;

	void Start ()
	{
		if (FaderManager.Instance != null)
		{
			FaderManager.Instance.FaderOn (false, IntroFadeDuration);
		}
	}

	IEnumerator LoadLevel (string levelName)
	{
		if (Time.timeScale > 0.0f)
		{ 
			yield return new WaitForSeconds(OutroFadeDuration);
		}

		if (Game.current != null)
		{
			Game.current.UnPause ();
		}

		SceneManager.LoadScene (levelName);
	}
		
	public void GoToLevel (string levelName)
	{
		if (FaderManager.Instance != null)
		{
			FaderManager.Instance.FaderOn (true, OutroFadeDuration);
		}

		StartCoroutine ("LoadLevel", levelName);
	}
}
