using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour 
{
	[SerializeField] Text achievementText;
	[SerializeField] Button achievementButton;

	PlayerAchievement playerAchievement = null;

	public void GetReward ()
	{
		if (Game.current != null)
		{
			Game.current.Coins += playerAchievement.Achievement.RewardGold;
			LobbyManagerUI.Instance.RefreshCoins ();

			playerAchievement.IsRewarded = true;

			SetAchievementButton (playerAchievement);
		}
	}

	public void SetAchievementButton (PlayerAchievement data)
	{
		playerAchievement = data;

		if (achievementButton != null)
		{
			if (data.IsUnlocked)
			{
				if (data.IsRewarded)
				{
					achievementButton.interactable = false;
				} else
				{
					achievementButton.interactable = true;
				}
			} else
			{
				achievementButton.interactable = false;
			}
		}

		if (achievementText != null)
		{
			if (data.IsRewarded)
			{
				achievementText.text = "Complete";
			} else {
				achievementText.text = "Reward";
			}
		}
	}
}
