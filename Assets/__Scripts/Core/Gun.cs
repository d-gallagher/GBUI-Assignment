using UnityEngine;
using static Enums;

/// <summary>
/// Gun Script
/// <para>Set gunBarrelPosition to transform where projectile will spawn.</para>
/// <para>Set Should projectile to the object to spawn wwhen gun is fired.</para>
/// </summary>
public class Gun : MonoBehaviour
{
    #region Public Variables
    [Header("Gun Settings")]
    /// <summary>
    /// Postion of the end of the gun barrel.
    /// </summary>
    public Transform[] projectileSpawnPoints;
    public Projectile projectile;

    /// <summary>
    /// Time in milliseconds between each gunshot.
    /// </summary>
    public float timeBetweenShots = 100;
    /// <summary>
    /// Initial speed of projectile fired from gun.
    /// </summary>
    public float shotVelocity = 35;

    [Header("Fire Mode Settings")]
    public FireMode fireMode;
    public int burstCount;


    [Header("Shell Settings")]
    // Shell
    public Transform shell;
    public Transform shellEjectionPoint;
    #endregion

    #region Private Variables
    // Keep tranck of when next projectile can be fired.
    private float _nextShotTime;

    private MuzzleFlash _muzzleFlash;

    private bool _triggerReleasedSinceLastShot;
    private int _shotRemainingInBurst;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _muzzleFlash = GetComponent<MuzzleFlash>();
        _shotRemainingInBurst = burstCount;
    }
    #endregion

    public void OnTriggerHold()
    {
        Shoot();
        _triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        _triggerReleasedSinceLastShot = true;
        _shotRemainingInBurst = burstCount;
    }

    private void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            switch (fireMode)
            {
                case FireMode.Single:
                    if (!_triggerReleasedSinceLastShot) return;
                    break;

                case FireMode.Burst:
                    if (_shotRemainingInBurst == 0) return;
                    else _shotRemainingInBurst--;
                    break;

                case FireMode.Auto:
                    break;

                default:
                    break;
            }

            // Loop through each projectile spawn point
            foreach (var t in projectileSpawnPoints)
            {
                _nextShotTime = Time.time + timeBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, t.position, t.rotation) as Projectile;
                newProjectile.SetSpeed(shotVelocity);
            }

            // Only muzzle flash and eject shell once
            Instantiate(shell, shellEjectionPoint.position, shellEjectionPoint.rotation);
            _muzzleFlash.Activate();
        }
    }

}
