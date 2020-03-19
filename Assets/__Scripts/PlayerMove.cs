using UnityEngine;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class PlayerMove : MonoBehaviour
{

    [Header("Myo")]
    public GameObject myoGameObject;
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float rotationSensitivity = .01f;
    public float deadZoneBorder = 15;


    private ThalmicMyo _myo;
    private CharacterController _characterController;

    private Vector3 _originalRotation = Vector3.zero;
    private Vector3 _currentRotation = Vector3.zero;

    public float xAngle, yAngle, zAngle;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _myo = myoGameObject.GetComponent<ThalmicMyo>();
        _originalRotation = transform.localRotation.eulerAngles;
    }

    void FixedUpdate()
    {
        float z = Input.GetAxis("Vertical") * moveSpeed;
        float x = Input.GetAxis("Horizontal") * moveSpeed;

        Move(new Vector3(x, 0, z));

        UpdateCraneRotation();

        transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Original Rotation: "+_originalRotation);
            Debug.Log("Current Rotation: "+_currentRotation);
            Debug.Log("Myo Rotation: "+ _myo.transform.localRotation.eulerAngles.y);
        }
    }

    void Move(Vector3 moveDirection)
    {
        moveDirection.y = -9.8f;

        moveDirection = transform.TransformDirection(moveDirection);

        _characterController.Move(moveDirection * Time.deltaTime);
    }

    void UpdateCraneRotation()
    {
        // Get Myo rotation in z
        //_originalRotation = _currentRotation;

        float myoYRot = _myo.transform.localRotation.eulerAngles.y;
        if (myoYRot <= 360 && myoYRot >= 270)
        {
            myoYRot -= 360;
        }
        _currentRotation = new Vector3(0f, _myo.transform.localRotation.eulerAngles.y, 0f);


    }
}
