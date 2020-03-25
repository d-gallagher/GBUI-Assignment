using UnityEngine;

public class MyoTestScript : BaseMyoBehaviour_OLD
{
    // Start is called before the first frame update
    void Start() => base.OnStart();

    // Update is called once per frame
    void Update() => base.OnUpdate();

    private string baseMsg = "Got Pose: ";

    protected override void OnDoubleTap()
    {
        Debug.Log(baseMsg + "Double Tap");
    }

    protected override void OnFingerSpread()
    {
        Debug.Log(baseMsg + "Finger Spread");
    }

    protected override void OnFist()
    {
        Debug.Log(baseMsg + "Fist");
    }

    protected override void OnRest()
    {
        Debug.Log(baseMsg + "Rest");
    }

    protected override void OnUnknown()
    {
        Debug.Log(baseMsg + "Unknown");
    }

    protected override void OnWaveIn()
    {
        Debug.Log(baseMsg + "Wave In");
    }

    protected override void OnWaveOut()
    {
        Debug.Log(baseMsg + "Wave Out");
    }
}
