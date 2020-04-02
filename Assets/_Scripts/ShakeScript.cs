using UnityEngine;

public class ShakeScript : MonoBehaviour
{
    // How long the object should shake for.
    public float duration = 1f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float intensity = 1f;
    public float decreaseFactor = 1f;
    public bool _isEnabled;

    private Vector3 _originalPos;

    private float _timeToShake;

    public void Shake() => _timeToShake = duration;

    private void Start()
    {
        _isEnabled = true;
        _originalPos = transform.position;
    }

    void Update()
    {
        if (_timeToShake >= 0f)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * intensity;
            _timeToShake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            transform.localPosition = _originalPos;
        }
    }
}
