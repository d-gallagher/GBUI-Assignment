using System;
using Thalmic.Myo;
using UnityEngine;

/// <summary>
/// Base class for any 'Living' game entities.
/// <para>Implementation of IDamageable.</para>
/// </summary>
public abstract class BaseLivingEntity : MonoBehaviour, IDamageable, IVibrateable
{
    #region Public Variables
    public float startingHealth;
    #endregion

    #region Protected Variables
    public float health { get; protected set; }
    protected bool isDead;
    #endregion

    private Shake shake;
    private ThalmicMyo _thalmicMyo;

    #region Events
    public event Action OnDeath;
    #endregion

    #region Unity Methods
    protected virtual void Start() {
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<Shake>();
        _thalmicMyo = FindObjectOfType<ThalmicMyo>();
        health = startingHealth;
    }
    #endregion

    public virtual void TakeHit(float damage, UnityEngine.Vector3 hitPoint, UnityEngine.Vector3 hitDirection) => TakeDamage(damage);

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        HapticFeedback(0);

        if (health <= 0 && !isDead)
        {
            shake.CamShake();
            HapticFeedback(2);
            Die();
        }
    }
    public virtual void HapticFeedback(int vibrationType)
    {
        switch (vibrationType)
        {
            case 0:
                _thalmicMyo.Vibrate(VibrationType.Short);
                break;
            case 1:
                _thalmicMyo.Vibrate(VibrationType.Medium);
                break;
            case 2:
                _thalmicMyo.Vibrate(VibrationType.Long);
                break;
            default:
                break;
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
