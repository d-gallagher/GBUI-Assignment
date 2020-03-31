using UnityEngine;
using UnityEngine.UI;

public class PanelPoseScript : MonoBehaviour
{
    public Thalmic.Myo.Pose pose { get; private set; }
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image _image;

    private Color _activeColor;
    private Color _inActiveColor;
    private bool _isActive;

    public void Initialise(Thalmic.Myo.Pose pose, Color activeColor, Color inActiveColor)
    {
        this.pose = pose;
        _text.text = this.pose.ToString()+"\n"+GetPoseAction(pose);
        _activeColor = activeColor;
        _inActiveColor = inActiveColor;
    }

    private void Update() => _image.color = _isActive ? _activeColor : _inActiveColor;

    public void SetActive(bool isActive) => _isActive = isActive;

    private string GetPoseAction(Thalmic.Myo.Pose pose)
    {
        string poseAction = "pose";
        switch (pose)
        {
            case Thalmic.Myo.Pose.Rest:
                poseAction = "";
                break;
            case Thalmic.Myo.Pose.Fist:
                poseAction = "Fire Primary Weapon";
                break;
            case Thalmic.Myo.Pose.WaveIn:
                poseAction = "Previous Weapon";
                break;
            case Thalmic.Myo.Pose.WaveOut:
                poseAction = "Next Weapon";
                break;
            case Thalmic.Myo.Pose.FingersSpread:
                poseAction = "Secondary Weapon";
                break;
            case Thalmic.Myo.Pose.DoubleTap:
                poseAction = "Reset Aim - 12 O'Clock";
                break;
            case Thalmic.Myo.Pose.Unknown:
                poseAction = "";
                break;
            default:
                break;
        }
        return poseAction;
    }
}
