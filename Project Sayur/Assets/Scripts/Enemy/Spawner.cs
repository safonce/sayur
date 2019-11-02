using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour 
{
	[Header("Spawner Properties")]
	[SerializeField] GameObject enemyPrefab = null;
	[SerializeField] float spawnRate = 5;
	[SerializeField] int maxEnemies = 10;

	[Header("Debugging Properties")]
	[SerializeField] bool loop = true;

	List<EnemyController> enemies = new List<EnemyController>();

	WaitForSeconds spawnDelay;

	bool canSpawn;

	void Awake ()
	{
		CreateEnemy ();

		spawnDelay = new WaitForSeconds (spawnRate);
	}

	IEnumerator Start ()
	{
		while (loop)
		{
			yield return spawnDelay;

			if (canSpawn)
				SpawnEnemy ();
		}
	}

	void CreateEnemy ()
	{
		if (enemyPrefab == null)
			return;
		
		for (int i = 0; i < maxEnemies; i++)
		{
			GameObject enemyClone = Instantiate (enemyPrefab) as GameObject;
			EnemyController enemy = enemyClone.GetComponent<EnemyController> ();

			enemyClone.transform.parent = transform;
			enemyClone.SetActive (false);

			enemies.Add (enemy);
		}
	}

	void SpawnEnemy ()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			if (!enemies [i].gameObject.activeSelf)
			{
				enemies [i].transform.position = transform.position;
				enemies [i].transform.rotation = transform.rotation;

				enemies [i].gameObject.SetActive (true);

				SpawnerManager.Instance.AddEnemy ();

				return;
			}
		}
	}

	public void EnableSpawner ()
	{
		canSpawn = true;
	}

	public void DisableSpawner ()
	{
		canSpawn = false;
	}
}
