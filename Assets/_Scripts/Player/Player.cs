using UnityEngine;

/// <summary>
/// Player Script
/// <para>Expects PlayerController and GunController scripts to be present on GameObject</para>
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
[RequireComponent(typeof(RadialBeltController))]
public class Player : BaseLivingEntity, IMyoGesturable
{
    #region Public Variables
    /// <summary>
    /// Player Movement Speed
    /// </summary>
    public float moveSpeed = 5;
    public Crosshairs crosshairs;
    public float minCrosshairDistance = 1.0f;
    public float myoCrosshairDistance = 5.0f;
    public bool isInvincible;

    [Header("Use Myo")]
    public bool isUsingMyo;
    #endregion

    #region Private Variables
    private Thalmic.Myo.Pose _lastPose;
    float hapticOnDamegeReceived;
    #endregion

    #region References
    private Camera _cam;
    private PlayerController _playerController;
    private GunController _gunController;
    private RadialBeltScript _radialBeltScript;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _gunController = GetComponent<GunController>();
        //_beltController = GetComponent<RadialBeltController>();
        _feedbackController = FindObjectOfType<FeedbackController>();

        _radialBeltScript = GetComponentInChildren<RadialBeltScript>();

        _cam = Camera.main;
        FindObjectOfType<Spawner>().OnNewWave += OnNewWave;

        // Subcsribe to OnNewPose
        MyoGestureController.OnNewPose += OnNewPose;
        MyoGestureController.OnHoldPose += OnHoldPose;

    }

    protected override void Start()
    {
        //=> 
        base.Start();
        hapticOnDamegeReceived = health;
    }

    private void Update()
    {
        // Move Input
        UnityEngine.Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        UnityEngine.Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        _playerController.Move(moveVelocity);


        if (!isUsingMyo)
        {
            // Mouse Control
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(UnityEngine.Vector3.up, UnityEngine.Vector3.up * _gunController.GunHeight);

            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                UnityEngine.Vector3 point = ray.GetPoint(rayDistance);
                Debug.DrawLine(ray.origin, point, Color.red);
                //Debug.DrawRay(ray.origin,ray.direction * 100,Color.red);
                _playerController.LookAt(point);
                // Set crosshairs on mouse point
                crosshairs.transform.position = point;
                crosshairs.DetectTargets(ray);

                // Fix aiming - do not allow crosshair to pass through player, causing odd gun rotation.
                if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > minCrosshairDistance)
                {
                    _gunController.Aim(point);
                }
            }
        }
        else
        {
            // Myo Control
            Vector3 aimPoint = transform.position + (transform.forward * myoCrosshairDistance * _gunController.GunHeight);
            crosshairs.transform.position = aimPoint;
            _gunController.Aim(aimPoint);
        }

        // Primary Weapon Input
        if (Input.GetMouseButton(0)) _gunController.OnTriggerHold();
        if (Input.GetMouseButtonUp(0)) _gunController.OnTriggerRelease();
        if (Input.GetKeyDown(KeyCode.R)) _gunController.Reload();

        // Special Weapon Input
        if (Input.GetMouseButton(1)) _radialBeltScript.OnTriggerHold();
        if (Input.GetMouseButton(1)) _radialBeltScript.OnTriggerRelease();

        // Weapon Switch Input
        if (Input.GetKeyDown(KeyCode.X)) _gunController.OnSwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Z)) _gunController.OnSwitchWeapon(-1);

        // Enable/Disable Myo
        if (Input.GetKeyDown(KeyCode.P)) isUsingMyo = !isUsingMyo;

        // Haptic if the player takes damage
        if (health < hapticOnDamegeReceived)
        {
            //HapticFeedback("Short");
            hapticOnDamegeReceived = health;
        }

        // Kill the Player if it falls off the map.
        if (transform.position.y < -10)
        {
            //HapticFeedback("Long");
            TakeDamage(health);
        }


        // Debugging - Feedback
        if (Input.GetKeyDown(KeyCode.F10)) _feedbackController.ShakeAndVibrate(Enums.FeedbackType.Short);
        if (Input.GetKeyDown(KeyCode.F11)) _feedbackController.ShakeAndVibrate(Enums.FeedbackType.Medium);
        if (Input.GetKeyDown(KeyCode.F12)) _feedbackController.ShakeAndVibrate(Enums.FeedbackType.Long);


    }
    #endregion

    #region Implementation of IMyoGesturable
    public void OnNewPose(Thalmic.Myo.Pose newPose)
    {
        bool isPrimaryFiring = newPose == Thalmic.Myo.Pose.Fist;
        bool isSecondaryFiring = newPose == Thalmic.Myo.Pose.FingersSpread;

        // Fist
        if (newPose == Thalmic.Myo.Pose.Fist) _gunController.OnTriggerHold();
        if (_lastPose == Thalmic.Myo.Pose.Fist && !isPrimaryFiring) _gunController.OnTriggerRelease();

        // Finger Spread
        if (newPose == Thalmic.Myo.Pose.FingersSpread) _radialBeltScript.OnTriggerHold();
        if (_lastPose == Thalmic.Myo.Pose.FingersSpread && !isSecondaryFiring) _radialBeltScript.OnTriggerRelease();

        // Wave In/Out
        if (newPose == Thalmic.Myo.Pose.WaveIn) _gunController.OnSwitchWeapon(1);
        if (newPose == Thalmic.Myo.Pose.WaveOut) _gunController.OnSwitchWeapon(-1);

        _lastPose = newPose;
    }

    public void OnHoldPose()
    {
        // Not needed yet...
        if (_lastPose == Thalmic.Myo.Pose.Fist) _gunController.OnTriggerHold();
    }
    #endregion

    #region BaseLivingEntity Overrides
    protected override void Die()
    {
        AudioManager.instance.PlaySound("Player Death", Vector3.zero);
        base.Die();
        _feedbackController.ShakeAndVibrate(Enums.FeedbackType.Long);
    }

    public override void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            base.TakeDamage(damage);
        }
        _feedbackController.ShakeAndVibrate(Enums.FeedbackType.Medium);
    }
    #endregion

    private void OnNewWave(int waveNumber)
    {
        health = startingHealth;
        _gunController.EquipGun(waveNumber - 1);
    }
}