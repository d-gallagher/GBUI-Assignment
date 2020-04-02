using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    public FeedbackConfig[] feedbackConfigs;

    private IVibrateable _vibrateScript;
    private ShakeScript _shakeScript;

    private void Start()
    {
        _vibrateScript = FindObjectOfType<MyoVibrationController>();
        _shakeScript = Camera.main.GetComponent<ShakeScript>();
    }

    public void Shake() { }

    public void Vibrate() { }

    public void ShakeAndVibrate() { }
}