using UnityEngine;

public class Gun : MonoBehaviour
{
	/// <summary>
	/// Postion of the end of the gun barrel.
	/// </summary>
	public Transform gunBarrelPosition;
	public Projectile projectile;

	/// <summary>
	/// Time in milliseconds between each gunshot.
	/// </summary>
	public float timeBetweenShots = 100;
	/// <summary>
	/// Initial speed of projectile fired from gun.
	/// </summary>
	public float shotVelocity = 35;

	private float _nextShotTime;

	public void Shoot()
	{

		if (Time.time > _nextShotTime)
		{
			_nextShotTime = Time.time + timeBetweenShots / 1000;
			Projectile newProjectile = Instantiate(projectile, gunBarrelPosition.position, gunBarrelPosition.rotation) as Projectile;
			newProjectile.SetSpeed(shotVelocity);
		}
	}
}
