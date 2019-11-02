using System;

[Serializable]
public class LevelData
{
	public string title;
	public int levelToUnlock;

	public LevelData()
	{
		this.title = string.Empty;
		this.levelToUnlock = 1;
	}

	public LevelData (string title, int levelToUnlock)
	{
		this.title = title;
		this.levelToUnlock = levelToUnlock;
	}
}
