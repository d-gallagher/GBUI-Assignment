using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    #region Public Variables
    public static int Score { get; private set; }
    public int streakCount;
    #endregion

    #region Private Variables
    private float _lastEnemyKillTime;
    private float _streakExpiryTime = 1;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Enemy.OnDeathStatic += OnEnemyKilled;
        FindObjectOfType<Player>().OnDeath += OnPlayerDeath;
    }
    #endregion

    private void OnEnemyKilled()
    {
        if (Time.time < _lastEnemyKillTime + _streakExpiryTime) streakCount++;
        else streakCount = 0;

        _lastEnemyKillTime = Time.time;
        Score += 5 + (int)Mathf.Pow(2, streakCount);
    }

    private void OnPlayerDeath() => Enemy.OnDeathStatic -= OnEnemyKilled;
}
