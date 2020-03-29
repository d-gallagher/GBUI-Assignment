using System;
using UnityEngine;

public sealed class MyoGestureController : MonoBehaviour
{
    private ThalmicMyo _thalmicMyo;
    private Thalmic.Myo.Pose _lastPose;

    public static event Action<Thalmic.Myo.Pose> OnNewPose;
    public static event Action OnHoldPose;

    // Start is called before the first frame update
    private void Start()
    {
        _thalmicMyo = FindObjectOfType<ThalmicMyo>();
        bool myoFound = _thalmicMyo != null;
        Debug.Log($"Myo Found: {myoFound}");
    }

    // Update is called once per frame
    private void Update()
    {
        if (_thalmicMyo.pose != _lastPose)
        {
            _lastPose = _thalmicMyo.pose;
            OnNewPose(_thalmicMyo.pose);
        }
        else
        {
            OnHoldPose?.Invoke();
        }
    }
}
