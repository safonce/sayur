using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
	[HideInInspector] public PlayerController Player;

	[SerializeField] string heroPath = "Characters/";
	[SerializeField] Transform playerSpawn = null;

	public int Coins { get; private set; }

	protected override void Awake ()
	{
		base.Awake ();

		if (Game.current == null)
			Game.current = new Game ();

		CreatePlayer ();
	}

	public void AddCoins (int coinsToAdd)
	{
		GameManager.Instance.Coins += coinsToAdd;

		GameManagerUI.Instance.RefreshCoins ();
	}

	public void SetCoins (int coins)
	{
		GameManager.Instance.Coins = coins;

		GameManagerUI.Instance.RefreshCoins ();
	}

	public void Pause ()
	{
		if (Time.timeScale > 0)
		{
			if (Game.current != null)
			{
				Game.current.Pause ();
			}

			if (GameManagerUI.Instance != null)
			{
				GameManagerUI.Instance.SetPause (true);
			}
		} else
		{
			UnPause ();
		}
	}

	public void UnPause ()
	{
		if (Game.current != null)
		{
			Game.current.UnPause ();
		}

		if (GameManagerUI.Instance != null)
		{
			GameManagerUI.Instance.SetPause (false);
		}
	}

	public void ToggetAutoAimTarget ()
	{
		if (Game.current != null)
		{
			Game.current.ToggleAutoAimTarget ();

			if (GameManagerUI.Instance != null)
				GameManagerUI.Instance.SetAutoTargetActive (Game.current.AutoAimTarget);
		}
	}

	void GameOver ()
	{
		if (GameManagerUI.Instance != null)
		{
			GameManagerUI.Instance.SetGameOver (true);
		}
	}

	public void PlayerDied ()
	{
		
	}

	public void PlayerDeathComplete ()
	{
		if (Game.current != null)
			Game.current.Coins += Coins;

		SaveLoad.Save ();

		GameOver ();
	}
		
	void CreatePlayer ()
	{
		if (playerSpawn == null)
			return;
		
		Hero hero = Game.current.CurrentHero;
		if (hero == null)
			return;
		
		Object playerPrefab = Resources.Load (heroPath + hero.Name);

		GameObject playerClone = Instantiate (playerPrefab, playerSpawn.position, playerSpawn.rotation) as GameObject;

		Player = playerClone.GetComponent<PlayerController> ();
	}

	public void KillAllEnemies ()
	{
		EnemyController[] enemies = SpawnerManager.Instance.GetComponentsInChildren<EnemyController> ();

		for (int i = 0; i < enemies.Length; i++)
		{
			enemies [i].TakeDamage (100);
		}
	}
}
