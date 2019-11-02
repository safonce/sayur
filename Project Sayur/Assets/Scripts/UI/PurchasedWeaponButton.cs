using UnityEngine;
using System.Collections;

public class PurchasedWeaponButton : MonoBehaviour 
{
	[HideInInspector] public WeaponData weaponData = null;

	public void AddWeapon ()
	{
		if (weaponData == null)
			return;

		LobbyManager.Instance.AddWeaponToBag (weaponData.ID);
		LobbyManager.Instance.CloseWeaponPurchasedPanel ();
	}

	public void SetWeaponPurchasedButton (WeaponData data)
	{
		weaponData = data;
	}
}
