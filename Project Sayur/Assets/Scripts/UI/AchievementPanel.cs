using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AchievementPanel : MonoBehaviour 
{
	[SerializeField] Text titleText;
	[SerializeField] Text descriptionText;
	[SerializeField] Text rewardText;

	[SerializeField] AchievementButton achievementButton;

	public void SetAchievementPanel (PlayerAchievement data)
	{
		if (titleText != null)
			titleText.text = data.Achievement.Name;

		if (descriptionText != null)
			descriptionText.text = data.Achievement.Description;

		if (rewardText != null)
			rewardText.text = data.Achievement.RewardGold.ToString();

		if (achievementButton != null)
		{
			achievementButton.SetAchievementButton (data);
		}
	}
}
