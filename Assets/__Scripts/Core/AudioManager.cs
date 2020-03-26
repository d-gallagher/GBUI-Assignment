using System.Collections;
using UnityEngine;
using static Enums;

public class AudioManager : MonoBehaviour
{
    #region Public Variables
    public float masterVolumePercent = 1;
    public float sfxVolumePercent = 1;
    public float musicVolumePercent = 1f;
    #endregion

    #region Private Variables
    private AudioSource _soundEffect2DSource;
    private AudioSource[] _musicSources;
    private int _activeMusicSourceIndex;

    private Transform _audioListenerTransform;
    private Transform _playerTransform;
    private SoundLibrary _soundLibrary;
    #endregion

    // Singleton
    public static AudioManager instance;

    #region Unity Methods
    private void Awake()
    {
        // Assign singleton
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;

            _soundLibrary = GetComponent<SoundLibrary>();

            _musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source " + (i + 1));
                _musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            // Set up 2D SFX AudioSource
            GameObject newSoundEffect2DSource = new GameObject("2D sfx");
            _soundEffect2DSource = newSoundEffect2DSource.AddComponent<AudioSource>();
            _soundEffect2DSource.transform.parent = transform;

            _audioListenerTransform = FindObjectOfType<AudioListener>().transform;

            // Try to get the Player
            Player player = FindObjectOfType<Player>();
            if (player != null) _playerTransform = player.transform;

            TryGetVolumePrefs();
        }
    }

    private void Update()
    {
        if (_playerTransform != null) _audioListenerTransform.position = _playerTransform.position;
    }
    #endregion

    #region Public Methods
    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;

            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;

            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;

            default:
                break;
        }

        _musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        _musicSources[1].volume = musicVolumePercent * masterVolumePercent;

        TrySetVolumePrefs();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        _activeMusicSourceIndex = 1 - _activeMusicSourceIndex;
        _musicSources[_activeMusicSourceIndex].clip = clip;
        _musicSources[_activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null) AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
    }

    public void PlaySound(string soundName, Vector3 pos) => PlaySound(_soundLibrary.GetClipFromName(soundName), pos);

    public void PlaySound2D(string soundName) => _soundEffect2DSource.PlayOneShot(_soundLibrary.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
    #endregion

    #region Private Variables
    private void TryGetVolumePrefs()
    {
        var temp_masterVolumePercent = PlayerPrefs.GetFloat("master vol");
        Debug.Log("MASTER: " + temp_masterVolumePercent);
        if (temp_masterVolumePercent > 0f) masterVolumePercent = temp_masterVolumePercent;

        var temp_sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol");
        Debug.Log("SFX: " + temp_sfxVolumePercent);
        if (temp_sfxVolumePercent > 0f) sfxVolumePercent = temp_sfxVolumePercent;

        var temp_musicVolumePercent = PlayerPrefs.GetFloat("music vol");
        Debug.Log("MUSIC: " + temp_musicVolumePercent);
        if (temp_musicVolumePercent > 0f) musicVolumePercent = temp_musicVolumePercent;
    }

    private void TrySetVolumePrefs()
    {
        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
    }

    private IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            _musicSources[_activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            _musicSources[1 - _activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }
    #endregion
}