using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Public Variables
    [Header("General")]
    public Image fadePlane;

    [Header("Game Over")]
    public GameObject gameOverUI;
    public Text gameOverScoreUI;
    public Color gameOverFadeToColor = new Color(0, 0, 0, 0.95f);

    [Header("New Wave")]
    public RectTransform newWaveBanner;
    public Text newWaveTitle;
    public Text newWaveEnemyCount;
    public float newWaveBannerSpeed = 2.5f;
    public float newWaveBannerDelayTime = 1;
    public Vector2 anchorPosition = new Vector2(970, 825);

    [Header("Score")]
    public Text scoreUI;

    [Header("Health")]
    public RectTransform healthBar;
    public Text healthUI;

    [Header("Weapon")]
    public Text equippedWeapon;
    #endregion

    #region Private Variables
    private Player _player;
    private Spawner _spawner;
    #endregion

    #region Unity Methods
    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _player.OnDeath += OnGameOver;
    }

    private void Awake()
    {
        _spawner = FindObjectOfType<Spawner>();
        _spawner.OnNewWave += OnNewWave;
    }

    private void Update()
    {
        scoreUI.text = ScoreKeeper.Score.ToString("D6");

        float healthPercent = 0;
        if (_player != null) healthPercent = _player.health / _player.startingHealth;
        healthBar.localScale = new Vector3(healthPercent, 1, 1);

        if (_player != null)
        {
            equippedWeapon.text = _player.GetComponentInChildren<Gun>().name.Split('(')[0];
        }
    }
    #endregion

    #region Public Methods
    public void StartNewGame() => SceneManager.LoadScene("MainScene");

    public void ReturnToMainMenu() => SceneManager.LoadScene("Menu");
    #endregion

    #region Private Methods
    private void OnGameOver()
    {
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, gameOverFadeToColor, 1));
        gameOverScoreUI.text = scoreUI.text;
        scoreUI.gameObject.SetActive(false);
        healthBar.transform.parent.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    private void OnNewWave(int waveNumber)
    {
        int index = waveNumber - 1;

        string[] numbers = { "One", "Two", "Three", "Four", "Five" };
        newWaveTitle.text = $"- Wave {numbers[index]} -";

        string enemyCountString = _spawner.waves[index].isInfiniteWave ? "Infinite" : $"Enemies {_spawner.waves[index].enemyCount}";
        newWaveEnemyCount.text = enemyCountString;

        StopCoroutine("AnimateNewWaveBanner");
        StartCoroutine("AnimateNewWaveBanner");
    }

    private IEnumerator AnimateNewWaveBanner()
    {
        float animatePercent = 0;
        float endDelayTime = (Time.time + 1 / newWaveBannerSpeed) + newWaveBannerDelayTime;
        float direction = 1;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * newWaveBannerSpeed * direction;
            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    direction = -1;
                }
            }

            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(anchorPosition.x, anchorPosition.y, animatePercent);
            yield return null;
        }
    }

    private IEnumerator Fade(Color from, Color to, float duration)
    {
        float speed = 1 / duration;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }
    #endregion
}
