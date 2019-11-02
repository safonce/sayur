using UnityEngine;
using System.Collections;

public class ExtendCustomMonoBehaviour : MonoBehaviour 
{
	protected Animator animator;
	protected Rigidbody rigidBody;
	protected CapsuleCollider capsuleCollider;
	protected AudioSource audioSource;
	protected UnityEngine.AI.NavMeshAgent navMeshAgent;

	protected virtual void Awake ()
	{
		animator = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody> ();
		capsuleCollider = GetComponent<CapsuleCollider> ();
		audioSource = GetComponent<AudioSource> ();
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent> ();

		SetupAnimator ();
	}

	void SetupAnimator ()
	{
		Animator[] childAnimators = GetComponentsInChildren<Animator> ();

		for (int i = 0; i < childAnimators.Length; i++)
		{
			if (childAnimators [i] != animator)
			{
				animator.avatar = childAnimators [i].avatar;
				Destroy (childAnimators [i]);

				break;
			}
		}
	}
}
