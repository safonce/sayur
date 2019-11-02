using UnityEngine;
using System.Collections;

public class Shotgun : Weapon 
{
	[SerializeField] int bulletsPerShot = 5;
	[SerializeField] float angleBetweenBullets = 10;

	int bulletCount = 0;

	protected override void LaunchBullet (Vector3 direction)
	{
		if (bulletSpawn == null)
			return;

		float calculatedAngle = CalculateAngle ();

		for (int i = 0; i < bulletsList.Count; i++)
		{
			if (!bulletsList [i].gameObject.activeInHierarchy)
			{
				bulletsList [i].transform.position = bulletSpawn.position;

				Vector3 newDirection = Quaternion.Euler (0, calculatedAngle, 0) * direction;

				bulletsList [i].SetPath (newDirection);
				bulletsList [i].gameObject.SetActive (true);

				calculatedAngle -= angleBetweenBullets;
				bulletCount++;

				if (bulletCount >= bulletsPerShot)
				{
					bulletCount = 0;
					return;
				}
			}
		}
	}

	float CalculateAngle ()
	{
		return angleBetweenBullets * ((bulletsPerShot - 1) / 2);
	}
}
