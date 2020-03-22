using System;

/// <summary>
/// POCO representing a Wave of Enemies.
/// </summary>
[Serializable]
public class EnemyWave
{
	public int enemyCount;
	public float timeBetweenSpawns;
}
