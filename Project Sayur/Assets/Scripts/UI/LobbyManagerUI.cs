using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyManagerUI : Singleton<LobbyManagerUI> 
{
	[SerializeField] Text levelText = null;
	[SerializeField] Text coinsText = null;

	[SerializeField] string avatarPath = "Avatars/";
	[SerializeField] GameObject heroButtonPrefab = null;
	[SerializeField] Transform heroPanelContent = null;
	[SerializeField] GameObject characterPanel = null;
	[SerializeField] Text heroName = null;
	[SerializeField] Text heroDescription = null;
	[SerializeField] Button selectButton = null;

	[SerializeField] GameObject bagButtonPrefab;
	[SerializeField] Transform bagPanelContent;

	[SerializeField] string weaponIconPath = "WeaponIcons/";
	[SerializeField] GameObject weaponButtonPrefab;
	[SerializeField] Transform weaponPanelContent;
	[SerializeField] Text weaponName = null;
	[SerializeField] Text weaponDetail = null;
	[SerializeField] GameObject weaponPanel = null;
	[SerializeField] Button equipButton = null;
	[SerializeField] Button removeButton = null;
	[SerializeField] Text damageText = null;
	[SerializeField] Text fireRateText = null;
	[SerializeField] Text capacityText = null;

	[SerializeField] GameObject achievementPanelPrefab = null;
	[SerializeField] Transform achievementPanelContent = null;
	[SerializeField] GameObject achievementPanel = null;

	[SerializeField] GameObject purchasedWeaponButtonPrefab;
	[SerializeField] Transform purchasedWeaponPanelContent;
	[SerializeField] GameObject purchasedWeaponPanel;

	[SerializeField] GameObject warningPanel = null;

	List<HeroButton> HeroButtons = new List<HeroButton> ();
	List<WeaponButton> WeaponButtons = new List<WeaponButton> ();
	List<PurchasedWeaponButton> PurchasedWeaponButtons = new List<PurchasedWeaponButton>();

	BagButton[] bagButtons;

	void Start ()
	{
		RefreshCoins ();
		RefreshLevel ();
	}

	public void RefreshCoins ()
	{
		if (coinsText.text != null)
		{
			coinsText.text = Game.current.Coins.ToString ("00000000");
		}
	}

	public void RefreshLevel ()
	{
		if (levelText.text != null)
		{
			levelText.text = "Level " + Game.current.Level.ToString ();
		}
	}

	public void CreateHeroButtons ()
	{
		if (heroButtonPrefab == null || heroPanelContent == null)
			return;

		for (int i = 0; i < Game.current.Heroes.Count; i++)
		{
			GameObject heroButtonClone = Instantiate (heroButtonPrefab) as GameObject;
			heroButtonClone.transform.SetParent (heroPanelContent, false);

			HeroParty heroParty = Game.current.Heroes [i];
			if (heroParty == null)
				return;
			
			Sprite sprite = Resources.Load (avatarPath + heroParty.Hero.Name, typeof(Sprite)) as Sprite;
			if (sprite != null)
			{
				Image image = heroButtonClone.GetComponent<Image> ();
				if (image != null)
				{
					image.sprite = sprite;
				}
			}

			HeroButton heroButton = heroButtonClone.GetComponent<HeroButton> ();
			if (heroButton != null)
			{
				heroButton.SetHeroButton (heroParty);

				HeroButtons.Add (heroButton);
			}
		}
	}

	public void RefreshHeroButtons ()
	{
		if (HeroButtons == null)
			return;
		
		for (int i = 0; i < HeroButtons.Count; i++)
		{
			HeroParty heroParty = Game.current.GetHeroFromParty (i);

			if (heroParty != null)
			{
				HeroButtons [i].SetHeroButton (heroParty);
			}
		}
	}

	public void CreateBagButtons ()
	{
		if (bagButtonPrefab == null ||bagPanelContent == null || Game.current == null)
			return;

		bagButtons = new BagButton[Game.current.Bag.GetBagLength()];

		for (int i = 0; i < bagButtons.Length; i++)
		{
			GameObject bagButtonClone = Instantiate (bagButtonPrefab) as GameObject;
			bagButtonClone.transform.SetParent (bagPanelContent, false);

			BagButton bagButton = bagButtonClone.GetComponent<BagButton> ();

			WeaponData weaponData = Game.current.Bag.weapons [i];

			if (weaponData != null)
			{
				Sprite sprite = Resources.Load (weaponIconPath + weaponData.Name, typeof(Sprite)) as Sprite;
				if (sprite != null)
				{
					Image image = bagButton.GetComponent<Image> ();
					if (image != null)
					{
						image.sprite = sprite;
					}
				}
			}

			bagButton.SetBagButton (weaponData);

			bagButtons [i] = bagButton;
		}
	}

	public void AddBagButton (int weaponIndex)
	{
		if (BagIsFull ())
			return;

		bool isExist = false;
		for (int i = 0; i < bagButtons.Length; i++)
		{
			WeaponData weaponData = bagButtons [i].weaponData;
			if (weaponData == null)
				continue;
			
			if (bagButtons [i].weaponData.ID == weaponIndex)
			{
				isExist = true;
				break;
			}
		}

		if (!isExist)
		{
			for (int i = 0; i < bagButtons.Length; i++)
			{
				if (bagButtons [i].weaponData == null)
				{
					WeaponData weaponData = Game.current.InventoryWeapons[weaponIndex].Weapon;
					if (weaponData == null)
						return;

					Sprite sprite = Resources.Load (weaponIconPath + weaponData.Name, typeof(Sprite)) as Sprite;
					if (sprite != null)
					{
						Image image = bagButtons [i].GetComponent<Image> ();
						if (image != null)
						{
							image.sprite = sprite;
						}
					}

					bagButtons[i].SetBagButton (weaponData);

					return;
				}
			}
		}
	}

	public void RemoveBagButton (int weaponIndex)
	{
		for (int i = 0; i < bagButtons.Length; i++)
		{
			if (bagButtons [i].weaponData == null)
				continue;

			if (Game.current.Bag.weapons[i].ID == weaponIndex)
			{
				Image image = bagButtons[i].GetComponent<Image> ();
				if (image != null)
				{
					image.sprite = null;
				}

				bagButtons[i].SetBagButton (null);

				return;
			}
		}
	}

	bool BagIsFull ()
	{
		for (int i = 0; i < bagButtons.Length; i++)
		{
			if (bagButtons [i].weaponData == null)
			{
				return false;
			}
		}

		return true;
	}

	public void CreateWeaponButtons ()
	{
		if (weaponButtonPrefab == null || weaponPanelContent == null)
			return;

		for (int i = 0; i < Game.current.InventoryWeapons.Count; i++)
		{
			GameObject weaponButtonClone = Instantiate (weaponButtonPrefab) as GameObject;
			weaponButtonClone.transform.SetParent (weaponPanelContent, false);

			InventoryWeapon weaponData = Game.current.InventoryWeapons [i];
			if (weaponData == null)
				return;

			Sprite sprite = Resources.Load (weaponIconPath + weaponData.Weapon.Name, typeof(Sprite)) as Sprite;
			if (sprite != null)
			{
				Image image = weaponButtonClone.GetComponent<Image> ();
				if (image != null)
				{
					image.sprite = sprite;
				}
			}

			WeaponButton weaponButton = weaponButtonClone.GetComponent<WeaponButton> ();
			if (weaponButton != null)
			{
				weaponButton.SetWeaponButton (weaponData);

				WeaponButtons.Add (weaponButton);
			}
		}
	}

	public void RefreshWeaponButtons ()
	{
		if (WeaponButtons == null)
			return;

		for (int i = 0; i < WeaponButtons.Count; i++)
		{
			InventoryWeapon weaponData = Game.current.InventoryWeapons [i];

			if (weaponData != null)
			{
				WeaponButtons [i].SetWeaponButton (weaponData);
			}
		}
	}

	public void RefreshHeroDescription ()
	{
		int index = LobbyManager.Instance.selectedHeroIndex;

		HeroParty heroParty = Game.current.GetHeroFromParty (index);

		if (heroName != null)
		{
			heroName.text = heroParty.Hero.Name;
		}

		if (heroDescription != null)
		{
			heroDescription.text = heroParty.Hero.Description;
		}
	}
		
	public void RefreshWeaponDescription ()
	{
		int index = LobbyManager.Instance.selectedWeaponIndex;

		WeaponData weaponData = Game.current.InventoryWeapons [index].Weapon;

		if (weaponName != null)
		{
			weaponName.text = weaponData.Name;
		}

		if (weaponDetail != null)
		{
			weaponDetail.text = weaponData.Description;
		}

		if (damageText != null)
		{
			damageText.text = weaponData.Damage.ToString();
		}

		if (fireRateText != null)
		{
			fireRateText.text = weaponData.FireRate.ToString();
		}

		if (capacityText != null)
		{
			capacityText.text = weaponData.Capacity.ToString ();
		}
	}

	public void RefreshSelectButton ()
	{
		if (Game.current == null)
			return;

		int index = LobbyManager.Instance.selectedHeroIndex;

		if (Game.current.CurrentHero.ID == index)
		{
			if (selectButton != null)
				selectButton.interactable = false;
		} else
		{
			if (selectButton != null)
				selectButton.interactable = true;
		}
	}

	public void RefreshEquipRemoveButton ()
	{
		if (Game.current == null)
			return;

		int index = LobbyManager.Instance.selectedWeaponIndex;

		if (Game.current.InventoryWeapons[index].Unlocked)
		{
			if (Game.current.Bag.WeaponIsInBag (index))
			{
				if (equipButton != null) 
					equipButton.interactable = false;

				if (removeButton != null)
					removeButton.interactable = true;
			} else
			{
				if (equipButton != null) 
					equipButton.interactable = true;

				if (removeButton != null)
					removeButton.interactable = false;
			}
		} else
		{
			if (equipButton != null) 
				equipButton.interactable = false;

			if (removeButton != null)
				removeButton.interactable = false;
		}
	
	}

	public void CreatePurchasedWeaponButtons ()
	{
		if (purchasedWeaponButtonPrefab == null ||purchasedWeaponPanelContent == null || Game.current == null)
			return;

		for (int i = 0; i < Game.current.InventoryWeapons.Count; i++)
		{
			GameObject weaponPurchasedButtonClone = Instantiate (purchasedWeaponButtonPrefab) as GameObject;
			weaponPurchasedButtonClone.transform.SetParent (purchasedWeaponPanelContent, false);

			PurchasedWeaponButton purchasedWeaponButton = weaponPurchasedButtonClone.GetComponent<PurchasedWeaponButton> ();

			PurchasedWeaponButtons.Add (purchasedWeaponButton);
		}
	}

	public void FillPurchasedPanel ()
	{
		for (int i = 0; i < PurchasedWeaponButtons.Count; i++)
		{
			InventoryWeapon weaponData = Game.current.InventoryWeapons [i];

			if (weaponData.Unlocked && !Game.current.Bag.WeaponIsInBag (weaponData.Weapon.ID))
			{
				if (weaponData != null)
				{
					Sprite sprite = Resources.Load (weaponIconPath + weaponData.Weapon.Name, typeof(Sprite)) as Sprite;
					if (sprite != null)
					{
						Image image = PurchasedWeaponButtons [i].GetComponent<Image> ();
						if (image != null)
						{
							image.sprite = sprite;
						}
					}
				}

				PurchasedWeaponButtons [i].SetWeaponPurchasedButton (weaponData.Weapon);
			}
		}
	}

	public void ClearPurchasedPanel ()
	{
		for (int i = 0; i < PurchasedWeaponButtons.Count; i++)
		{
			Image image = PurchasedWeaponButtons[i].GetComponent<Image> ();
			if (image != null)
			{
				image.sprite = null;
			}

			PurchasedWeaponButtons[i].SetWeaponPurchasedButton (null);
		}
	}

	public void SetCharacterPanelActive(bool state)
	{
		if (characterPanel != null)
		{
			characterPanel.SetActive (state);
		}
	}

	public void SetWeaponPanelActive(bool state)
	{
		if (weaponPanel != null)
		{
			weaponPanel.SetActive (state);
		}
	}

	public void SetPurchasedWeaponPanelActive (bool state)
	{
		if (purchasedWeaponPanel != null)
		{
			purchasedWeaponPanel.SetActive (state);
		}
	}

	public void SetAchievementPanelActive(bool state)
	{
		if (achievementPanel != null)
		{
			achievementPanel.SetActive (state);
		}
	}

	public void SetWarningPanelActive (bool state)
	{
		if (warningPanel != null)
		{
			warningPanel.SetActive (state);
		}
	}

	public void CreateAchievementPanels ()
	{
		if (achievementPanelPrefab == null || achievementPanelContent == null)
			return;

		for (int i = 0; i < Game.current.achievementManager.playerAchievements.Count; i++)
		{
			GameObject achivementPanelClone = Instantiate (achievementPanelPrefab) as GameObject;
			achivementPanelClone.transform.SetParent (achievementPanelContent, false);

			PlayerAchievement playerAchievement = Game.current.achievementManager.playerAchievements [i];
			if (playerAchievement == null)
				return;

			AchievementPanel achievementButton = achivementPanelClone.GetComponent<AchievementPanel> ();
			if (achievementButton != null)
			{
				achievementButton.SetAchievementPanel (playerAchievement);
			}
		}
	}
}
