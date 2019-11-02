using System;

[Serializable]
public class Hero
{
	public int ID;
	public string Name;
	public string Description;
	public int LevelToUnlock;
	public int Cost;

	public Hero ()
	{
		ID = 0;
		Name = String.Empty;
		Description = String.Empty;
		LevelToUnlock = 1;
		Cost = 0;
	}

	public Hero (int id, string title, string description, int levelToUnlock, int cost)
	{
		ID = id;
		Name = title;
		Description = description;
		LevelToUnlock = levelToUnlock;
		Cost = cost;
	}
}