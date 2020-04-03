using UnityEngine;

public class ShakeScript : MonoBehaviour, IShakeable
{
    private ShakeConfig _shakeConfig;

    private float _timeToShake;
    private Vector3 _originalPos;

    public void Shake(ShakeConfig shakeConfig)
    {
        _shakeConfig = shakeConfig;
        _timeToShake = _shakeConfig.shakeDuration;
    }

    private void Start() => _originalPos = transform.position;

    void Update()
    {
        if (_timeToShake >= 0f && _originalPos!=null)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * _shakeConfig.shakeIntensity;
            _timeToShake -= Time.deltaTime * _shakeConfig.shakeDecreaseFactor;
        }
        else
        {
            transform.localPosition = _originalPos;
        }
    }
}
