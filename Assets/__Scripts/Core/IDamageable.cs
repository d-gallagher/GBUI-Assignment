using UnityEngine;

/// <summary>
/// Interface defining behaviour for any entity which can take damage.
/// </summary>
public interface IDamageable
{
	void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection);

	void TakeDamage(float damage);
}
