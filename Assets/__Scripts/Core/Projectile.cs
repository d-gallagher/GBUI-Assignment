using UnityEngine;

/// <summary>
/// Projectile behaviour script.
/// <para>Script to be used on weapon projectiles or similar</para>
/// </summary>
public class Projectile : MonoBehaviour
{
    #region Public Variables
    public float speed = 10;
    #endregion

    #region Unity Methods
    private void Update() => transform.Translate(Vector3.forward * Time.deltaTime * speed);
    #endregion

    public void SetSpeed(float newSpeed) => speed = newSpeed;
}