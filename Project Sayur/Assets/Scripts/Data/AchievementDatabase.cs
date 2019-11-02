using System.Collections.Generic;
using System.Linq;

public static class AchievementDatabase
{
	private static readonly List<Achievement> achievements = new List<Achievement>();

	static AchievementDatabase ()
	{
		Achievement clearKiller = new Achievement (0, "Killer", "Kill 10 enemies", 100, 10, AchievementType.Kill);
		Achievement clearAssasin = new Achievement (1, "Assasin", "Kill 50 enemies", 500, 50, AchievementType.Kill);
		Achievement clearBountyHunter = new Achievement (2, "BountyHunter", "Collect 1000 gold", 200, 1000, AchievementType.Coin);

		achievements.Add (clearKiller);
		achievements.Add (clearAssasin);
		achievements.Add (clearBountyHunter);
	}

	public static int GetAchievementsLength ()
	{
		return achievements.Count;
	}

	public static Achievement GetAchievement (int id)
	{
		return achievements.SingleOrDefault (x => x.Id == id);
	}

	public static Achievement GetAchievement (string name)
	{
		return achievements.SingleOrDefault (x => x.Name == name);
	}
}
