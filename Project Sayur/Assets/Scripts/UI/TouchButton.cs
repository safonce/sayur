using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public UnityEvent ButtonPressedFirstTime;
	public UnityEvent ButtonReleased;
	public UnityEvent ButtonPressed;

	public float PressedOpacity = .5f;

	private bool zonePressed = false;
	private CanvasGroup canvasGroup;
	private float initialOpacity;

	private void Start ()
	{
		canvasGroup = GetComponent<CanvasGroup> ();
		if (canvasGroup != null)
		{
			initialOpacity = canvasGroup.alpha;
		}
	}

	private void Update ()
	{
		if (zonePressed)
		{
			OnPointerPressed ();
		}
	}

	public void OnPointerDown (PointerEventData data)
	{
		zonePressed = true;
		if (canvasGroup != null)
		{
			canvasGroup.alpha = PressedOpacity;
		}
		if (ButtonPressedFirstTime != null)
		{
			ButtonPressedFirstTime.Invoke ();
		}
	}

	public void OnPointerUp (PointerEventData data)
	{
		zonePressed = false;
		if (canvasGroup != null)
		{
			canvasGroup.alpha = initialOpacity;
		}
		if (ButtonReleased != null)
		{
			ButtonReleased.Invoke ();
		}
	}

	public void OnPointerPressed ()
	{
		if (ButtonPressed != null)
		{
			ButtonPressed.Invoke ();
		}
	}
}
