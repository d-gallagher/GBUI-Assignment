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
    protected bool dead;
    #endregion

    #region Unity Methods
    protected virtual void Start() => health = startingHealth;
    #endregion

    public void TakeHit(float damage, RaycastHit hit)
    {
        health -= damage;

        if (health <= 0 && !dead) Die();
    }

    protected void Die()
    {
        dead = true;
        GameObject.Destroy(gameObject);
    }
}
