using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Game
{
	public static Game current;

	public int Coins;
	public int Level;
	public int ExperiencePoints;
	public int NextExperiencePoints { get { return CalculateNextExp (); } }

	public string CurrentLevel;
	public Bag Bag;

	public bool Paused;
	public bool AutoAimTarget;
	public float savedTimeScale;

	public List<InventoryWeapon> InventoryWeapons;

	public List<HeroParty> Heroes;
	public Hero CurrentHero;

	public AchievementManager achievementManager;

	public Game()
	{
		Coins = 0;
		Level = 1;
		ExperiencePoints = 0;

		CurrentLevel = "Suburb";

		Bag = new Bag ();

		InventoryWeapons = new List<InventoryWeapon> ();

		InventoryWeapon inventoryWeapon = new InventoryWeapon (WeaponDatabase.GetWeapon (0), true);
		InventoryWeapons.Add (inventoryWeapon);

		Bag.AddWeapon (InventoryWeapons [0].Weapon.ID);

		Heroes = new List<HeroParty> ();

		HeroParty heroParty = new HeroParty (HeroDatabase.GetHero (0), true);
		Heroes.Add (heroParty);

		CurrentHero = Heroes [0].Hero;

		Paused = false;
		AutoAimTarget = true;
		savedTimeScale = 1;

		achievementManager = new AchievementManager ();
	}

	public void SetTimeScale (float newTimeScale)
	{
		savedTimeScale = Time.timeScale;
		Time.timeScale = newTimeScale;
	}

	public void ResetTimeScale ()
	{
		Time.timeScale = savedTimeScale;
	}

	public void Pause ()
	{
		SetTimeScale (0);
		Paused = true;
	}

	public void UnPause ()
	{
		SetTimeScale (1);
		Paused = false;
	}

	public void ToggleAutoAimTarget ()
	{
		AutoAimTarget = !AutoAimTarget;
	}

	public void AddExperiencePoints (int addExperiencePoints)
	{
		ExperiencePoints += addExperiencePoints;

		while (ExperiencePoints >= NextExperiencePoints)
			LevelUp ();
	}

	void LevelUp()
	{
		Level++;
	}

	int CalculateNextExp ()
	{
		return (int)Mathf.Pow (Level / .2f, 2);
	}

	public void AddWeaponToInventory (WeaponData weaponToAdd)
	{
		InventoryWeapon existingWeaponInInventory = InventoryWeapons.SingleOrDefault(iw => iw.Weapon.Name == weaponToAdd.Name);

		if(existingWeaponInInventory == null)
		{
			InventoryWeapons.Add (new InventoryWeapon(weaponToAdd, false));
		}
	}

	public void RemoveWeaponFromInventory (WeaponData weaponToRemove)
	{
		InventoryWeapon weapon = InventoryWeapons.SingleOrDefault(iw => iw.Weapon.Name == weaponToRemove.Name);

		if(weapon != null)
		{
			InventoryWeapons.Remove(weapon);
		}
	}

	public int GetInventoryLength ()
	{
		return InventoryWeapons.Count;
	}

	public void AddHeroToParty (Hero heroToAdd)
	{
		HeroParty hero = Heroes.SingleOrDefault(hp => hp.Hero.ID == heroToAdd.ID);

		if (hero == null)
		{
			Heroes.Add (new HeroParty (heroToAdd, false));
		}
	}

	public void RemoveHeroFromParty (Hero heroToRemove)
	{
		HeroParty hero = Heroes.SingleOrDefault(hp => hp.Hero.ID == heroToRemove.ID);

		if (hero != null)
		{
			Heroes.Remove (hero);
		}
	}

	public HeroParty GetHeroFromParty (int index)
	{
		HeroParty hero = Heroes.SingleOrDefault(hp => hp.Hero.ID == index);

		if (hero != null)
		{
			return hero;
		}

		return null;
	}

	public int GetHeroLength ()
	{
		return Heroes.Count;
	}

	public void UpdateAchievments (AchievementType type, int count)
	{
		Game.current.achievementManager.UpdateAchievement (type, count);
	}
}
