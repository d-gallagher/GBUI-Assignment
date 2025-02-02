﻿using System.Collections.Generic;
using UnityEngine;

public class RadialBeltScript : MonoBehaviour, IFireable
{
    [Header("Belt Setup")]
    public Transform[] notches;
    public GameObject beltNotchPrefab;
    public Projectile projectile;
    public float shotVelocity = 35;
    public float cooldownTime = 3.0f;

    public bool IsCoolingDown => _timeUntilNextShot > 0f;

    [Header("Audio")]
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    private float _timeUntilNextShot;
    private Dictionary<Transform, MuzzleFlash> _beltNotchDict;
    private bool _triggerReleasedSinceLastShot;
    private bool _isReloading;
    private Transform _bulletParent;
    private IFeedbackController _feedbackController;

    #region Unity Methods
    private void Start()
    {
        projectile.SetDamage(10);
        projectile.SetPenetrating(true);

        _beltNotchDict = new Dictionary<Transform, MuzzleFlash>();

        _bulletParent = GameObject.Find("BulletParent").transform;
        _feedbackController = FindObjectOfType<FeedbackController>();

        GameObject prefab;
        foreach (Transform child in transform)
        {
            // Instantiate a prefab
            prefab = Instantiate(beltNotchPrefab, child, false);
            _beltNotchDict.Add(prefab.transform, prefab.GetComponent<MuzzleFlash>());
        }
    }

    private void Update()
    {
        if (_timeUntilNextShot > 0) _timeUntilNextShot -= Time.deltaTime;
       
    }

    private void FixedUpdate()
    {
        Reload();
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
    }
    #endregion

    #region Private Methods
    private void Shoot()
    {
        if (_timeUntilNextShot <= 0f)
        {
            if (!_triggerReleasedSinceLastShot) return; //

            _timeUntilNextShot = cooldownTime;

            Transform t;
            MuzzleFlash m;

            foreach (var d in _beltNotchDict)
            {
                t = d.Key;
                m = d.Value;

                Projectile newProjectile = Instantiate(projectile, t.position, t.rotation, _bulletParent) as Projectile;
                newProjectile.SetSpeed(shotVelocity);
                m.Activate();
            }
            AudioManager.instance.PlaySound(shootAudio, transform.position);
            _feedbackController.Shake(Enums.FeedbackType.Short);
        _isReloading = true;
        }
    }

    private void Reload()
    {
        if (_isReloading)
        {
            _isReloading = false;
            AudioManager.instance.PlaySound(reloadAudio, transform.position);
        }        
    }
    #endregion
}
