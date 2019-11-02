using UnityEngine;
using System.Collections;

public static class LevelDatabase
{
	static LevelData[] levels = new LevelData [] {
		new LevelData ("Suburb", 1),
		new LevelData ("Farm", 3)
	};

	public static int GetLevelsLength ()
	{
		return levels.Length;
	}

	public static LevelData GetLevel (int index)
	{
		if (index < levels.Length)
		{
			return levels [index];
		}

		return null;
	}

	public static LevelData GetLevel (string name)
	{
		for (int i = 0; i < levels.Length; i++)
		{
			if (levels [i].title == name)
			{
				return levels [i];
			}
		}

		return null;
	}

	public static int GetIndexOf (string name)
	{
		for (int i = 0; i < levels.Length; i++)
		{
			if (levels [i].title == name)
			{
				return i;
			}
		}

		return -1;
	}
}
