using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[System.Serializable]
public class JoystickEvent : UnityEvent <Vector2> {}

public class TouchJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
	[Header("Camera")]
	[SerializeField] Camera TargetCamera = null;

	[Header("Axis")]
	[SerializeField] bool horizontalAxisEnabled = true;
	[SerializeField] bool verticalAxisEnabled = true;
	[SerializeField] float maxRange = 1.5f;

	[Header("Binding")]
	public JoystickEvent JoystickValue;
	public UnityEvent OnJoystickDown;
	public UnityEvent OnJoystickUp;

	Vector2 neutralPosition;
	Vector2 joystickValue;

	Vector2 newTargetPosition;
	Vector3 newJoystickPosition;
	float initialZPosition;
	RenderMode parentCanvasRenderMode;

	void Start ()
	{
		SetNeutralPosition ();
		if (TargetCamera == null)
		{
			throw new Exception ("You have to set a target camera");
		}
		parentCanvasRenderMode = GetComponentInParent<Canvas> ().renderMode;
		initialZPosition = transform.position.z;
	}

	void Update ()
	{
		if (JoystickValue != null)
		{
			if (horizontalAxisEnabled || verticalAxisEnabled)
			{
				JoystickValue.Invoke (joystickValue);
			}
		}
		if (OnJoystickDown != null)
		{
			if (horizontalAxisEnabled || verticalAxisEnabled)
			{
				if (joystickValue.sqrMagnitude > .1f)
				{
					OnJoystickDown.Invoke ();
				}
			}
		}
		if (OnJoystickUp != null)
		{
			if (horizontalAxisEnabled || verticalAxisEnabled)
			{
				if (joystickValue.sqrMagnitude <= .1f)
				{
					OnJoystickUp.Invoke ();
				}
			}
		}
	}


	void SetNeutralPosition ()
	{
		neutralPosition = GetComponent<RectTransform> ().transform.position;
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (parentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
		{
			newTargetPosition = TargetCamera.ScreenToWorldPoint (eventData.position);
		} else
		{
			newTargetPosition = eventData.position;
		}

		newTargetPosition = Vector2.ClampMagnitude (newTargetPosition - neutralPosition, maxRange);

		if (!horizontalAxisEnabled)
		{
			newTargetPosition.x = 0;
		}
		if (!verticalAxisEnabled)
		{
			newTargetPosition.y = 0;
		}

		joystickValue.x = EvaluateInputValue (newTargetPosition.x);
		joystickValue.y = EvaluateInputValue (newTargetPosition.y);

		newJoystickPosition = neutralPosition + newTargetPosition;
		newJoystickPosition.z = initialZPosition;

		transform.position = newJoystickPosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		newJoystickPosition = neutralPosition;
		newJoystickPosition.z = initialZPosition;
		transform.position = newJoystickPosition;
		joystickValue.x = 0;
		joystickValue.y = 0;
	}

	float EvaluateInputValue (float vectorPosition)
	{
		return Mathf.InverseLerp (0, maxRange, Mathf.Abs (vectorPosition)) * Mathf.Sign (vectorPosition);
	}
}
