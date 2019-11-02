using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
	[SerializeField] Image foregroundSprite = null;
	[SerializeField] Color maximumHealthColor = new Color(0, 1, 0);
	[SerializeField] Color minimumHealthColor = new Color (1, 0, 0);

	static PlayerController player;

	void Start ()
	{
		if (GameManager.Instance != null)
			player = GameManager.Instance.Player;
	}

	void Update ()
	{
		if (player == null)
			return;

		float healthPercent = player.GetHealthPercent ();
		healthPercent = Mathf.Max (0, healthPercent);

		foregroundSprite.color = Color.Lerp (minimumHealthColor, maximumHealthColor, healthPercent);
		foregroundSprite.transform.localScale = new Vector3 (healthPercent, 1, 1);
	}
}
