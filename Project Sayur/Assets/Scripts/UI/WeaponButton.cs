using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
	[SerializeField] GameObject lockImage;
	[SerializeField] GameObject costPanel;

	[SerializeField] Text costText;

	InventoryWeapon weaponData = null;

	Button button;

	void Awake ()
	{
		button = GetComponent<Button> ();
	}

	public void SetWeapon ()
	{
		if (weaponData == null)
			return;

		if (weaponData.Unlocked)
		{
			LobbyManager.Instance.SetWeapon (weaponData.Weapon.ID);
		} else
		{
			LobbyManager.Instance.BuyWeapon (weaponData.Weapon.ID);
		}
	}

	public void SetWeaponButton (InventoryWeapon data)
	{
		weaponData = data;

		if (Game.current.Level >= data.Weapon.LevelToUnlocked)
		{
			if (button != null)
				button.interactable = true;

			if (lockImage != null)
				lockImage.SetActive (false);
		} else
		{
			if (button != null)
				button.interactable = false;

			if (lockImage != null)
				lockImage.SetActive (true);
		}

		if (data.Unlocked)
		{
			if (costPanel != null)
				costPanel.SetActive (false);
		} else
		{
			if (costPanel != null)
				costPanel.SetActive (true);

			if (costText != null)
				costText.text = data.Weapon.Cost.ToString();
		}
	}
}
