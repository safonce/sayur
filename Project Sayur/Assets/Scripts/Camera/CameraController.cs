using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	[Header("Camera Properties")]
	[SerializeField] Vector3 positionOffset = new Vector3 (0, 15, -22);
	[SerializeField] float lerpSpeed = 5;

	Transform target;

	void Start ()
	{
		FindPlayer ();
	}

	void FixedUpdate ()
	{
		if (target == null)
		{
			FindPlayer ();
		} else
		{
			FollowTarget ();
		}
	}

	void FindPlayer ()
	{
		if (GameManager.Instance.Player != null)
		{
			target = GameManager.Instance.Player.transform;
		}
	}

	void FollowTarget ()
	{
		Vector3 newPosition = Vector3.Lerp (transform.position, target.position + positionOffset, lerpSpeed * Time.deltaTime);
		transform.position = newPosition;
	}

	public void SetTarget (Transform newTarget)
	{
		target = newTarget;
	}
}
