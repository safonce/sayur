using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuUI : Singleton<MainMenuUI> 
{
	[SerializeField] GameObject warningPanel = null;

	[SerializeField] Button continueButton = null;

	public void SetWarningPanelActive (bool state)
	{
		if (warningPanel != null)
		{
			warningPanel.SetActive (state);
		}
	}

	public void setContinueButtonInteractable (bool state)
	{
		if (continueButton != null)
		{
			continueButton.interactable = state;
		}
	}
}
