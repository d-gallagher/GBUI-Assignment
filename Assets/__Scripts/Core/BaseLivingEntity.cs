﻿using System;
using UnityEngine;

/// <summary>
/// Base class for any 'Living' game entities.
/// <para>Implementation of IDamageable.</para>
/// </summary>
public abstract class BaseLivingEntity : MonoBehaviour, IDamageable
{
    #region Public Variables
    public float startingHealth;
    #endregion

    #region Protected Variables
    protected float health;
    protected bool isDead;
    #endregion

    #region Events
    public event Action OnDeath;
    #endregion

    #region Unity Methods
    protected virtual void Start() => health = startingHealth;
    #endregion

    public void TakeHit(float damage, RaycastHit hit)
    {
        health -= damage;

        if (health <= 0 && !isDead) Die();
    }

    protected void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
