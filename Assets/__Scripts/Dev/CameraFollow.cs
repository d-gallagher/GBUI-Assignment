using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 10f;
    public Vector3 cameraOffset;

    //private Vector3 offset;

    private void Start()
    {
        //offset = target.position - transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + cameraOffset;
        //Vector3 desiredPosition = target.position + offset;
        Vector3 smothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smothedPosition;
        transform.LookAt(target);
    }
}
