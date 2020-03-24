using UnityEngine;

/// <summary>
/// Player Script
/// <para>Expects PlayerController and GunController scripts to be present on GameObject</para>
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : BaseLivingEntity
{
    #region Public Variables
    /// <summary>
    /// Player Movement Speed
    /// </summary>
    public float moveSpeed = 5;
    public Crosshairs crosshairs;
    #endregion

    #region References
    private Camera _cam;
    private PlayerController _playerController;
    private GunController _gunController;
    #endregion

    #region Unity Methods
    protected override void Start()
    {
        base.Start();
        _playerController = GetComponent<PlayerController>();
        _gunController = GetComponent<GunController>();
        _cam = Camera.main;
    }

    private void Update()
    {
        // Move Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        _playerController.Move(moveVelocity);

        // Look Input
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * _gunController.GunHeight);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            //Debug.DrawRay(ray.origin,ray.direction * 100,Color.red);
            _playerController.LookAt(point);
            // Set crosshairs on mouse point
            crosshairs.transform.position = point;
            crosshairs.DetectTargets(ray);
        }

        // Weapon Input
        if (Input.GetMouseButton(0)) _gunController.OnTriggerHold();
        if (Input.GetMouseButtonUp(0)) _gunController.OnTriggerrelease();
    }
    #endregion
}