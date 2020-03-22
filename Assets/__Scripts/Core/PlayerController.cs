using UnityEngine;

/// <summary>
/// PlayerController - Controls player movement.
/// <para>Expects Rigidbody component on GameObject</para>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Vector3 _velocity;
    private Rigidbody _rb;

    private void Start() => _rb = GetComponent<Rigidbody>();

    /// <summary>
    /// Move the GameObject at a certain velocity.
    /// </summary>
    /// <param name="_velocity"></param>
    public void Move(Vector3 _velocity) => this._velocity = _velocity;

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    /// <summary>
    /// Physics update.
    /// </summary>
    public void FixedUpdate() => _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
}
