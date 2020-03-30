using UnityEngine;
using static Enums;

public class RadialBelt : MonoBehaviour, IFireable
{
    public Transform[] projectileSpawnPoints;
    public Projectile projectile;
    public float shotVelocity = 35;

    [Header("Shell")]
    // Shell
    public Transform shell;
    public Transform shellEjectionPoint;

    [Header("Audio")]
    public AudioClip shootAudio;

    // Fire Mode
    public FireMode fireMode;
    private bool _triggerReleasedSinceLastShot;

    #region Implementation of IFireable
    public void OnTriggerHold()
    {
         Shoot();
        _triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        _triggerReleasedSinceLastShot = true;
    }
    #endregion

    #region Private Methods
    private void Shoot()
    {

        //switch (fireMode)
        //{
        //    case FireMode.Single:
        //        if (!_triggerReleasedSinceLastShot) return;
        //        break;

        //    //case FireMode.Burst:
        //    //    if (_shotRemainingInBurst == 0) return;
        //    //    else _shotRemainingInBurst--;
        //    //    break;

        //    case FireMode.Auto:
        //        break;

        //    default:
        //        break;
        //}
        // Loop through each projectile spawn point
        foreach (var t in projectileSpawnPoints)
        {
            Projectile newProjectile = Instantiate(projectile, t.position, t.rotation) as Projectile;
            newProjectile.SetSpeed(shotVelocity);
        }

        // Only muzzle flash and eject shell once
        Instantiate(shell, shellEjectionPoint.position, shellEjectionPoint.rotation);

        AudioManager.instance.PlaySound(shootAudio, transform.position);
    }
    #endregion
}
