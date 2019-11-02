using UnityEngine;
using System.Collections;

public static class HeroDatabase
{
	static Hero[] heroes = new Hero[] {
		new Hero (0, "Awang", "Anak melayu", 1, 0),
		new Hero (1, "Cowboy", "A boy who want to fly", 3, 200),
		new Hero (2, "My Robot", "Colorful Robot", 5, 500)
	};

	public static int GetHeroesLength ()
	{
		return heroes.Length;
	}

	public static Hero GetHero (int index)
	{
		if (index < heroes.Length)
		{
			return heroes [index];
		}

		return null;
	}

	public static Hero GetHero (string name)
	{
		for (int i = 0; i < heroes.Length; i++)
		{
			if (heroes [i].Name == name)
			{
				return heroes [i];
			}
		}

		return null;
	}
}
