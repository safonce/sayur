using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour 
{
	string levelName = string.Empty;

	[SerializeField] string nextSceneName = string.Empty;
	[SerializeField] Text levelText = null;
	[SerializeField] GameObject lockImage = null;

	Button button;

	void Awake ()
	{
		button = GetComponent<Button> ();
	}

	public void SetLevel ()
	{
		SelectLevel.Instance.SetLevel (levelName);

		if (LevelManager.Instance != null)
			LevelManager.Instance.GoToLevel (nextSceneName);
	}

	public void SetLevelButton (LevelData levelData)
	{
		levelName = levelData.title;

		if (levelText != null)
		{
			levelText.text = levelName.ToUpper();
		}

		if (Game.current.Level >= levelData.levelToUnlock)
		{
			button.interactable = true;
			if (lockImage != null)
				lockImage.SetActive (false);
		} else
		{
			button.interactable = false;
			if (lockImage != null)
				lockImage.SetActive (true);
		}
	}
}
