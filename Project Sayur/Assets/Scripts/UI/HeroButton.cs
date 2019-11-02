using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour 
{
	[SerializeField] GameObject lockImage;
	[SerializeField] GameObject costPanel;

	[SerializeField] Text costText;

	HeroParty heroParty = null;

	Button button;

	void Awake ()
	{
		button = GetComponent<Button> ();
	}

	public void SetCharacter ()
	{
		if (heroParty == null)
			return;

		if (heroParty.Unlocked)
		{
			LobbyManager.Instance.SetHero (heroParty.Hero.ID);
		} else
		{
			LobbyManager.Instance.BuyCharacter (heroParty.Hero.ID);
		}
	}

	public void SetHeroButton (HeroParty data)
	{
		heroParty = data;

		if (Game.current.Level >= data.Hero.LevelToUnlock)
		{
			if (button != null)
				button.interactable = true;
			
			if (lockImage != null)
				lockImage.SetActive (false);
		} else
		{
			if (button != null)
				button.interactable = false;
			
			if (lockImage != null)
				lockImage.SetActive (true);
		}

		if (data.Unlocked)
		{
			if (costPanel != null)
				costPanel.SetActive (false);
		} else
		{
			if (costPanel != null)
				costPanel.SetActive (true);

			if (costText != null)
				costText.text = data.Hero.Cost.ToString();
		}
	}
}
