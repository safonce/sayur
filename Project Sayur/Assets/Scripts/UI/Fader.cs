using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
	[SerializeField] Color introColor = new Color (0, 0, 0, 0);
	[SerializeField] Color outroColor = new Color (0, 0, 0, 1);

	Image image;

	CanvasGroup canvasGroup;

	void Awake ()
	{
		image = GetComponent<Image> ();
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	public void Fade (bool state, float duration)
	{
		if (state)
		{
			StartCoroutine (FadeImage (duration, outroColor));
		} else
		{
			StartCoroutine (FadeImage (duration, introColor));
		}
	}

	IEnumerator FadeImage (float duration, Color color)
	{
		if (image == null)
			yield break;

		FaderBlock (true);

		float alpha = image.color.a;

		for (float t = 0; t < 1; t += Time.deltaTime / duration)
		{
			if (image == null)
				yield break;
			
			Color newColor = new Color (color.r, color.g, color.b, Mathf.SmoothStep (alpha, color.a, t));
			image.color = newColor;
			yield return null;
		}

		image.color = color;

		FaderBlock (false);
	}

	void FaderBlock (bool state)
	{
		canvasGroup.interactable = state;
		canvasGroup.blocksRaycasts = state;
	}
}
