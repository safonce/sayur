using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	[SerializeField] float speed = 30;
	[SerializeField] float lifeDuration = 5;
	[SerializeField] float collisionRadius = .5f;

	Vector3 direction;
	int damage;
	LayerMask mask;

	float lifeTimer;
	bool isMoving;

	Ray ray;
	RaycastHit hit;

	TrailRenderer trail;

	void Awake ()
	{
		trail = GetComponentInChildren<TrailRenderer> ();
	}

	void OnEnable ()
	{
		if (trail != null)
			trail.Clear ();
		
		isMoving = true;
	}

	void Update ()
	{
		if (!isMoving)
			return;

		lifeTimer += Time.deltaTime;
		if (lifeTimer >= lifeDuration)
			StopProjectile ();

		float moveDistance = speed * Time.deltaTime;
		CheckCollision (moveDistance);

		transform.Translate (moveDistance * direction);
	}

	void StopProjectile ()
	{
		lifeTimer = 0;
		gameObject.SetActive (false);
	}

	void CheckCollision (float distance)
	{
		ray = new Ray (transform.position, direction);

		if (Physics.SphereCast (ray, collisionRadius, out hit, distance, mask, QueryTriggerInteraction.Ignore))
		{
			OnHitObject (hit);
		}
	}

	void OnHitObject (RaycastHit hit)
	{
		isMoving = false;

		IDamageable hitObject = hit.collider.GetComponent<IDamageable> ();
		if (hitObject != null)
		{
			hitObject.TakeHit (damage, hit);
		}

		gameObject.SetActive (false);
	}

	public void SetupProjectile (int newDamage, LayerMask newMask)
	{
		damage = newDamage;
		mask = newMask;
	}

	public void SetPath (Vector3 newDirection)
	{
		direction = newDirection;
	}

	public void OnDrawGizmosSelected ()
	{
		Gizmos.DrawWireSphere (transform.position, collisionRadius);
	}
}
