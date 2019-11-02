using UnityEngine;
using System.Collections;

public static class WeaponDatabase
{
	static WeaponData[] weapons = new WeaponData [] {
		new WeaponData (0, "Pistol", "Just a gun", 1, 0, 20, .7f, 20),
		new WeaponData (1, "SMG", "A rapid-fire gun", 3, 100, 10, .2f, 50),
		new WeaponData (2, "Shotgun", "Five bullet shots at once", 5, 300, 30, 1.2f, 10)
	};
		
	public static int GetWeaponsLength ()
	{
		return weapons.Length;
	}

	public static WeaponData GetWeapon (int index)
	{
		if (index < weapons.Length)
		{
			return weapons [index];
		}

		return null;
	}

	public static WeaponData GetWeapon (string name)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons [i].Name == name)
			{
				return weapons [i];
			}
		}

		return null;
	}
}
