using UnityEngine;
using System.Collections;

public class MainMenu : Singleton<MainMenu>
{
	[SerializeField] string nextSceneName = "SelectLevel";

	bool fileExist = false;

	protected override void Awake ()
	{
		base.Awake ();

		fileExist = SaveLoad.IsFileExist ();

		if (!fileExist)
		{
			MainMenuUI.Instance.setContinueButtonInteractable (false);
		}
	}

	public void CheckGame ()
	{
		if (fileExist)
		{
			MainMenuUI.Instance.SetWarningPanelActive (true);
		} else
		{
			NewGame ();
		}
	}

	public void Continue ()
	{
		if (fileExist)
		{
			SaveLoad.Load ();

			LevelManager.Instance.GoToLevel (nextSceneName);
		}
	}

	public void NewGame ()
	{
		Game.current = new Game ();

		SaveLoad.Save ();

		MainMenuUI.Instance.SetWarningPanelActive (false);

		LevelManager.Instance.GoToLevel (nextSceneName);
	}

	public void Cancel ()
	{
		MainMenuUI.Instance.SetWarningPanelActive (false);
	}
}
