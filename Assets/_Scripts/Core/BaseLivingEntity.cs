using System;
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
    public float health { get; protected set; }
    protected bool isDead;
    #endregion

    private Shake shake;

    #region Events
    public event Action OnDeath;
    #endregion

    #region Unity Methods
    protected virtual void Start() {
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        health = startingHealth;
    }
    #endregion

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection) => TakeDamage(damage);

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !isDead)
        {
            shake.CamShake();
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    protected virtual void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
        Destroy(gameObject);
    }

}
