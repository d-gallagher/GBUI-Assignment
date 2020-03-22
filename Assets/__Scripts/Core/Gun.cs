using UnityEngine;

/// <summary>
/// Gun Script
/// <para>Set gunBarrelPosition to transform where projectile will spawn.</para>
/// <para>Set Should projectile to the object to spawn wwhen gun is fired.</para>
/// </summary>
public class Gun : MonoBehaviour
{
    #region Public Variables
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
    #endregion

    #region Private Variables
    // Keep tranck of when next projectile can be fired.
    private float _nextShotTime;
    #endregion

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
