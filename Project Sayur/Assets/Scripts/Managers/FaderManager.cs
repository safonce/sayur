using UnityEngine;
using System.Collections;

public class FaderManager : Singleton<FaderManager> 
{
	[SerializeField] Fader Fader = null;

	public void FaderOn (bool state, float duration)
	{
		if (Fader != null)
		{
			Fader.gameObject.SetActive (true);
			Fader.Fade (state, duration);
		}
	}
}
