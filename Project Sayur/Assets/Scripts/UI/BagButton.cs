using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BagButton : MonoBehaviour
{
	[HideInInspector] public WeaponData weaponData = null;

	public void RemoveWeapon ()
	{
		if (weaponData == null)
		{
			LobbyManager.Instance.OpenWeaponPurchasedPanel ();
		} else
		{
			LobbyManager.Instance.RemoveWeaponFromBag (weaponData.ID);
		}
	}

	public void SetBagButton (WeaponData data)
	{
		weaponData = data;
	}
}
