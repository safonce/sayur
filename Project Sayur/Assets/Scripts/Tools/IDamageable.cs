using UnityEngine;
using System.Collections;

public interface IDamageable
{
	void TakeHit (int amount, RaycastHit hit);
	void TakeDamage (int amount);
}
