using UnityEngine;

public class MyoTestScript_J : MonoBehaviour
{
    ThalmicMyo _thalmicMyo;
    private Thalmic.Myo.Pose _lastPose;

    // Start is called before the first frame update
    void Start()
    {
        _thalmicMyo = FindObjectOfType<ThalmicMyo>();
        bool myoFound = _thalmicMyo != null;
        Debug.Log($"Myo Found: {myoFound}");
    }

    // Update is called once per frame
    void Update()
    {
        if (_thalmicMyo.pose != _lastPose)
        {
            _lastPose = _thalmicMyo.pose;
            OnNewPose(_thalmicMyo.pose);
        }
    }

    private void OnNewPose(Thalmic.Myo.Pose newPose)
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

    private void OnRest()
    {
        Debug.Log("Rest");
    }

    private void OnFist()
    {
        Debug.Log("Fist");
    }

    private void OnWaveIn()
    {
        Debug.Log("Wave In");
    }

    private void OnWaveOut()
    {
        Debug.Log("Wave Out");
    }

    private void OnFingerSpread()
    {
        Debug.Log("Finger Spread");
    }

    private void OnDoubleTap()
    {
        Debug.Log("Double Tap");
    }

    private void OnUnknown()
    {
        Debug.Log("Unknown");
    }

}
