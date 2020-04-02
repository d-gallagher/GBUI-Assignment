using Thalmic.Myo;
using UnityEngine;

public sealed class MyoVibrationController : MonoBehaviour, IVibrateable
{
    public float timeBetweenShort = 0.3f;
    private ThalmicMyo _thalmicMyo;
    private float _timeUntilNext;

    private void Start() => _thalmicMyo = FindObjectOfType<ThalmicMyo>();

    private void Update()
    {
        if (!(_timeUntilNext <= 0f)) _timeUntilNext -= Time.deltaTime;
    }

    public void Vibrate(VibrationType vibrationType)
    {
        if (_timeUntilNext <= 0f)
        {
            _thalmicMyo.Vibrate(vibrationType);
            _timeUntilNext += timeBetweenShort;
        }

    }
}