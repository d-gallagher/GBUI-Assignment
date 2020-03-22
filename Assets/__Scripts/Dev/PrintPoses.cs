using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class PrintPoses : MonoBehaviour
{
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Access the ThalmicMyo component attached to the Myo object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        // Update references when the pose becomes fingers spread or the q key is pressed.
        bool updateReference = false;
        bool fistReference = false;
        bool waveInReference = false;
        bool waveOutReference = false;
        bool fingerSpreadReference = false;
        bool dTapReference = false;

        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.FingersSpread)
            {
                updateReference = true;

                //ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
        if (Input.GetKeyDown("r"))
        {
            updateReference = true;
        }

        switch (thalmicMyo.pose)
        {
            case Pose.Rest:
                // Default
                updateReference = true;
                fistReference = false;
                waveInReference = false;
                waveOutReference = false;
                fingerSpreadReference = false;
                dTapReference = false;
                //Debug.Log("Pose: " + thalmicMyo.pose);
                break;
            case Pose.Fist:
                // Decrease Velocity
                updateReference = true;
                fistReference = true;
                waveInReference = false;
                waveOutReference = false;
                fingerSpreadReference = false;
                dTapReference = false;
                Debug.Log("Pose: " + thalmicMyo.pose);

                break;
            case Pose.WaveIn:
                // Nose Dive
                updateReference = true;
                fistReference = false;
                waveInReference = true;
                waveOutReference = false;
                fingerSpreadReference = false;
                dTapReference = false;
                Debug.Log("Pose: " + thalmicMyo.pose);

                break;
            case Pose.WaveOut:
                // Nose Climb
                updateReference = true;
                fistReference = false;
                waveInReference = false;
                waveOutReference = true;
                fingerSpreadReference = false;
                dTapReference = false;
                Debug.Log("Pose: " + thalmicMyo.pose);

                break;
            case Pose.FingersSpread:
                // Increase Velocity
                updateReference = true;
                fistReference = false;
                waveInReference = false;
                waveOutReference = false;
                fingerSpreadReference = true;
                dTapReference = false;
                Debug.Log("Pose: " + thalmicMyo.pose);

                break;
            case Pose.DoubleTap:
                // Something interesting..?
                fistReference = false;
                waveInReference = false;
                waveOutReference = false;
                fingerSpreadReference = false;
                dTapReference = true;
                Debug.Log("Pose: " + thalmicMyo.pose);

                break;
            case Pose.Unknown:
                // Default
                break;
            default:
                break;
        }// switch
    }
}
