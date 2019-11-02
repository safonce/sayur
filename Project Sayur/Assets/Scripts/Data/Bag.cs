using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bag
{
	public WeaponData[] weapons;
	public int maximumWeapon = 3;

	public Bag()
	{
		weapons = new WeaponData[maximumWeapon];
	}

	public void AddWeapon (int weaponIndex)
	{
		if (InventoryIsFull ())
			return;

		bool isExist = false;
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons [i] == null)
				continue;
			
			if (weapons[i].ID == weaponIndex)
			{
				isExist = true;
				break;
			}
		}

		if (!isExist)
		{
			for (int i = 0; i < weapons.Length; i++)
			{
				if (weapons [i] == null)
				{
					weapons [i] = WeaponDatabase.GetWeapon(weaponIndex);

					return;
				}
			}
		}
	}

	public void RemoveWeapon (int weaponIndex)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons [i] == null)
				continue;
			
			if (weapons[i].ID == weaponIndex)
			{
				weapons [i] = null;

				return;
			}
		}
	}

	public bool WeaponIsInBag (int weaponIndex)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons [i] == null)
				continue;
			
			if (weapons[i].ID == weaponIndex)
			{
				return true;
			}
		}

		return false;
	}

	bool InventoryIsFull ()
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons [i] == null)
			{
				return false;
			}
		}

		return true;
	}

	public bool InventoryIsEmpty ()
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons [i] != null)
			{
				return false;
			}
		}

		return true;
	}

	public int GetBagLength ()
	{
		return weapons.Length;
	}
}
