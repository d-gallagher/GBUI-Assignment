using UnityEngine;

public abstract class BaseMyoBehaviour : MonoBehaviour
{
    protected ThalmicMyo _thalmicMyo;
    protected Thalmic.Myo.Pose _lastPose;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _thalmicMyo = FindObjectOfType<ThalmicMyo>();
        bool myoFound = _thalmicMyo != null;
        Debug.Log($"Myo Found: {myoFound}");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (_thalmicMyo.pose != _lastPose)
        {
            _lastPose = _thalmicMyo.pose;
            OnNewPose(_thalmicMyo.pose);
        } else
        {
            Debug.Log("HOLDING POSE");
            HoldLastPose();
        }
    }

    protected abstract void OnNewPose(Thalmic.Myo.Pose newPose);
    protected abstract void HoldLastPose();
}
