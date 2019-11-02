using UnityEngine;
using System.Collections;

public class MouseLocation : Singleton<MouseLocation> 
{
	[HideInInspector] public Vector3 MousePosition;
	[HideInInspector] public bool IsValid;

	[SerializeField] LayerMask groundMask;

	Ray ray;
	RaycastHit hit;
	float camRayLength = 100;

	void Update ()
	{
		IsValid = false;

		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, camRayLength, groundMask))
		{
			IsValid = true;
			MousePosition = hit.point;
		}
	}
}
