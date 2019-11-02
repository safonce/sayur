using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour 
{
	[SerializeField] GameObject inventoryButtonPrefab;
	[SerializeField] Transform inventoryPanelContent;
	[SerializeField] string weaponIconPath = "WeaponIcons/";

	void Start ()
	{
		CreateInventoryButtons ();
	}

	void CreateInventoryButtons ()
	{
		if (inventoryButtonPrefab == null || Game.current == null)
			return;

		for (int i = 0; i < Game.current.Bag.GetBagLength (); i++)
		{
			GameObject inventoryButtonClone = Instantiate (inventoryButtonPrefab) as GameObject;
			inventoryButtonClone.transform.SetParent (inventoryPanelContent, false);

			WeaponData weaponData = Game.current.Bag.weapons [i];
			if (weaponData == null)
				continue;
			
			Sprite sprite = Resources.Load (weaponIconPath + weaponData.Name, typeof(Sprite)) as Sprite;

			Image image = inventoryButtonClone.GetComponent<Image> ();
			if (image != null)
			{
				image.sprite = sprite;
			}

			InGameInventoryButton inventoryButton = inventoryButtonClone.GetComponent<InGameInventoryButton> ();
			inventoryButton.SetIndex (i);
		}
	}
}
