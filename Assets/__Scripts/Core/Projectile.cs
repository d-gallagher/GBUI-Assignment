using UnityEngine;

/// <summary>
/// Projectile behaviour script.
/// <para>Script to be used on weapon projectiles or similar</para>
/// </summary>
public class Projectile : MonoBehaviour
{
    #region Public Variables
    public LayerMask collisionMask;
    #endregion

    #region Private Serialized Variables
    [SerializeField]
    private float _speed = 10;
    [SerializeField]
    private float _damage = 1;
    [SerializeField]
    float _lifetime = 3;
    [SerializeField]
    float _skinWidth = .1f;

    #endregion

    #region Unity Methods
    private void Start()
    {
        Destroy(gameObject, _lifetime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0) OnHitObject(initialCollisions[0]);
    }

    private void Update()
    {
        float moveDistance = _speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }
    #endregion

    public void SetSpeed(float newSpeed) => _speed = newSpeed;

    private void CheckCollisions(float moveDistance)
    {
        // Cast a ray forward from the projectile.
        Ray ray = new Ray(transform.position, transform.forward);
        // Check if the ray hit anything on the layer...
        if (Physics.Raycast(ray, out RaycastHit hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            // Raycast hit, handle hitting the GameObject
            OnHitObject(hit);
        }
    }

    private void OnHitObject(RaycastHit hit)
    {
        // Check to see if the object which was hit is damageable and deal a hit if it is.
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null) damageableObject.TakeHit(_damage, hit);
        // Destroy this projectile.
        Destroy(gameObject);
    }

    private void OnHitObject(Collider c)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null) damageableObject.TakeDamage(_damage);
        Destroy(gameObject);
    }
}