using System.Collections;
using UnityEngine;
using static Enums;

/// <summary>
/// Gun Script
/// <para>Set gunBarrelPosition to transform where projectile will spawn.</para>
/// <para>Set Should projectile to the object to spawn wwhen gun is fired.</para>
/// </summary>
public class Gun : MonoBehaviour, IFireable
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

    [Header("Fire Mode / Rounds")]
    public FireMode fireMode;
    public int burstCount;
    public int roundsPerMag;

    [Header("Reload")]
    public float reloadTime = 1.0f;
    public float maxReloadAngle = 30.0f;

    [Header("Recoil")]
    public Vector2 minMaxRecoilAmount = new Vector2(3, 5);
    public Vector2 minMaxRecoilAngle = new Vector2(0.05f, 0.2f);
    public float recoilMoveSettleTime = 0.1f;
    public float recoilRotationSettleTime = 0.1f;

    [Header("Shell")]
    // Shell
    public Transform shell;
    public Transform shellEjectionPoint;

    [Header("Audio")]
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    #endregion

    #region Private Variables
    // Gun
    private float _nextShotTime;
    private MuzzleFlash _muzzleFlash;

    // Fire Mode
    private bool _triggerReleasedSinceLastShot;
    private int _shotRemainingInBurst;
    private int _roundsRemainingInMag;

    // Recoil
    private Vector3 _recoilSmoothDampVelocity;
    private float _recoilRotationSmoothDampVelocity;
    private float _recoilAngle;

    // Reload
    private bool _isReloading;


    #endregion

    #region Unity Methods
    private void Start()
    {
        _muzzleFlash = GetComponent<MuzzleFlash>();
        _shotRemainingInBurst = burstCount;
        _roundsRemainingInMag = roundsPerMag;
    }

    private void LateUpdate()
    {
        // Animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref _recoilSmoothDampVelocity, recoilMoveSettleTime);
        _recoilAngle = Mathf.SmoothDamp(_recoilAngle, 0, ref _recoilRotationSmoothDampVelocity, recoilRotationSettleTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * _recoilAngle;

        // Reload if not already reloading and mag is empty.
        if (!_isReloading && _roundsRemainingInMag == 0) Reload();
    }
    #endregion

    #region Implementation of IFireable
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
    #endregion

    #region Public Methods
    public void Aim(Vector3 aimPoint)
    {
        // Only aim if not reloading
        if (!_isReloading) transform.LookAt(aimPoint);
    }

    public void Reload()
    {
        // Only reload if not already and mag is not full
        if (!_isReloading && _roundsRemainingInMag != roundsPerMag)
        {
            StartCoroutine(AnimateReload());
            AudioManager.instance.PlaySound(reloadAudio, transform.position);
        }
    }
    #endregion

    #region Private Methods
    private void Shoot()
    {
        if (!_isReloading && Time.time > _nextShotTime && _roundsRemainingInMag > 0)
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
                // Break from loop if mag is empty
                if (_roundsRemainingInMag == 0) break;

                _roundsRemainingInMag -= 1;
                _nextShotTime = Time.time + timeBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, t.position, t.rotation) as Projectile;
                newProjectile.SetSpeed(shotVelocity);
            }

            // Only muzzle flash and eject shell once
            Instantiate(shell, shellEjectionPoint.position, shellEjectionPoint.rotation);
            _muzzleFlash.Activate();
            transform.localPosition = Vector3.forward * Random.Range(minMaxRecoilAmount.x, minMaxRecoilAmount.y);
            _recoilAngle += Random.Range(minMaxRecoilAngle.x, minMaxRecoilAngle.y);
            _recoilAngle = Mathf.Clamp(_recoilAngle, 0, 30.0f);

            AudioManager.instance.PlaySound(shootAudio, transform.position);
        }
    }

    private IEnumerator AnimateReload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRotation = transform.eulerAngles;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRotation + Vector3.left * reloadAngle;


            yield return null;
        }

        _isReloading = false;
        _roundsRemainingInMag = roundsPerMag;
    }
    #endregion
}
