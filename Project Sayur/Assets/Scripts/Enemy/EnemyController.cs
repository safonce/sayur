using UnityEngine;
using System.Collections;

public class EnemyController : ExtendCustomMonoBehaviour, IDamageable
{
	[Header("Movement Properties")]
	[SerializeField] float speed = 5f;
	[SerializeField] float smoothDampTime = .1f;

	[Header("Health Properties")]
	[SerializeField] int maximumHealth = 100;
	[SerializeField] bool isInvulnerable = false;
	[SerializeField] float deathDuration = 2;
	[SerializeField] int costValue = 10;
	[SerializeField] int experiencePointValue = 10;

	[Header("Attack Properties")]
	[SerializeField] float timeBetweenAttacks = 1;
	[SerializeField] int attackDamage = 10;	
	[SerializeField] float attackRange = 1;
	[SerializeField] Collider damageCollider = null;
	[SerializeField] LayerMask Mask;

	Vector3 moveDirection;
	float originalSpeed;
	float forwardAmount;

	int currentHealth;
	bool isDead;

	bool canAttack;
	bool playerInRange;
	float attackTimer;
	Transform target;

	WaitForSeconds updateDelay = new WaitForSeconds(.5f);

	void OnEnable ()
	{
		navMeshAgent.enabled = true;
		navMeshAgent.speed = Random.Range(1f, speed);

		currentHealth = maximumHealth;
		isDead = false;

		canAttack = true;

		capsuleCollider.isTrigger = false;
		rigidBody.isKinematic = false;

		StartCoroutine ("ChasePlayer");
	}

	void Update ()
	{	
		if (Input.GetKeyDown (KeyCode.T))
			TakeDamage (100);
		
		if (isDead)
			return;
		
		if (target == null)
		{
			SetTarget (GameManager.Instance.Player.transform);
		} else
		{
			CheckPlayerCondition ();
			AttackPlayer ();
		}

		Move ();
		Animating ();
	}

	IEnumerator ChasePlayer ()
	{
		yield return null;

		if (GameManager.Instance == null)
			yield break;

		while (navMeshAgent.enabled)
		{
			if (playerInRange)
			{
				moveDirection = Vector3.zero;
				navMeshAgent.Stop ();
			} else
			{
				moveDirection = navMeshAgent.desiredVelocity;
				SetDestination (target.position);
			}

			yield return updateDelay;
		}
	}

	void AttackPlayer()
	{
		if (!canAttack || target == null)
			return;

		playerInRange = false;

		float dist = (target.transform.position - transform.position).sqrMagnitude;
		if (dist <= attackRange)
			playerInRange = true;
		
		attackTimer += Time.deltaTime;
		if (attackTimer >= timeBetweenAttacks && playerInRange)
		{
			animator.SetTrigger ("Attack");

			attackTimer = 0;
		}
	}

	void Move ()
	{
		moveDirection.Set (moveDirection.x, 0, moveDirection.z);
		moveDirection = moveDirection.normalized;

		Vector3 localMoveDirection = transform.InverseTransformDirection (moveDirection);
		forwardAmount = localMoveDirection.z;
	}

	void Animating ()
	{
		animator.SetFloat ("Forward", forwardAmount, smoothDampTime, Time.deltaTime);
	}

	void SetDestination (Vector3 targetPos)
	{
		navMeshAgent.SetDestination (targetPos);
		navMeshAgent.Resume ();
	}

	void CheckPlayerCondition()
	{
		if (GameManager.Instance.Player == null)
			return;
		
		if (GameManager.Instance.Player.IsDead ())
		{
			animator.SetTrigger("PlayerDead");

			canAttack = false;

			navMeshAgent.enabled = false;
			rigidBody.isKinematic = true;
		}
	}

	void Defeated ()
	{
		isDead = true;

		canAttack = false;

		capsuleCollider.isTrigger = true;
		rigidBody.isKinematic = true;

		animator.SetTrigger ("Die");

		navMeshAgent.enabled = false;

		GameManager.Instance.AddCoins (costValue);
		Game.current.UpdateAchievments (AchievementType.Coin, costValue);

		if (Game.current != null)
			Game.current.AddExperiencePoints (experiencePointValue);

		Game.current.UpdateAchievments (AchievementType.Kill, 1);

		if (SpawnerManager.Instance != null)
			SpawnerManager.Instance.RemoveEnemy ();
		
		Invoke ("DeathComplete", deathDuration);
	}

	void DeathComplete ()
	{
		gameObject.SetActive (false);
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

		if (currentHealth <= 0)
		{
			Defeated ();
		}
	}

	public void SetTarget (Transform newTarget)
	{
		target = newTarget;
	}
		
	public void OpenDamageCollider ()
	{
		if (damageCollider != null)
		{
			damageCollider.enabled = true;
		}
	}

	public void CloseDamageCollider ()
	{
		if (damageCollider != null)
		{
			damageCollider.enabled = false;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if ((Mask.value & (1 << other.gameObject.layer)) > 0 && other.isTrigger == false)
		{
			IDamageable hitObject = other.GetComponent<IDamageable> ();
			if (hitObject != null)
			{
				hitObject.TakeDamage (attackDamage);
			}
		}
	}

	public bool IsDead ()
	{
		return isDead;
	}
}
