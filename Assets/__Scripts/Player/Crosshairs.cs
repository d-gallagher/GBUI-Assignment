using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    #region Public Variables
    public float rotationSpeed = -40.0f;
    public float rayDistance = 100.0f;
    public LayerMask targetMask;
    public SpriteRenderer dot;
    public Color dotHighlightColor;
    #endregion

    #region Private Variables
    private Color _originalDotColor;
    #endregion

    #region Unity Methods
    private void Start()
    {
        Cursor.visible = false;
        _originalDotColor = dot.color;
    }

    private void Update() => transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    #endregion

    public void DetectTargets(Ray ray)
    {
        if (Physics.Raycast(ray, rayDistance, targetMask))
        {
            dot.color = dotHighlightColor;
        }
        else
        {
            dot.color = _originalDotColor;
        }
    }
}
