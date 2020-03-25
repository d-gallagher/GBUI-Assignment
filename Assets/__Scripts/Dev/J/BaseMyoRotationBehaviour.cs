﻿using UnityEngine;

public class BaseMyoRotationBehaviour : MonoBehaviour
{
    // A rotation that compensates for the Myo armband's orientation parallel to the ground, i.e. yaw.
    // Once set, the direction the Myo armband is facing becomes "forward" within the program.
    // Set by making the fingers spread pose or pressing "r".
    private Quaternion _antiYaw = Quaternion.identity;

    // A reference angle representing how the armband is rotated about the wearer's arm, i.e. roll.
    // Set by making the fingers spread pose or pressing "r".
    private float _referenceRoll = 0.0f;

    // Privates
    private ThalmicMyo _thalmicMyo;

    private void Start()
    {
        _thalmicMyo = FindObjectOfType<ThalmicMyo>();
        PanelMyoDebugScript.OnDoubleTap += ResetRotation;
    }

    private void Update()
    {
        // Current zero roll vector and roll value.
        Vector3 zeroRoll = RotationMaths.ComputeZeroRollVector(_thalmicMyo.transform.forward);
        float roll = RotationMaths.RollFromZero(zeroRoll, _thalmicMyo.transform.forward, _thalmicMyo.transform.up);

        // The relative roll is simply how much the current roll has changed relative to the reference roll.
        // adjustAngle simply keeps the resultant value within -180 to 180 degrees.
        float relativeRoll = RotationMaths.NormalizeAngle(roll - _referenceRoll);

        // antiRoll represents a rotation about the _thalmicMyo Armband's forward axis adjusting for reference roll.
        Quaternion antiRoll = Quaternion.AngleAxis(relativeRoll, _thalmicMyo.transform.forward);

        // Here the anti-roll and yaw rotations are applied to the _thalmicMyo Armband's forward direction to yield
        // the orientation of the joint.
        transform.rotation = _antiYaw * antiRoll * Quaternion.LookRotation(_thalmicMyo.transform.forward);

        // The above calculations were done assuming the Myo armbands's +x direction, in its own coordinate system,
        // was facing toward the wearer's elbow. If the Myo armband is worn with its +x direction facing the other way,
        // the rotation needs to be updated to compensate.
        if (_thalmicMyo.xDirection == Thalmic.Myo.XDirection.TowardWrist)
        {
            // Mirror the rotation around the XZ plane in Unity's coordinate system (XY plane in Myo's coordinate
            // system). This makes the rotation reflect the arm's orientation, rather than that of the Myo armband.
            transform.rotation = new Quaternion(transform.localRotation.x,
                                                -transform.localRotation.y,
                                                transform.localRotation.z,
                                                -transform.localRotation.w);
        }
    }

    public void ResetRotation()
    {
        // Update references. This anchors the joint on-screen such that it faces forward away
        // from the viewer when the Myo armband is oriented the way it is when these references are taken.
        //
        // _antiYaw represents a rotation of the Myo armband about the Y axis (up) which aligns the forward
        // vector of the rotation with Z = 1 when the wearer's arm is pointing in the reference direction.
        _antiYaw = Quaternion.FromToRotation(
            new Vector3(_thalmicMyo.transform.forward.x, 0, _thalmicMyo.transform.forward.z),
            new Vector3(0, 0, 1)
        );

        // _referenceRoll represents how many degrees the Myo armband is rotated clockwise
        // about its forward axis (when looking down the wearer's arm towards their hand) from the reference zero
        // roll direction. This direction is calculated and explained below. When this reference is
        // taken, the joint will be rotated about its forward axis such that it faces upwards when
        // the roll value matches the reference.
        Vector3 referenceZeroRoll = RotationMaths.ComputeZeroRollVector(_thalmicMyo.transform.forward);
        _referenceRoll = RotationMaths.RollFromZero(referenceZeroRoll, _thalmicMyo.transform.forward, _thalmicMyo.transform.up);
    }

    //// Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given _thalmicMyo that a user action was
    //// recognized.
    //public static void ExtendUnlockAndNotifyUserAction(ThalmicMyo _thalmicMyo)
    //{
    //    ThalmicHub hub = ThalmicHub.instance;

    //    if (hub.lockingPolicy == LockingPolicy.Standard)
    //    {
    //        _thalmicMyo.Unlock(UnlockType.Timed);
    //    }

    //    _thalmicMyo.NotifyUserAction();
    //}
}


