using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager> 
{
	public enum InputForcedMode { None, Mobile, Desktop}

	[Header("Device Properties")]
	public bool AutoMobileDetection = true;
	public InputForcedMode ForcedMode;
	public bool IsMobile { get; private set; }

	Transform mainCamera;

	static PlayerController playerController;

	WeaponController weaponController;
	AutoAim autoAim;

	Vector3 moveDirection;
	Vector3 lookDirection;

	void Start ()
	{
		if (Camera.main != null)
			mainCamera = Camera.main.transform;

		if (GameManager.Instance.Player != null)
		{
			playerController = GameManager.Instance.Player;
		}

		if (playerController != null)
		{
			weaponController = playerController.GetComponent<WeaponController> ();
			autoAim = playerController.GetComponent<AutoAim> ();
		}

		if (GameManagerUI.Instance != null)
		{
			GameManagerUI.Instance.SetMobileControlsActive (false);
			IsMobile = false;
			if (AutoMobileDetection)
			{
				#if UNITY_ANDROID || UNITY_IPHONE
				GameManagerUI.Instance.SetMobileControlsActive (true);
			IsMobile = true;
				#endif
			}
			if (ForcedMode == InputForcedMode.Mobile)
			{
				GameManagerUI.Instance.SetMobileControlsActive (true);
				IsMobile = true;
			}
			if (ForcedMode == InputForcedMode.Desktop)
			{
				GameManagerUI.Instance.SetMobileControlsActive (false);
				IsMobile = false;
			}
		}
	}

	void Update ()
	{	
		if (IsMobile)
		{
			if (Game.current != null)
			{
				if (Game.current.AutoAimTarget)
					SetAutoLookDirection ();
			}

			return;
		}
		
		SetMoveDirection ();
		SetLookDirection ();
		
		if (Input.GetButtonDown ("Fire1") || Input.GetButton ("Fire1"))
		{
			FireWeapon ();
		}

		if (Input.GetButtonUp ("Fire1"))
		{
			StopFiring ();
		}

		if (Input.GetButtonUp ("Reload"))
		{
			ReloadWeapon ();
		}

		if (Input.GetKeyDown (KeyCode.Alpha1))
		{
			SetWeapon (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha2))
		{
			SetWeapon (1);
		} else if (Input.GetKeyDown (KeyCode.Alpha3))
		{
			SetWeapon (2);
		} else if (Input.GetKeyDown (KeyCode.Alpha3))
		{
			SetWeapon (3);
		}

		if (Input.GetAxis ("Mouse ScrollWheel") > 0)
		{
			NextWeapon ();
		} else if (Input.GetAxis ("Mouse ScrollWheel") < 0)
		{
			PreviouseWeapon ();
		}

		if (Input.GetKeyDown (KeyCode.P))
		{
			Pause ();
		}
	}

	void SetMoveDirection ()
	{
		if (playerController == null)
			return;
		
		if (!IsMobile)
		{
			moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			moveDirection = TransformCameraDirection (moveDirection);

			playerController.MoveDirection = moveDirection;
		}
	}

	public void SetMoveDirection (Vector2 input)
	{
		if (playerController == null)
			return;
		
		if (IsMobile)
		{
			moveDirection = new Vector3 (input.x, 0, input.y);
			moveDirection = TransformCameraDirection (moveDirection);

			playerController.MoveDirection = moveDirection;
		}
	}

	void SetLookDirection ()
	{
		if (playerController == null)
			return;

		if (!IsMobile)
		{
			if (MouseLocation.Instance != null && MouseLocation.Instance.IsValid)
			{
				lookDirection = (MouseLocation.Instance.MousePosition - playerController.transform.position).normalized;

				playerController.LookDirection = lookDirection;
			}
		}
	}

	public void SetLookDirection (Vector2 input)
	{
		if (playerController == null)
			return;

		if (IsMobile)
		{
			if (MouseLocation.Instance != null && MouseLocation.Instance.IsValid)
			{
				lookDirection = new Vector3 (input.x, 0, input.y);
				lookDirection = TransformCameraDirection (lookDirection);

				playerController.LookDirection = lookDirection;
			}
		}
	}

	void SetAutoLookDirection ()
	{
		if (playerController == null)
			return;
	
		if (IsMobile)
		{
			if (autoAim != null)
			{
				if (autoAim.IsValid)
				{
					lookDirection = (autoAim.EnemyPosition - playerController.transform.position).normalized;

					playerController.LookDirection = lookDirection;
				} else
				{
					playerController.LookDirection = moveDirection;
				}
			}
		}
	}

	public void ToggleAutoAimTarget ()
	{
		if (Game.current != null)
		{
			GameManager.Instance.ToggetAutoAimTarget ();

			autoAim.RemoveAllEnemy ();
			autoAim.enabled = Game.current.AutoAimTarget;
		}
	}

	public void FireWeapon ()
	{
		if (playerController == null)
			return;

		weaponController.FireWeapon ();
	}

	public void StopFiring ()
	{
		if (playerController == null)
			return;

		weaponController.StopFiringWeapon ();
	}

	public void ReloadWeapon ()
	{
		if (playerController == null)
			return;

		weaponController.ReloadWeapon ();
	}

	public void SetWeapon (int index)
	{
		if (playerController == null)
			return;

		weaponController.SetWeapon (index);
	}

	public void NextWeapon ()
	{
		if (playerController == null)
			return;

		weaponController.NextWeapon (true);
	}

	public void PreviouseWeapon ()
	{
		if (playerController == null)
			return;

		weaponController.PreviousWeapon (true);
	}

	public void Pause ()
	{
		GameManager.Instance.Pause ();
	}

	Vector3 TransformCameraDirection (Vector3 direction)
	{
		Vector3 newDirection = Vector3.zero;

		if (mainCamera != null)
		{
			Vector3 cameraForward = mainCamera.forward;
			cameraForward.y = 0;

			newDirection = direction.x * mainCamera.right + direction.z * cameraForward;
		} else
		{
			newDirection = direction.x * Vector3.right + direction.z * Vector3.forward;
		}

		return newDirection;
	}
}
