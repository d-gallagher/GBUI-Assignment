using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pose = Thalmic.Myo.Pose;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

/// <summary>
/// Perform actions on objects with the crane
/// - Pinch to pick up
/// - Releases to drop
/// - Up/Down to raise/lower the object
/// </summary>
public class MoveWithCrane : MonoBehaviour
{
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    ThalmicMyo thalmicMyo;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    public float smoothSpeed = 10f;
    private bool forceApplying;

    Rigidbody rb;
    // Start is called before the first frame update

    #region == Myo Pose Bools ==
    // Update references when the pose detected
    //bool updateReference = false;
    bool fistReference = false;
    bool waveInReference = false;
    bool waveOutReference = false;
    bool fingerSpreadReference = false;
    bool dTapReference = false;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Access the ThalmicMyo component attached to the Myo object.
        thalmicMyo = myo.GetComponent<ThalmicMyo>();

        // Get the pose changes and perform related action
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;
            GetPose(thalmicMyo);

        }

    }

    /// <summary>
    /// == WinchUp : WaveOut ==
    /// Raise an object
    /// </summary>
    /// <param name="toPosition"></param>
    void WinchUp() 
    {
        Debug.Log("Performing WinchUp..");
        if (waveOutReference)
        {
            rb.AddForce(Vector3.up * smoothSpeed, ForceMode.Force);
        }
        //if(Input.GetKeyDown(KeyCode.Space)) forceApplying = ! forceApplying;
    }

    /// <summary>
    /// == WinchDown : WaveIn ==
    /// Lower an object
    /// </summary>
    /// <param name="toPosition"></param>
    void WinchDown() {
        Debug.Log("Performing WinchDown..");
        if (waveInReference)
        {
            rb.AddForce(Vector3.down * smoothSpeed, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// == WinchPinch : Fist==
    /// Pick an object
    /// </summary>
    /// <param name="toPosition"></param>
    void WinchPinch() {
        Debug.Log("Performing Fist..");
        if (fistReference)
        {
            //
        }
    }

    /// <summary>
    /// == WinchRelease : FingersSpread ==
    /// Release an object
    /// </summary>
    /// <param name="toPosition"></param>
    void WinchRelease() {
        Debug.Log("Performing FingerSpread..");
        if (fingerSpreadReference)
        {

        }
    }

    /// <summary>
    /// == WinchKill : DoubleTap ==
    /// Reset pose to unknown to stop winch activity.
    /// </summary>
    void WinchKill() {
        Debug.Log("Performing DoubleTap.. ForceApplying = " + forceApplying);
        if (dTapReference)
        {
            fistReference = false;
            waveInReference = false;
            waveOutReference = false;
            fingerSpreadReference = false;
        }
    }


    /// <summary>
    /// == GetPose == 
    /// Switch between detected poses to perform actions
    /// Optional Vector3 toPos should be a lerp value for winchX on pose commands
    /// 
    /// </summary>
    /// <param name="myo"></param>
    /// <param name="toPosition"></param>
    void GetPose(ThalmicMyo myo, params Vector3[] toPosition)
    {
        switch (myo.pose)
        {
            // Do Nothing on Rest
            case Pose.Rest:
                //updateReference = true;
                break;
            
            // WinchPinch to pick up obj
            case Pose.Fist:
                //updateReference = true;
                fistReference = true;
                WinchPinch();
                break;

            // WinchDown to lower obj
            case Pose.WaveIn:
                //updateReference = true;
                waveInReference = true;
                WinchDown();
                break;

            // WinchUp to raise obj
            case Pose.WaveOut:
                //updateReference = true;
                waveOutReference = !waveOutReference;
                WinchUp();
                break;

            // WinchRelease to drop obj
            case Pose.FingersSpread:
                //updateReference = true;
                fingerSpreadReference = true;
                WinchRelease();
                break;

            // Reset pose to unknown..?
            case Pose.DoubleTap:
                //updateReference = true;
                dTapReference = true;
                forceApplying = !forceApplying;
                WinchKill();
                break;
            
            // DoNothing
            case Pose.Unknown:
                break;
            default:
                break;
        }// switch
    }
}
