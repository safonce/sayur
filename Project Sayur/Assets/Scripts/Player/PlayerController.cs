using UnityEngine;
using System.Collections;

public class PlayerController : ExtendCustomMonoBehaviour, IDamageable
{
	[HideInInspector] public Vector3 MoveDirection;
	[HideInInspector] public Vector3 LookDirection;

	[Header("Movement Properties")]
	[SerializeField] float speed = 6;
	[SerializeField] float smoothDampTime = .1f;

	[Header("Health Properties")]
	[SerializeField] int maximumHealth = 100;
	[SerializeField] bool isInvulnerable = false;

	[Header ("Data Properties")]
	public Hero hero = null;
	public string title = "Awang";
	public string content = "Anak Melayu";
	public string avatarPath = "Avatars/";

	int currentHealth;
	bool isDead;

	float forwardAmount;
	float sidewayAmount;

	bool canMove = true;

	WeaponController weaponController;

	protected override void Awake ()
	{
		base.Awake ();

		weaponController = GetComponent<WeaponController> ();

		currentHealth = maximumHealth;
	}

	void FixedUpdate ()
	{
		if (!canMove)
			return;

		Move ();
		Turning ();
		Animating ();
	}

	void Move ()
	{
		MoveDirection.Set (MoveDirection.x, 0, MoveDirection.z);
		MoveDirection = MoveDirection.normalized;

		//Vector3 localMoveDirection = transform.InverseTransformDirection (MoveDirection);
		//forwardAmount = localMoveDirection.z;
		//sidewayAmount = localMoveDirection.x;

		rigidBody.MovePosition (transform.position + MoveDirection * speed * Time.deltaTime);
	}
	public Transform hips;
	void Turning ()
	{
		LookDirection.Set (LookDirection.x, 0, LookDirection.z);
		LookDirection = LookDirection.normalized;

		if (LookDirection != Vector3.zero)
		{
			//rigidBody.MoveRotation (Quaternion.LookRotation (LookDirection));
		}
	}

	void LateUpdate ()
	{
		if (LookDirection != Vector3.zero)
		{
			Vector3 pos = MouseLocation.Instance.MousePosition;
			pos.y = 1;
			hips.LookAt (pos);
		}
	}

	void Animating ()
	{
		animator.SetFloat ("Forward", forwardAmount, smoothDampTime, Time.deltaTime);
		animator.SetFloat ("Sideway", sidewayAmount, smoothDampTime, Time.deltaTime);
	}

	void Defeated ()
	{
		isDead = true;

		canMove = false;

		animator.SetTrigger ("Die");

		if (weaponController != null)
		{
			weaponController.DisableWeapon ();
		}

		GameManager.Instance.PlayerDied ();
	}

	public void TakeHit (int amount, RaycastHit hit)
	{
		TakeDamage (amount);
	}

	public void TakeDamage (int amount)
	{
		if (isDead || isInvulnerable)
			return;

		currentHealth -= amount;

		GameManagerUI.Instance.Flash ();

		if (currentHealth <= 0)
		{
			Defeated ();
		}
	}

	public void DeathComplete ()
	{
		GameManager.Instance.PlayerDeathComplete ();
	}

	public float GetHealthPercent ()
	{
		return (float) currentHealth / (float) maximumHealth;
	}

	public bool IsDead ()
	{
		return isDead;
	}
}
