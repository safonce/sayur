using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlashFade : MonoBehaviour 
{
	[SerializeField] Color flashColor = new Color(1, 0, 0, .1f);
	[SerializeField] float flashSpeed = 5;

	Image image;

	void Awake ()
	{
		image = GetComponent<Image> ();
	}

	public void Flash ()
	{
		StopCoroutine ("FadeImage");

		image.color = flashColor;

		StartCoroutine ("FadeImage");
	}

	IEnumerator FadeImage ()
	{
		while (image.color.a > .01f)
		{
			image.color = Color.Lerp (image.color, Color.clear, flashSpeed * Time.deltaTime);
			yield return null;
		}

		image.color = Color.clear;
	}
}
