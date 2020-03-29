using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    #region Public Variables
    public AudioClip mainTheme;
    public AudioClip menuTheme;
    public float fadeDuration = 2.0f;
    #endregion

    #region Public Variables
    private string sceneName;
    #endregion

    #region Unity Methods
    void Start() => OnLevelLoaded(0);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) AudioManager.instance.PlayMusic(mainTheme, 3);
    }
    #endregion

    #region Private Methods
    private void OnLevelLoaded(int sceneIndex)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", 0.2f);
        }
    }

    private void PlayMusic()
    {
        AudioClip musicClip = null;
        if (sceneName == "Menu")
        {
            musicClip = menuTheme;
        }
        else if (sceneName == "Game")
        {
            musicClip = mainTheme;
        }

        if (musicClip != null)
        {
            AudioManager.instance.PlayMusic(musicClip, fadeDuration);
            Invoke("PlayMusic", musicClip.length);
        }
    }
    #endregion
}
