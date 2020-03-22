using UnityEngine;
using System.Collections;
using UnityEngine.AI;

/// <summary>
/// Enemy Script
/// <para>Basic Enemy AI script using NavMeshAgent</para>
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
	private NavMeshAgent _pathfinder;
	private Transform _target;

	void Start()
	{
		_pathfinder = GetComponent<NavMeshAgent>();
		_target = GameObject.FindGameObjectWithTag("Player").transform;

		StartCoroutine(UpdatePath());
	}

	IEnumerator UpdatePath()
	{
		float refreshRate = .25f;

		while (_target != null)
		{
			Vector3 targetPosition = new Vector3(_target.position.x, 0, _target.position.z);
			_pathfinder.SetDestination(targetPosition);
			yield return new WaitForSeconds(refreshRate);
		}
	}
}