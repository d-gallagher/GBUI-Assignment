using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using static Enums;

/// <summary>
/// Enemy Script - Basic Enemy AI script.
/// <para>Expects NavMeshAgent component on GameObject</para>
/// <para>Expects a GameObject with tag "Player" to be present in scene.</para>
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : BaseLivingEntity
{

    #region Private/Serialized Variables
    [SerializeField]
    private EnemyState _currentState;
    [SerializeField]
    float attackDistanceThreshold = .5f;
    [SerializeField]
    float timeBetweenAttacks = 1;

    private NavMeshAgent _pathfinder;
    private Transform _target;
    private Material _skinMaterial;
    private Color _originalColour;

    private float _nextAttackTime;
    private float _myCollisionRadius;
    private float _targetCollisionRadius;
    #endregion

    #region Unity Methods
    protected override void Start()
    {
        base.Start();

        // Appearance
        _skinMaterial = GetComponent<Renderer>().material;
        _originalColour = _skinMaterial.color;

        // Pathfinding
        _pathfinder = GetComponent<NavMeshAgent>();
        _currentState = EnemyState.Chasing;
        _target = GameObject.FindGameObjectWithTag("Player").transform;

        // Set up other variables.
        _myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        _targetCollisionRadius = _target.GetComponent<CapsuleCollider>().radius;

        // Begin the coroutine to follow the Player.
        StartCoroutine(UpdatePath());
    }

    private void Update()
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
    #endregion

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

        while (percent <= 1)
        {
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

        while (_target != null)
        {
            // Get Player position and move towards.
            Vector3 targetPosition = new Vector3(_target.position.x, 0, _target.position.z);
            // Only update path if enemy is still alive...
            if (!isDead) _pathfinder.SetDestination(targetPosition);
            // Wait until next path update.
            yield return new WaitForSeconds(refreshRate);
        }
    }
}