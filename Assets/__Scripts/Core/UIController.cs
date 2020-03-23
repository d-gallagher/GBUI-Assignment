using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image fadePlane;
    public GameObject gameOverUI;

    private void Start()
    {
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    private void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
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
}
