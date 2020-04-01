using Thalmic.Myo;
using UnityEngine;

public sealed class MyoVibrationController : MonoBehaviour, IVibrateable
{
    private ThalmicMyo _thalmicMyo;

    private void Start()
    {
        _thalmicMyo = FindObjectOfType<ThalmicMyo>();
        bool myoFound = _thalmicMyo != null;
    }

    public void Vibrate(VibrationType vibrationType) => _thalmicMyo.Vibrate(vibrationType);
}