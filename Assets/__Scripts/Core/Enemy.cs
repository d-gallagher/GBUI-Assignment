using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using static Enums;
using System;

/// <summary>
/// Enemy Script - Basic Enemy AI script.
/// <para>Expects NavMeshAgent component on GameObject</para>
/// <para>Expects a GameObject with tag "Player" to be present in scene.</para>
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : BaseLivingEntity
{

    public ParticleSystem deathEffect;

    #region Private/Serialized Variables
    [SerializeField]
    private EnemyState _currentState;
    [SerializeField]
    float attackDistanceThreshold = .5f;
    [SerializeField]
    float timeBetweenAttacks = 1;
    [SerializeField]
    float _damage = 1;

    private NavMeshAgent _pathfinder;
    private Transform _target;
    private BaseLivingEntity _targetEntity;
    private Material _skinMaterial;
    private Color _originalColour;

    private float _nextAttackTime;
    private float _myCollisionRadius;
    private float _targetCollisionRadius;

    private bool _hasTarget;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Pathfinding
        _pathfinder = GetComponent<NavMeshAgent>();

        // Check that player exists and is still alive...
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            _hasTarget = true;
            // Set up target variables.
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            _targetEntity = _target.GetComponent<BaseLivingEntity>();

            // Set up collision variables.
            _myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            _targetCollisionRadius = _target.GetComponent<CapsuleCollider>().radius;
        }
    }

    protected override void Start()
    {
        base.Start();

        // Check that player exists and is still alive...
        if (_hasTarget)
        {
            _hasTarget = true;
            _currentState = EnemyState.Chasing;
            _targetEntity.OnDeath += OnTargetDeath;

            // Begin the coroutine to follow the Player.
            StartCoroutine(UpdatePath());
        }

    }

    private void Update()
    {
        if (_hasTarget)
        {
            if (Time.time > _nextAttackTime)
            {
                float sqrDstToTarget = (_target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + _myCollisionRadius + _targetCollisionRadius, 2))
                {
                    _nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }
    #endregion

    #region Overrides
    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (_damage >= health)
        {
            // TODO: remove obsolete code
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
        }
        base.TakeHit(damage, hitPoint, hitDirection);
    }

    #endregion

    public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColor)
    {
        // Speed
        _pathfinder.speed = moveSpeed;
        // Damage and Health
        if (_hasTarget) _damage = Mathf.Ceil(_targetEntity.startingHealth / hitsToKillPlayer);
        startingHealth = enemyHealth;
        // Appearance
        _skinMaterial = GetComponent<Renderer>().material;
        _skinMaterial.color = skinColor;
        _originalColour = _skinMaterial.color;
    }

    void OnTargetDeath()
    {
        _hasTarget = false;
        _currentState = EnemyState.Idle;
    }

    IEnumerator Attack()
    {
        _currentState = EnemyState.Attacking;
        _pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (_target.position - transform.position).normalized;
        Vector3 attackPosition = _target.position - dirToTarget * (_myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        _skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                _targetEntity.TakeDamage(_damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        _skinMaterial.color = _originalColour;
        _currentState = EnemyState.Chasing;
        _pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        // Limit the frequency of updates to ease CPU cycles.
        float refreshRate = .25f;

        while (_hasTarget)
        {
            if (_currentState == EnemyState.Chasing)
            {
                // Get Player position and move towards.
                Vector3 dirToTarget = (_target.position - transform.position).normalized;
                Vector3 targetPosition = _target.position - dirToTarget * (_myCollisionRadius + _targetCollisionRadius + attackDistanceThreshold / 2);
                if (!isDead)
                {
                    // Only update path if enemy is still alive...
                    _pathfinder.SetDestination(targetPosition);
                }
            }
            // Wait until next path update.
            yield return new WaitForSeconds(refreshRate);
        }
    }
}