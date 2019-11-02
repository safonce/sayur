using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
	[HideInInspector] public WeaponController User;

	[SerializeField] int damage = 20;
	[SerializeField] float fireRate = .7f;
	[SerializeField] int maxClipAmmo = 20;
	[SerializeField] float reloadDuration = 3.6f;

	public WeaponType WeaponType = WeaponType.Pistol;
	[SerializeField] LayerMask Mask;

	[SerializeField] GameObject muzzleFlashPrefab = null;

	[SerializeField] int maximumBullet = 5;
	[SerializeField] GameObject bulletPrefab = null;

	public Transform bulletSpawn = null;

	[Header("Data Properties")]
	public string title;
	public string content;

	bool firing;
	int clipAmmo;

	bool reloading;
	MuzzleFlash muzzleFlash;

	protected List <Projectile> bulletsList = new List<Projectile>();

	WaitForSeconds fireDelay;
	WaitForSeconds reloadDelay;

	AudioSource audioSource;

	void Awake ()
	{
		audioSource = GetComponent<AudioSource> ();

		clipAmmo = maxClipAmmo;
	}

	void Start ()
	{
		fireDelay = new WaitForSeconds (fireRate);
		reloadDelay = new WaitForSeconds (reloadDuration);

		CreateMuzzleFlash ();
		CreateBullet ();
	}

	void OnEnable ()
	{
		firing = false;
		reloading = false;
	}

	void CreateMuzzleFlash ()
	{
		if (muzzleFlashPrefab == null || bulletSpawn == null)
			return;

		GameObject muzzleFlashClone = Instantiate (muzzleFlashPrefab) as GameObject;

		muzzleFlashClone.transform.SetParent (bulletSpawn, false);

		muzzleFlash = muzzleFlashClone.GetComponent<MuzzleFlash> ();
	}

	void CreateBullet ()
	{
		if (bulletPrefab == null)
			return;

		for (int i = 0; i < maximumBullet; i++)
		{
			GameObject bulletClone = Instantiate (bulletPrefab) as GameObject;
			bulletClone.SetActive (false);

			if (User != null)
			{
				Physics.GetIgnoreLayerCollision (bulletClone.layer, User.gameObject.layer);
			}

			Projectile bullet = bulletClone.GetComponent<Projectile> ();
			bullet.SetupProjectile (damage, Mask);

			if (bullet != null) 
				bulletsList.Add (bullet);
		}
	}

	void LoadClip()
	{
		clipAmmo = maxClipAmmo;
	}

	void PlayMuzzleFlash ()
	{
		if (muzzleFlash != null)
		{
			muzzleFlash.gameObject.SetActive (true);
			muzzleFlash.Play ();
		}
	}

	protected virtual void LaunchBullet (Vector3 direction)
	{
		if (bulletSpawn == null)
			return;

		for (int i = 0; i < bulletsList.Count; i++)
		{
			if (!bulletsList [i].gameObject.activeInHierarchy)
			{
				bulletsList [i].transform.position = bulletSpawn.position;

				bulletsList [i].SetPath (direction);
				bulletsList [i].gameObject.SetActive (true);

				return;
			}
		}
	}

	IEnumerator StopFiring()
	{
		yield return fireDelay;
		firing = false;
	}

	IEnumerator StopReloading()
	{
		yield return reloadDelay;
		LoadClip ();
		reloading = false;
	}

	public void Fire(Vector3 direction)
	{
		PlayMuzzleFlash ();
		LaunchBullet (direction);

		clipAmmo--;
		firing = true;

		StartCoroutine (StopFiring ());
	}

	public void Reload()
	{
		if (audioSource != null)
			audioSource.Play ();
		
		reloading = true;
		StartCoroutine (StopReloading ());
	}

	public bool ClipAmmoIsEmpty ()
	{
		return clipAmmo <= 0;
	}

	public bool ClipAmmoIsFull ()
	{
		return clipAmmo == maxClipAmmo;
	}

	public bool IsReloading ()
	{
		return reloading;
	}

	public bool IsFiring ()
	{
		return firing;
	}

	public int GetClipAmmo ()
	{
		return clipAmmo;
	}

	public int GetMaximumClipAmmo ()
	{
		return maxClipAmmo;
	}

	public WeaponType GetWeaponType ()
	{
		return WeaponType;
	}
}
