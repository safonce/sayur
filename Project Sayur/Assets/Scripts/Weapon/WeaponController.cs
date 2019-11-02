using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : ExtendCustomMonoBehaviour 
{
	public Transform weaponHold;
	[SerializeField] string weaponPath = "Weapons/";

	[System.Serializable]
	public class WeaponOffset
	{
		public Vector3 positionOffset = Vector3.zero;
		public Vector3 rotationOffset = Vector3.zero;
	}
	[SerializeField] WeaponOffset[] weaponOffset;

	List<Weapon> weaponsList = new List<Weapon> ();

	Weapon currentWeapon;

	int selectedWeaponIndex = 0;
	int lastSelectedWeaponIndex = -1;

	bool canFire = true;
	bool switching;

	WaitForSeconds switchDelay = new WaitForSeconds (.8f);

	void Start()
	{
		CreateWeapon ();

		SetWeapon (0);
	}

	void CreateWeapon ()
	{
		for (int i = 0; i < Game.current.Bag.GetBagLength(); i++)
		{
			if (Game.current.Bag.weapons [i] == null)
				continue;
			
			Object weaponPrefab = Resources.Load (weaponPath + Game.current.Bag.weapons[i].Name);
			GameObject weaponClone = Instantiate (weaponPrefab) as GameObject;
			weaponClone.transform.SetParent (weaponHold, false);

			weaponClone.transform.localPosition = weaponOffset [i].positionOffset;
			weaponClone.transform.localRotation = Quaternion.Euler(weaponOffset [i].rotationOffset);

			Weapon newWeapon = weaponClone.GetComponent<Weapon> ();

			newWeapon.User = this;
			weaponsList.Add (newWeapon);

			weaponClone.SetActive (false);
		}
	}

	void DisableCurrentWeapon ()
	{
		if (WeaponIsEmpty ())
			return;

		currentWeapon = weaponsList [selectedWeaponIndex];
		currentWeapon.gameObject.SetActive (false);
	}

	void EnableCurrentWeapon ()
	{
		if (WeaponIsEmpty ())
			return;

		currentWeapon = weaponsList [selectedWeaponIndex];
		currentWeapon.gameObject.SetActive (true);
	}

	void AnimatePose ()
	{
		int weaponType = 0;

		if (currentWeapon == null)
			return;

		switch (currentWeapon.GetWeaponType())
		{
		case WeaponType.Pistol:
			weaponType = 1;
			break;
		case WeaponType.SMG:
			weaponType = 2;
			break;
		case WeaponType.Shotgun:
			weaponType = 3;
			break;
		}

		animator.SetInteger ("WeaponType", weaponType);
	}

	IEnumerator StopSwitching ()
	{
		yield return switchDelay;
		switching = false;
	}

	public void SetWeapon (int choosenWeaponIndex)
	{
		if (choosenWeaponIndex == lastSelectedWeaponIndex || !canFire || switching)
			return;

		switching = true;

		DisableCurrentWeapon ();

		selectedWeaponIndex = choosenWeaponIndex;

		if (selectedWeaponIndex < 0)
			selectedWeaponIndex = weaponsList.Count - 1;

		if (selectedWeaponIndex > weaponsList.Count - 1)
			selectedWeaponIndex = weaponsList.Count - 1;

		lastSelectedWeaponIndex = selectedWeaponIndex;

		EnableCurrentWeapon ();

		AnimatePose ();

		StartCoroutine ("StopSwitching");
	}

	public void NextWeapon (bool canLoop)
	{
		if (!canFire || switching)
			return;

		switching = true;

		DisableCurrentWeapon ();

		selectedWeaponIndex++;

		if (selectedWeaponIndex == weaponsList.Count)
		{
			if (canLoop)
			{
				selectedWeaponIndex = 0;
			} else
			{
				selectedWeaponIndex = weaponsList.Count - 1;
			}
		}

		lastSelectedWeaponIndex = selectedWeaponIndex;

		EnableCurrentWeapon ();

		AnimatePose ();

		StartCoroutine ("StopSwitching");
	}

	public void PreviousWeapon (bool canLoop)
	{
		if (!canFire || switching)
			return;

		switching = true;

		DisableCurrentWeapon ();

		selectedWeaponIndex--;

		if (selectedWeaponIndex < 0)
		{
			if (canLoop)
			{
				selectedWeaponIndex = weaponsList.Count - 1;
			} else
			{
				selectedWeaponIndex = 0;
			}
		}

		lastSelectedWeaponIndex = selectedWeaponIndex;

		EnableCurrentWeapon ();

		AnimatePose ();

		StartCoroutine ("StopSwitching");
	}
		
	public void FireWeapon()
	{
		if (currentWeapon == null || switching || !canFire)
			return;

		if (currentWeapon.IsFiring () || currentWeapon.IsReloading())
			return;
		
		if (currentWeapon.ClipAmmoIsEmpty ())
		{
			ReloadWeapon ();
			return;
		}
	
		Vector3 fireDirection = transform.TransformDirection (Vector3.forward);

		currentWeapon.Fire (fireDirection);
	}

	public void StopFiringWeapon ()
	{

	}

	public void ReloadWeapon ()
	{
		if (currentWeapon == null || switching || !canFire)
			return;

		if (currentWeapon.ClipAmmoIsFull() || currentWeapon.IsReloading())
			return;
		
		animator.SetTrigger ("Reload");

		currentWeapon.Reload ();
	}

	public bool WeaponIsEmpty ()
	{
		return weaponsList.Count <= 0;
	}

	public void DisableWeapon ()
	{
		canFire = false;
	}

	public Weapon GetCurrentWeapon ()
	{
		return currentWeapon;
	}

	public Weapon GetWeaponByIndex (int index)
	{
		if (WeaponIsEmpty ())
			return null;
		
		return weaponsList[index];
	}
}
