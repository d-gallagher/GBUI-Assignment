using UnityEngine;

/// <summary>
/// PlayerController - Controls player movement.
/// <para>Expects Rigidbody component on GameObject</para>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Private Variables
    private Vector3 _velocity;
    #endregion

    #region References
    private Rigidbody _rb;
    #endregion

    #region Unity Methods
    private void Start() => _rb = GetComponent<Rigidbody>();
    
    /// <summary>
    /// Physics update.
    /// </summary>
    public void FixedUpdate() => _rb.MovePosition(_rb.position + _velocity * Time.fixedDeltaTime);
    #endregion

    /// <summary>
    /// Move the GameObject at a certain velocity.
    /// </summary>
    /// <param name="_velocity"></param>
    public void Move(Vector3 _velocity) => this._velocity = _velocity;

    public void LookAt(Vector3 lookPoint)
    {
        // Always looking at same height on Y axis, so account for this.
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

}
