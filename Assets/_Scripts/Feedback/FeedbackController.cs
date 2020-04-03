using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class FeedbackController : MonoBehaviour, IFeedbackController
{
    public FeedbackConfig[] feedbackConfigs;

    private IVibrateable _vibrateScript;
    private IShakeable _shakeScript;

    private Dictionary<FeedbackType, FeedbackConfig> _feedbackConfigsDict;

    private void Start()
    {
        _feedbackConfigsDict = new Dictionary<FeedbackType, FeedbackConfig>();
        foreach (var c in feedbackConfigs) _feedbackConfigsDict.Add(c.feedbackType, c);

        _vibrateScript = FindObjectOfType<MyoVibrationController>();
        _shakeScript = Camera.main.GetComponent<ShakeScript>();
    }

    public void Shake(FeedbackType feedbackType) => _shakeScript.Shake(_feedbackConfigsDict[feedbackType].shakeConfig);

    public void Vibrate(FeedbackType feedbackType) => _vibrateScript.Vibrate(_feedbackConfigsDict[feedbackType].vibrateConfig);

    public void ShakeAndVibrate(FeedbackType feedbackType)
    {
        var f = _feedbackConfigsDict[feedbackType];
        _shakeScript.Shake(f.shakeConfig);
        _vibrateScript.Vibrate(f.vibrateConfig);
    }
}