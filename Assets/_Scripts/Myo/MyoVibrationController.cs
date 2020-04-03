using UnityEngine;

public sealed class MyoVibrationController : MonoBehaviour, IVibrateable
{
    private ThalmicMyo _thalmicMyo;
    private float _timeUntilNext;

    private void Start() => _thalmicMyo = FindObjectOfType<ThalmicMyo>();

    private void Update()
    {
        if (!(_timeUntilNext <= 0f)) _timeUntilNext -= Time.deltaTime;
    }

    public void Vibrate(VibrateConfig vibrateConfig)
    {
        if (_timeUntilNext <= 0f && _thalmicMyo != null)
        {
            _thalmicMyo.Vibrate(vibrateConfig.vibrationType);
            _timeUntilNext += vibrateConfig.vibrateDuration;
        }
    }
}