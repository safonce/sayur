using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class AchievementManager
{
	public List <PlayerAchievement> playerAchievements;
	private Dictionary<AchievementType, int> achievementKeeper;

	public AchievementManager ()
	{
		playerAchievements = new List<PlayerAchievement> ();

		achievementKeeper = new Dictionary<AchievementType, int> ();

		achievementKeeper.Add (AchievementType.Kill, 0);
		achievementKeeper.Add (AchievementType.Coin, 0);

		AddAchievementDatabaseToPlayerAchievement ();
	}

	public void AddAchievementToPlayerAchievement (Achievement achievementToAdd)
	{
		PlayerAchievement existingAchievementInPlayerAchievement = playerAchievements.SingleOrDefault(pa => pa.Achievement.Id == achievementToAdd.Id);

		if(existingAchievementInPlayerAchievement == null)
		{
			playerAchievements.Add (new PlayerAchievement(achievementToAdd, false, false));
		}
	}

	public void AddAchievementDatabaseToPlayerAchievement ()
	{
		for (int i = 0; i < AchievementDatabase.GetAchievementsLength (); i++)
		{
			Achievement achievement = AchievementDatabase.GetAchievement (i);
			AddAchievementToPlayerAchievement (achievement);
		}
	}

	public void UpdateAchievement (AchievementType type, int count)
	{
		if (!achievementKeeper.ContainsKey (type))
			return;
		
		achievementKeeper [type] += count;

		CheckAchievements (type);
	}

	void CheckAchievements (AchievementType type)
	{
		List<PlayerAchievement> availableAchievements = new List<PlayerAchievement> ();

		availableAchievements.AddRange (playerAchievements.FindAll ((PlayerAchievement pa) => ((pa.Achievement.Type == type) && (pa.IsUnlocked == false))));

		for (int i = 0; i < availableAchievements.Count; i++)
		{
			if (achievementKeeper [type] >= availableAchievements [i].Achievement.CountToUnlock)
			{
				availableAchievements [i].IsUnlocked = true;
			}
		}
	}
}
