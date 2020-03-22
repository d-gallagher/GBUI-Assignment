using UnityEngine;

/// <summary>
/// Interface defining behaviour for any entity which can take damage.
/// </summary>
public interface IDamageable
{
	void TakeHit(float damage, RaycastHit hit);

	void TakeDamage(float damage);
}
