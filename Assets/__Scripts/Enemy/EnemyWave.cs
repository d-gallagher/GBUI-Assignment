using System;
using UnityEngine;

/// <summary>
/// POCO representing a Wave of Enemies.
/// </summary>
[Serializable]
public class EnemyWave
{
	public int enemyCount;
	public bool isInfiniteWave;
	public float timeBetweenSpawns;

	public float moveSpeed;
	public int hitsToKillPlayer;

	public float enemyHealth;
	public Color skinColor;
}
