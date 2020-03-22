using UnityEngine;

public abstract class BaseMyoBehaviour : MonoBehaviour
{
    protected ThalmicMyo _thalmicMyo;
    protected Thalmic.Myo.Pose _lastPose;

    // Start is called before the first frame update
    protected void OnStart()
    {
        _thalmicMyo = FindObjectOfType<ThalmicMyo>();
        bool myoFound = _thalmicMyo != null;
        Debug.Log($"Myo Found: {myoFound}");
    }

    // Update is called once per frame
    protected void OnUpdate()
    {
        if (_thalmicMyo.pose != _lastPose)
        {
            _lastPose = _thalmicMyo.pose;
            OnNewPose(_thalmicMyo.pose);
        }
    }

    protected void OnNewPose(Thalmic.Myo.Pose newPose)
    {
        switch (newPose)
        {
            case Thalmic.Myo.Pose.Rest:
                OnRest();
                break;
            case Thalmic.Myo.Pose.Fist:
                OnFist();
                break;
            case Thalmic.Myo.Pose.WaveIn:
                OnWaveIn();
                break;
            case Thalmic.Myo.Pose.WaveOut:
                OnWaveOut();
                break;
            case Thalmic.Myo.Pose.FingersSpread:
                OnFingerSpread();
                break;
            case Thalmic.Myo.Pose.DoubleTap:
                OnDoubleTap();
                break;
            case Thalmic.Myo.Pose.Unknown:
                OnUnknown();
                break;
            default:
                break;
        }
    }

    protected abstract void OnRest();

    protected abstract void OnFist();

    protected abstract void OnWaveIn();

    protected abstract void OnWaveOut();

    protected abstract void OnFingerSpread();

    protected abstract void OnDoubleTap();

    protected abstract void OnUnknown();
}
