using UnityEngine;

/// <summary>
/// Script for spawning Enemny Waves.
/// </summary>
public class Spawner : MonoBehaviour
{
    #region Public Variables
    public EnemyWave[] waves;
    public Enemy enemy;
    #endregion

    #region Private/Serialized Variables
    private EnemyWave _currentWave;
    private int _currentWaveNumber;

    private int _enemiesRemainingToSpawn;
    private int _enemiesRemainingAlive;
    private float _nextSpawnTime;
    #endregion

    #region Unity Methods
    private void Start() => SpawnNewWave();

    private void Update()
    {
        if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
        {
            _enemiesRemainingToSpawn--;
            _nextSpawnTime = Time.time + _currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            // Add the OnEnemyDeath method to the OnDeath action of the enemy...
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }
    #endregion

    /// <summary>
    /// Check to see if any enemies remain in this wave and spawn a new wave if needed.
    /// </summary>
    private void OnEnemyDeath()
    {
        _enemiesRemainingAlive--;

        if (_enemiesRemainingAlive == 0) SpawnNewWave();
    }

    /// <summary>
    /// Spawn a new wave of enemies if there are waves remaining.
    /// </summary>
    private void SpawnNewWave()
    {
        _currentWaveNumber++;
        print("Wave: " + _currentWaveNumber);
        if (_currentWaveNumber - 1 < waves.Length)
        {
            _currentWave = waves[_currentWaveNumber - 1];

            _enemiesRemainingToSpawn = _currentWave.enemyCount;
            _enemiesRemainingAlive = _enemiesRemainingToSpawn;
        }
    }
}