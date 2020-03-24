using System.Collections;
using UnityEngine;
using static Enums;

public class AudioManager : MonoBehaviour
{
    public float masterVolumePercent = 1;
    public float sfxVolumePercent = 1;
    public float musicVolumePercent = 1f;

    private AudioSource _soundEffect2DSource;
    private AudioSource[] _musicSources;
    private int _activeMusicSourceIndex;

    // Singleton
    public static AudioManager instance;

    private Transform _audioListenerTransform;
    private Transform _playerTransform;
    private SoundLibrary _soundLibrary;

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
            _playerTransform = FindObjectOfType<Player>().transform;

            masterVolumePercent = PlayerPrefs.GetFloat("master vol");
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol");
            musicVolumePercent = PlayerPrefs.GetFloat("music vol");
        }
    }

    private void Update()
    {
        if (_playerTransform != null) _audioListenerTransform.position = _playerTransform.position;
    }

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

        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
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
}