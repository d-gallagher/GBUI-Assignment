using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelMyoDebugScript : BaseMyoBehaviour
{
    public int textSize = 14;
    public GameObject panelPosePrefab;
    public Color activeColor = Color.green;
    public Color inActiveColor = Color.red;

    private Dictionary<Thalmic.Myo.Pose, PanelPoseScript> _poseScriptDict;
    private PanelPoseScript _lastPoseScript;

    protected override void Start()
    {
        base.Start();

        _poseScriptDict = new Dictionary<Thalmic.Myo.Pose, PanelPoseScript>();

        var poses = Enum.GetValues(typeof(Thalmic.Myo.Pose)).Cast<Thalmic.Myo.Pose>();
        foreach (var p in poses)
        {
            var panel = Instantiate(panelPosePrefab, transform);
            var ps = panel.GetComponent<PanelPoseScript>();

            _poseScriptDict.Add(p, ps);

            ps.Initialise(p, activeColor, inActiveColor);
        }
    }

    protected override void OnNewPose(Thalmic.Myo.Pose newPose)
    {
        if (_lastPoseScript != null) _lastPoseScript.SetActive(false);

        var script = _poseScriptDict[newPose];
        script.SetActive(true);
        _lastPoseScript = script;
    }

    protected override void HoldLastPose()
    {
        if (_lastPoseScript != null)
        {
            Debug.Log("HOLDING POSE: " + _lastPoseScript.pose);
            _lastPoseScript.SetActive(true);
        }
    }
}
