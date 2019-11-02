using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectLevel : Singleton<SelectLevel> 
{
	protected override void Awake ()
	{
		base.Awake ();

		if (Game.current == null)
			Game.current = new Game ();

		SelectLevelUI.Instance.CreateLevelButtons ();
	}

	public void SetLevel (string name)
	{
		if (Game.current != null)
		{
			Game.current.CurrentLevel = name;
		}
	}
}
