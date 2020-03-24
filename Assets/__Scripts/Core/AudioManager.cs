using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private float _masterVolumePercent = .2f;
    private float _sfxVolumePercent = 1;
    private float _musicVolumePercent = 1f;

    private AudioSource[] _musicSources;
    private int _activeMusicSourceIndex;

    // Singleton
    public static AudioManager instance;

    Transform audioListener;
    Transform playerT;

    void Awake()
    {
        // Assign singleton
        instance = this;

        _musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++)
        {
            GameObject newMusicSource = new GameObject("Music source " + (i + 1));
            _musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }

        audioListener = FindObjectOfType<AudioListener>().transform;
        playerT = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (playerT != null) audioListener.position = playerT.position;
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
        if (clip != null) AudioSource.PlayClipAtPoint(clip, pos, _sfxVolumePercent * _masterVolumePercent);
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            _musicSources[_activeMusicSourceIndex].volume = Mathf.Lerp(0, _musicVolumePercent * _masterVolumePercent, percent);
            _musicSources[1 - _activeMusicSourceIndex].volume = Mathf.Lerp(_musicVolumePercent * _masterVolumePercent, 0, percent);
            yield return null;
        }
    }
}