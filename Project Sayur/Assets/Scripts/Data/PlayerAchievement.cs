using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerAchievement 
{
	public Achievement Achievement { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsRewarded { get; set; }

	public PlayerAchievement (Achievement achievement, bool isUnlocked, bool isRewarded)
	{
		Achievement = achievement;
		IsUnlocked = isUnlocked;
		IsRewarded = isRewarded;
	}

}
