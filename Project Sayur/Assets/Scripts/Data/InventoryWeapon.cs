using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryWeapon
{
	public WeaponData Weapon;
	public bool Unlocked;

	public InventoryWeapon (WeaponData weapon, bool unlocked)
	{
		Weapon = weapon;
		Unlocked = unlocked;
	}
}
