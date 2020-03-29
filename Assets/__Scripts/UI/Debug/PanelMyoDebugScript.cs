using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelMyoDebugScript : MonoBehaviour, IMyoGesturable
{
    #region Public Variables
    public bool isLoggingToConsole = false;
    public GameObject panelPosePrefab;
    public Color activeColor = Color.green;
    public Color inActiveColor = Color.red;
    #endregion

    #region Private Variables
    private Dictionary<Thalmic.Myo.Pose, PanelPoseScript> _poseScriptDict;
    private PanelPoseScript _lastPoseScript;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Set up the dict for fast access to each Panel via the relevant Pose
        _poseScriptDict = new Dictionary<Thalmic.Myo.Pose, PanelPoseScript>();

        // Subsribe to relevant events on controller
        MyoGestureController.OnNewPose += OnNewPose;
        MyoGestureController.OnHoldPose += OnHoldPose;

        // Build a collection of all Pose enum values and iterate through
        var poses = Enum.GetValues(typeof(Thalmic.Myo.Pose)).Cast<Thalmic.Myo.Pose>();
        foreach (var p in poses)
        {
            // Create a panel for each Pose and add to this GameObject
            var panel = Instantiate(panelPosePrefab, transform);
            var ps = panel.GetComponent<PanelPoseScript>();

            // Also add to dict for quick access
            _poseScriptDict.Add(p, ps);
            // Set up the Pose Panel with relevant values
            ps.Initialise(p, activeColor, inActiveColor);
        }
    }
    #endregion

    #region Implementation of IMyoGesturable
    public void OnNewPose(Thalmic.Myo.Pose newPose)
    {
        if (isLoggingToConsole) Debug.Log("NEW POSE: " + newPose);
        // If a panel is currently set to active, deactivate it
        if (_lastPoseScript != null) _lastPoseScript.SetActive(false);

        // Set the relevant panel to active and remember
        var script = _poseScriptDict[newPose];
        script.SetActive(true);
        _lastPoseScript = script;
    }

    public void OnHoldPose()
    {
        // Keep the last pose panel active if one has been set
        if (_lastPoseScript != null)
        {
            if (isLoggingToConsole) Debug.Log("HOLDING POSE: " + _lastPoseScript.pose);
            _lastPoseScript.SetActive(true);
        }
    }
    #endregion
}
