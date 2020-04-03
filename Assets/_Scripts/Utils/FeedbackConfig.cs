using System;
using static Enums;

[Serializable]
public class FeedbackConfig
{
    public FeedbackType feedbackType;
    public VibrateConfig vibrateConfig;
    public ShakeConfig shakeConfig;
}

[Serializable]
public class VibrateConfig
{
    public Thalmic.Myo.VibrationType vibrationType;
    public float vibrateDuration;
}


[Serializable]
public class ShakeConfig
{
    public float shakeDuration;
    public float shakeIntensity;
    public float shakeDecreaseFactor;
}
