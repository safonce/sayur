using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LobbyManager : Singleton<LobbyManager>
{
	[SerializeField] string heroPath = "Characters/";
	[SerializeField] Transform heroPlaceholder = null;

	[SerializeField] string weaponPath = "Weapons/";
	[SerializeField] Transform weaponPlaceholder = null;

	string levelName = string.Empty;

	public int selectedHeroIndex = 0;
	public int lastSelectedHeroIndex = -1;

	public int selectedWeaponIndex = 0;
	public int lastSelectedWeaponIndex = -1;

	List <GameObject> heroesList = new List<GameObject>();
	GameObject currentHero = null;

	List <GameObject> weaponsList = new List<GameObject>();
	GameObject currentWeapon = null;

	protected override void Awake ()
	{
		base.Awake ();

		if (Game.current == null)
			Game.current = new Game ();

		levelName = Game.current.CurrentLevel;

		CreateHeroes ();

		selectedHeroIndex = Game.current.CurrentHero.ID;

		SetHero (selectedHeroIndex);

		LobbyManagerUI.Instance.CreateHeroButtons ();

		LobbyManagerUI.Instance.CreateBagButtons ();

		CreateWeapons ();

		LobbyManagerUI.Instance.CreateWeaponButtons ();

		SetWeapon (0);

		LobbyManagerUI.Instance.CreatePurchasedWeaponButtons ();

		LobbyManagerUI.Instance.CreateAchievementPanels ();
	}

	void CheckHeroForUpdate ()
	{
		for (int i = 0; i < HeroDatabase.GetHeroesLength (); i++)
		{
			Hero hero = HeroDatabase.GetHero (i);
			Game.current.AddHeroToParty (hero);
		}
	}

	void CheckWeaponForUpdate ()
	{
		for (int i = 0; i < WeaponDatabase.GetWeaponsLength (); i++)
		{
			WeaponData weapon = WeaponDatabase.GetWeapon (i);
			Game.current.AddWeaponToInventory (weapon);
		}
	}

	void CreateHeroes ()
	{
		CheckHeroForUpdate ();

		if (heroPlaceholder == null)
		{
			Debug.LogError ("Hero placeholder cannot be found");
			return;
		}

		for (int i = 0; i < Game.current.GetHeroLength (); i++)
		{
			Hero hero = Game.current.Heroes [i].Hero;

			Object heroPrefab = Resources.Load (heroPath + hero.Name);
			if (heroPrefab == null)
			{
				Debug.LogError ("File cannot be found");
				break;
			}

			GameObject heroClone = Instantiate (heroPrefab) as GameObject;
			heroClone.transform.SetParent (heroPlaceholder, false);
			heroClone.SetActive (false);

			PlayerController playerController = heroClone.GetComponent<PlayerController> ();
			if (playerController != null)
				playerController.enabled = false;

			WeaponController weaponController = heroClone.GetComponent<WeaponController> ();
			if (weaponController != null)
				weaponController.enabled = false;

			heroesList.Add (heroClone);
		}
	}

	void DisableCurrentHero ()
	{
		if (HeroListIsEmpty ())
			return;

		currentHero = heroesList [selectedHeroIndex];
		currentHero.SetActive (false);
	}

	void EnableCurrentHero ()
	{
		if (HeroListIsEmpty ())
			return;

		currentHero = heroesList [selectedHeroIndex];
		currentHero.SetActive (true);
	}

	bool HeroListIsEmpty ()
	{
		return heroesList.Count <= 0;
	}

	public void SetHero (int choosenHeroIndex)
	{
		if (choosenHeroIndex == lastSelectedHeroIndex)
			return;

		DisableCurrentHero ();

		selectedHeroIndex = choosenHeroIndex;

		if (selectedHeroIndex < 0)
			selectedHeroIndex = heroesList.Count - 1;

		if (choosenHeroIndex > heroesList.Count - 1)
			selectedHeroIndex = heroesList.Count - 1;

		lastSelectedHeroIndex = selectedHeroIndex;

		EnableCurrentHero ();

		LobbyManagerUI.Instance.RefreshHeroDescription ();

		LobbyManagerUI.Instance.RefreshSelectButton ();
	}

	public void SelectCharacter ()
	{
		if (Game.current == null)
			return;
		
		Game.current.CurrentHero = Game.current.Heroes [selectedHeroIndex].Hero;
		int index = Game.current.CurrentHero.ID;

		SetHero (index);

		SetCharacterPanelActive (false);

		SaveLoad.Save ();
	}

	public void CancelCharacter ()
	{
		if (Game.current == null)
			return;
		
		int index = Game.current.CurrentHero.ID;

		SetHero (index);

		SetCharacterPanelActive (false);
	}

	public void BuyCharacter (int choosenHeroIndex)
	{
		if (Game.current == null)
			return;

		HeroParty heroParty = Game.current.GetHeroFromParty (choosenHeroIndex);
		if (heroParty == null)
			return;
		
		if (Game.current.Coins >= heroParty.Hero.Cost)
		{
			Game.current.Coins -= heroParty.Hero.Cost;
			heroParty.Unlocked = true;

			if (LobbyManagerUI.Instance != null)
			{
				LobbyManagerUI.Instance.RefreshCoins ();
				LobbyManagerUI.Instance.RefreshHeroButtons ();
			}

			SetHero (choosenHeroIndex);

			SaveLoad.Save ();
		}
	}

	public void SetCharacterPanelActive (bool state)
	{
		LobbyManagerUI.Instance.RefreshHeroDescription ();

		LobbyManagerUI.Instance.RefreshSelectButton ();

		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.SetCharacterPanelActive (state);
		}
	}

	void CreateWeapons ()
	{
		CheckWeaponForUpdate ();

		if (weaponPlaceholder == null)
		{
			Debug.LogError ("Weapon placeholder cannot be found");
			return;
		}

		for (int i = 0; i < Game.current.GetInventoryLength(); i++)
		{
			WeaponData weaponData = Game.current.InventoryWeapons [i].Weapon;
			if (weaponData == null)
				return;
			
			Object weaponPrefab = Resources.Load (weaponPath + weaponData.Name);
			if (weaponPrefab == null)
			{
				Debug.LogWarning ("File cannot be found");
				return;
			}

			GameObject weaponClone = Instantiate (weaponPrefab) as GameObject;
			weaponClone.transform.SetParent (weaponPlaceholder, false);
			weaponClone.SetActive (false);

			Weapon weapon = weaponClone.GetComponent<Weapon> ();
			if (weapon != null)
				weapon.enabled = false;

			weaponsList.Add (weaponClone);
		}
	}

	void DisableCurrentWeapon ()
	{
		if (WeaponsListIsEmpty ())
			return;

		currentWeapon = weaponsList [selectedWeaponIndex];
		currentWeapon.SetActive (false);
	}

	void EnableCurrentWeapon ()
	{
		if (WeaponsListIsEmpty ())
			return;

		currentWeapon = weaponsList [selectedWeaponIndex];
		currentWeapon.SetActive (true);
	}

	public void SetWeapon (int choosenWeaponIndex)
	{
		if (choosenWeaponIndex == lastSelectedWeaponIndex)
			return;

		DisableCurrentWeapon ();

		selectedWeaponIndex = choosenWeaponIndex;

		if (selectedWeaponIndex < 0)
			selectedWeaponIndex = weaponsList.Count - 1;

		if (choosenWeaponIndex > weaponsList.Count - 1)
			selectedWeaponIndex = weaponsList.Count - 1;

		lastSelectedWeaponIndex = selectedWeaponIndex;

		EnableCurrentWeapon ();

		LobbyManagerUI.Instance.RefreshWeaponDescription ();
		LobbyManagerUI.Instance.RefreshEquipRemoveButton ();
	}

	public void BuyWeapon (int choosenWeaponIndex)
	{
		if (Game.current == null)
			return;

		InventoryWeapon inventoryWeapon = Game.current.InventoryWeapons [choosenWeaponIndex];
		if (inventoryWeapon == null)
			return;

		if (Game.current.Coins >= inventoryWeapon.Weapon.Cost)
		{
			Game.current.Coins -= inventoryWeapon.Weapon.Cost;
			inventoryWeapon.Unlocked = true;

			if (LobbyManagerUI.Instance != null)
			{
				LobbyManagerUI.Instance.RefreshCoins ();
				LobbyManagerUI.Instance.RefreshWeaponButtons ();
			}

			SetWeapon (choosenWeaponIndex);

			SaveLoad.Save ();
		}
	}

	bool WeaponsListIsEmpty ()
	{
		return weaponsList.Count <= 0;
	}

	public void AddWeaponToBag ()
	{
		if (Game.current == null)
			return;

		Game.current.Bag.AddWeapon (selectedWeaponIndex);

		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.AddBagButton (selectedWeaponIndex);
			LobbyManagerUI.Instance.RefreshEquipRemoveButton ();
		}

		SaveLoad.Save ();
	}

	public void AddWeaponToBag (int chooseWeaponIndex)
	{
		if (Game.current == null)
			return;

		Game.current.Bag.AddWeapon (chooseWeaponIndex);

		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.AddBagButton (chooseWeaponIndex);
			LobbyManagerUI.Instance.RefreshEquipRemoveButton ();
		}

		SaveLoad.Save ();
	}

	public void OpenWeaponPurchasedPanel ()
	{
		LobbyManagerUI.Instance.FillPurchasedPanel ();

		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.SetPurchasedWeaponPanelActive (true);
		}
	}

	public void CloseWeaponPurchasedPanel ()
	{
		LobbyManagerUI.Instance.ClearPurchasedPanel ();

		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.SetPurchasedWeaponPanelActive (false);
		}
	}

	public void OkWarningPanel ()
	{
		CloseWarningPanel ();

		OpenWeaponPurchasedPanel ();
	}

	public void OpenWarningPanel ()
	{
		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.SetWarningPanelActive (true);
		}
	}

	public void CloseWarningPanel ()
	{
		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.SetWarningPanelActive (false);
		}
	}

	public void RemoveWeaponFromBag ()
	{
		if (Game.current == null)
			return;

		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.RemoveBagButton (selectedWeaponIndex);
		}

		Game.current.Bag.RemoveWeapon (selectedWeaponIndex);

		LobbyManagerUI.Instance.RefreshEquipRemoveButton ();

		SaveLoad.Save ();
	}

	public void RemoveWeaponFromBag (int chooseWeaponIndex)
	{
		if (Game.current == null)
			return;

		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.RemoveBagButton (chooseWeaponIndex);
		}

		Game.current.Bag.RemoveWeapon (chooseWeaponIndex);
	
		LobbyManagerUI.Instance.RefreshEquipRemoveButton ();

		SaveLoad.Save ();
	}

	public void SetWeaponPanelActive (bool state)
	{
		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.SetWeaponPanelActive (state);
		}
	}

	public void SetAchievementPanelActive (bool state)
	{
		if (LobbyManagerUI.Instance != null)
		{
			LobbyManagerUI.Instance.SetAchievementPanelActive (state);
		}
	}

	public void StartGame ()
	{
		if (Game.current == null)
			return;
		
		if (Game.current.Bag.InventoryIsEmpty())
		{
			OpenWarningPanel ();
		} else
		{
			LevelManager.Instance.GoToLevel (levelName);
		}
	}
}
