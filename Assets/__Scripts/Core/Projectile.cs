using UnityEngine;

/// <summary>
/// Projectile behaviour script.
/// <para>Script to be used on weapon projectiles or similar</para>
/// </summary>
public class Projectile : MonoBehaviour
{
    public float speed = 10;

    private void Update() => transform.Translate(Vector3.forward * Time.deltaTime * speed);

    public void SetSpeed(float newSpeed) => speed = newSpeed;
}