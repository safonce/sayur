using System;

[Serializable]
public class WeaponData
{
	public int ID;
	public string Name;
	public string Description;
	public int LevelToUnlocked;
	public int Cost;

	public int Damage;
	public float FireRate;
	public int Capacity;

	public WeaponData ()
	{
		ID = 0;
		Name = String.Empty;
		Description = String.Empty;
		LevelToUnlocked = 1;
		Cost = 0;
	}

	public WeaponData (int id, string name, string description, int levelToUnlock, int cost,
		int damage, float fireRate, int capacity)
	{
		ID = id;
		Name = name;
		Description = description;
		LevelToUnlocked = levelToUnlock;
		Cost = cost;
		Damage = damage;
		FireRate = fireRate;
		Capacity = capacity;
	}
}