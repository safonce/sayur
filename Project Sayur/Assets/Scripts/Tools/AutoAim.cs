using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AutoAim : MonoBehaviour
{
	[HideInInspector] public Vector3 EnemyPosition;
	[HideInInspector] public bool IsValid;

	[SerializeField] LayerMask Mask;

	List<EnemyController> EnemiesInRange = new List<EnemyController> ();

	void Update ()
	{
		IsValid = false;

		if (EnemiesInRange.Count > 0)
		{
			IsValid = true;

			CheckEnemyCondition ();

			EnemyController closestEnemy = FindClosestEnemy ();

			if (closestEnemy != null)
			{
				EnemyPosition = closestEnemy.transform.position;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if ((Mask.value & (1 << other.gameObject.layer)) > 0)
		{
			EnemyController enemy = other.GetComponent<EnemyController> ();
			if (enemy != null)
			{
				AddEnemy (enemy);
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if ((Mask.value & (1 << other.gameObject.layer)) > 0)
		{
			EnemyController enemy = other.GetComponent<EnemyController> ();
			if (enemy != null)
			{
				RemoveEnemy (enemy);
			}
		}
	}

	void CheckEnemyCondition ()
	{
		for (int i = 0; i < EnemiesInRange.Count; i++)
		{
			if (EnemiesInRange [i].IsDead ())
				EnemiesInRange.RemoveAt (i);
		}
	}

	EnemyController FindClosestEnemy ()
	{
		EnemyController nearestEnemy = null;
		float minDistance = Mathf.Infinity;

		for (int i = 0; i < EnemiesInRange.Count; i++)
		{
			float distance = Vector3.Distance (EnemiesInRange [i].transform.position, transform.position);
			if (distance < minDistance)
			{
				nearestEnemy = EnemiesInRange [i];
				minDistance = distance;
			}
		}

		return nearestEnemy;
	}

	public void AddEnemy (EnemyController enemy)
	{
		EnemiesInRange.Add (enemy);
	}

	public void RemoveEnemy (EnemyController enemy)
	{
		EnemiesInRange.Remove (enemy);
	}

	public void RemoveAllEnemy ()
	{
		EnemiesInRange.Clear ();
	}
}
