using System.Collections;
using UnityEngine;

/// <summary>
/// Script for spawning Enemny Waves.
/// </summary>
public class Spawner : MonoBehaviour
{
    #region Public Variables
    public EnemyWave[] waves;
    public Enemy enemy;
    public float timeBetweenCampCheck = 2;
    public float campThresholdDistance = 1.5f;
    #endregion

    #region Private/Serialized Variables
    private EnemyWave _currentWave;
    private int _currentWaveNumber;

    private int _enemiesRemainingToSpawn;
    private int _enemiesRemainingAlive;
    private float _nextSpawnTime;
    private MapGenerator _map;

    private BaseLivingEntity _playerEntity;
    private Transform _playerTransform;
    private float _nextCampCheckTime;
    private Vector3 _campPositionOld;
    private bool _isCamping;

    private bool _isDisabled;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _playerEntity = FindObjectOfType<Player>();
        _playerTransform = _playerEntity.transform;

        _nextCampCheckTime = timeBetweenCampCheck + Time.deltaTime;
        _campPositionOld = _playerTransform.position;

        _playerEntity.OnDeath += OnPlayerDeath;

        _map = FindObjectOfType<MapGenerator>();
        SpawnNewWave();
    }

    private void Update()
    {
        if (!_isDisabled)
        {
            if (Time.time > _nextCampCheckTime)
            {
                _nextCampCheckTime = Time.time + timeBetweenCampCheck;
                _isCamping = (Vector3.Distance(_playerTransform.position, _campPositionOld) < campThresholdDistance);
                _campPositionOld = _playerTransform.position;
            }

            if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
            {
                _enemiesRemainingToSpawn--;
                _nextSpawnTime = Time.time + _currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }
    }
    #endregion

    private IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;

        Transform spawnTile = _map.GetRandomOpenTile();

        if (_isCamping) spawnTile = _map.GetTileFromPosition(_playerTransform.position);

        // TODO: FIX THIS!!!
        //Material tileMat = spawnTile.GetComponent<Renderer>().material;
        //Color originalColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0;

        while (spawnTimer < spawnDelay)
        {
            //tileMat.color = Color.Lerp(originalColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
        // Add the OnEnemyDeath method to the OnDeath action of the enemy...
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    private void OnPlayerDeath() => _isDisabled = true;

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