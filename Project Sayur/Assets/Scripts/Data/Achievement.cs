
[System.Serializable]
public class Achievement
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public int RewardGold { get; set; }
	public int CountToUnlock { get; set; }
	public AchievementType Type { get; set; }

	public Achievement (int id, string name, string description, int rewardGold, int countToUnlock, AchievementType type)
	{
		Id = id;
		Name = name;
		Description = description;
		RewardGold = rewardGold;
		CountToUnlock = countToUnlock;
		Type = type;
	}
}
