using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HiddenObject : MonoBehaviour 
{
	[SerializeField] LayerMask Mask;
	[SerializeField] string standardShaderPath = "Standard";
	[SerializeField] string transparentShaderPath = "Legacy Shaders/Transparent/Bumped Diffuse";
	[SerializeField] float alphaValue = .1f;

	static Transform player;

	List<GameObject> hiddenObjects = new List<GameObject>();

	void Start ()
	{
		if (GameManager.Instance.Player != null)
			player = GameManager.Instance.Player.transform;
	}

	void Update ()
	{
		if (player == null)
			return;
		
		Collider[] initialCollisions = Physics.OverlapSphere (transform.position, .1f);
		for (int i = 0; i < initialCollisions.Length; i++)
		{
			if (!hiddenObjects.Any (ho => (ho == initialCollisions [i].gameObject)))
			{
				hiddenObjects.Add (initialCollisions [i].gameObject);
				ChangeObjectToTransparent (initialCollisions [i].gameObject);
			}
		}

		Vector3 direction = player.position - transform.position;
		float distance = direction.magnitude;

		RaycastHit[] hits = Physics.RaycastAll (transform.position, direction, distance, Mask);

		for (int i = 0; i < hits.Length; i++)
		{
			if (!hiddenObjects.Any (ho => (ho == hits [i].collider.gameObject)))
			{
				hiddenObjects.Add (hits [i].collider.gameObject);
				ChangeObjectToTransparent (hits [i].collider.gameObject);
			}
		}

		for (int i = 0; i < hiddenObjects.Count; i++)
		{
			bool isHit = false;

			for (int j = 0; j < initialCollisions.Length; j++)
			{
				if (initialCollisions [j].gameObject == hiddenObjects [i])
				{
					isHit = true;
					break;
				}
			}

			for (int j = 0; j < hits.Length; j++)
			{
				if (hits [j].collider.gameObject == hiddenObjects [i])
				{
					isHit = true;
					break;
				}
			}

			if (!isHit)
			{
				RevertObjectBackToOriginal (hiddenObjects [i]);
				hiddenObjects.RemoveAt (i);
			}
		}
	}

	void ChangeObjectToTransparent (GameObject go)
	{
		Renderer mesh = go.GetComponentInChildren<Renderer> ();

		for (int i = 0; i < mesh.materials.Length; i++)
		{
			mesh.materials [i].shader = Shader.Find (transparentShaderPath);

			Color tempColor = mesh.materials [i].color;
			mesh.materials [i].color = new Color (tempColor.r, tempColor.g, tempColor.b, alphaValue);
		}
	}

	void RevertObjectBackToOriginal (GameObject go)
	{
		Renderer mesh = go.GetComponentInChildren<Renderer> ();

		for (int i = 0; i < mesh.materials.Length; i++)
		{
			mesh.materials [i].shader = Shader.Find(standardShaderPath);

			Color tempColor = mesh.materials [i].color;
			mesh.materials [i].color = new Color (tempColor.r, tempColor.g, tempColor.b, 1);
		}
	}
}
