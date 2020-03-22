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
    #endregion

    #region Unity Methods
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
}