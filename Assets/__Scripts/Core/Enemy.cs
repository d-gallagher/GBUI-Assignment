using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/// <summary>
/// Enemy Script - Basic Enemy AI script.
/// <para>Expects NavMeshAgent component on GameObject</para>
/// <para>Expects a GameObject with tag "Player" to be present in scene.</para>
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    #region Private Variables
    private NavMeshAgent _pathfinder;
	private Transform _target;
    #endregion

    #region Unity Methods
    void Start()
	{
		_pathfinder = GetComponent<NavMeshAgent>();
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		// Begin the coroutine to follow the Player.
		StartCoroutine(UpdatePath());
	}
    #endregion

    IEnumerator UpdatePath()
	{
		// Limit the frequency of updates to ease CPU cycles.
		float refreshRate = .25f;

		while (_target != null)
		{
			// Get Player position and move towards.
			Vector3 targetPosition = new Vector3(_target.position.x, 0, _target.position.z);
			_pathfinder.SetDestination(targetPosition);
			// Wait until next path update.
			yield return new WaitForSeconds(refreshRate);
		}
	}
}