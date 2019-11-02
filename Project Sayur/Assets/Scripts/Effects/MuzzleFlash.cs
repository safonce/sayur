using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour 
{
	ParticleSystem ps;

	void Awake ()
	{
		ps = GetComponent<ParticleSystem> ();
	}

	void Update ()
	{
		if (!ps.isPlaying)
		{
			ps.Stop ();

			gameObject.SetActive (false);
		}
	}

	public void Play ()
	{
		Stop ();

		if (ps != null)
		{
			ps.Play (true);
		}
	}

	public void Stop ()
	{
		if (ps != null)
		{
			ps.Stop (true);
		}
	}
}
