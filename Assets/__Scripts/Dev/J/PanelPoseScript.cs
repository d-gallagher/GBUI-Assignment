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
        _text.text = this.pose.ToString();
        _activeColor = activeColor;
        _inActiveColor = inActiveColor;
    }

    private void Update() => _image.color = _isActive ? _activeColor : _inActiveColor;

    public void SetActive(bool isActive) => _isActive = isActive;
}
