using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManagerUI : Singleton<GameManagerUI> 
{
	[SerializeField] GameObject HUD = null;
	[SerializeField] GameObject PauseSplash = null;
	[SerializeField] GameObject GameOverSplash = null;

	[SerializeField] CanvasGroup Joysticks = null;
	[SerializeField] CanvasGroup Buttons = null;

	[SerializeField] CanvasGroup ShootJoystick = null;
	[SerializeField] CanvasGroup ShootButton = null;

	[SerializeField] FlashFade FlashFader = null;
	[SerializeField] Text CoinsText = null;

	float initialJoystickAlpha;
	float initialButtonAlpha;

	protected override void Awake ()
	{
		base.Awake ();

		if (Joysticks != null)
		{
			initialJoystickAlpha = Joysticks.alpha;
		}
		if (Buttons != null)
		{
			initialButtonAlpha = Buttons.alpha;
		}
	}

	void Start ()
	{
		RefreshCoins ();

		if (Game.current != null)
		{
			SetAutoTargetActive (Game.current.AutoAimTarget);
		}
	}

	public void SetHUDActive (bool state)
	{
		if (HUD != null)
		{
			HUD.SetActive (state);
		}
	}

	public void SetMobileControlsActive (bool state)
	{
		if (Joysticks != null)
		{
			Joysticks.gameObject.SetActive (state);
			if (state)
			{
				Joysticks.alpha = initialJoystickAlpha;
			} else
			{
				Joysticks.alpha = 0;
			}
		}
		if (Buttons != null)
		{
			Buttons.gameObject.SetActive (state);
			if (state)
			{
				Buttons.alpha = initialButtonAlpha;
			} else
			{
				Buttons.alpha = 0;
			}
		}
	}

	public void SetAutoTargetActive (bool state)
	{
		if (state)
		{
			ShootButton.gameObject.SetActive (true);
			ShootJoystick.gameObject.SetActive (false);
		} else
		{
			ShootButton.gameObject.SetActive (false);
			ShootJoystick.gameObject.SetActive (true);
		}
	}

	public void Flash ()
	{
		if (FlashFader != null)
		{
			FlashFader.gameObject.SetActive (true);
			FlashFader.Flash ();
		}
	}

	public void SetPause (bool state)
	{
		if (PauseSplash != null)
		{
			PauseSplash.SetActive (state);
		}
	}

	public void SetGameOver (bool state)
	{
		if (GameOverSplash != null)
		{
			GameOverSplash.SetActive (state);
		}
	}

	public void RefreshCoins ()
	{
		if (CoinsText != null)
		{
			CoinsText.text = GameManager.Instance.Coins.ToString ("00000000");
		}
	}
}
