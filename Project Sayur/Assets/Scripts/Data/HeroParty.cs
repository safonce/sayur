
[System.Serializable]
public class HeroParty
{
	public Hero Hero;
	public bool Unlocked;

	public HeroParty (Hero hero, bool unlocked)
	{
		Hero = hero;
		Unlocked = unlocked;
	}
}
