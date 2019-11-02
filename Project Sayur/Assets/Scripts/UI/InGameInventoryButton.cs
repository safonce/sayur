using UnityEngine;
using System.Collections;

public class InGameInventoryButton : MonoBehaviour
{
	int index = 0;

	public void SetWeapon ()
	{
		InputManager.Instance.SetWeapon (index);
	}

	public void SetIndex (int index)
	{
		this.index = index;
	}
}
