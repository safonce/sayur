using UnityEngine;
using System.Collections;

public class SpawnerManager : Singleton<SpawnerManager> 
{
	[SerializeField] int maximumEnemiesInField = 20;
	[SerializeField] Spawner[] spawners = null;

	int spawnedEnemyCount;

	void Update ()
	{
		if (spawners == null)
			return;
		
		if (spawnedEnemyCount >= maximumEnemiesInField)
		{
			for (int i = 0; i < spawners.Length; i++)
			{
				spawners [i].DisableSpawner ();
			}
		} else
		{
			for (int i = 0; i < spawners.Length; i++)
			{
				spawners [i].EnableSpawner ();
			}
		}
	}

	public void AddEnemy ()
	{
		spawnedEnemyCount++;
	}

	public void RemoveEnemy ()
	{
		spawnedEnemyCount--;
	}
}
