using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
	[SerializeField] string LevelName = "";

	public void GoToLevel ()
	{
		LevelManager.Instance.GoToLevel (LevelName);
	}

	public void RestartLevel ()
	{
		SaveLoad.Save ();

		LevelManager.Instance.GoToLevel (SceneManager.GetActiveScene ().name);
	}

	public void QuitLevel ()
	{
		Application.Quit ();
	}
}
