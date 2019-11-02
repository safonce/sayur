using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectLevelUI : Singleton<SelectLevelUI>
{
	[SerializeField] GameObject levelButtonPrefab;

	[SerializeField] Transform levelPanelContent;

	[SerializeField] string levelImagePath = "Levels/";

	public void CreateLevelButtons ()
	{
		if (levelButtonPrefab == null)
			return;

		for (int i = 0; i < LevelDatabase.GetLevelsLength (); i++)
		{
			GameObject levelButtonClone = Instantiate (levelButtonPrefab) as GameObject;
			levelButtonClone.transform.SetParent (levelPanelContent, false);
		
			LevelData levelData = LevelDatabase.GetLevel (i);

			Sprite sprite = Resources.Load (levelImagePath + levelData.title, typeof(Sprite)) as Sprite;

			Image image = levelButtonClone.GetComponent<Image> ();
			if (image != null)
			{
				image.sprite = sprite;
			}

			LevelButton levelButton = levelButtonClone.GetComponent<LevelButton> ();
			levelButton.SetLevelButton (levelData);
		}
	}

}
