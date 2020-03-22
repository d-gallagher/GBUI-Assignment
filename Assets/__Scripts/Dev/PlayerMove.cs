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
    //public float rotationSpeed = 5f;
    //public float rotationSensitivity = .01f;
    //public float deadZoneBorder = 15;


    private ThalmicMyo _myo;
    private CharacterController _characterController;
    private Transform _player;

    private Vector3 _originalRotation = Vector3.zero;
    private Vector3 _currentRotation = Vector3.zero;


    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _myo = myoGameObject.GetComponent<ThalmicMyo>();
        _originalRotation = transform.localRotation.eulerAngles;
        _player = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        float z = Input.GetAxis("Vertical") * moveSpeed;
        float x = Input.GetAxis("Horizontal") * moveSpeed;

        Move(new Vector3(x, 0, z));

        UpdateCraneRotation();

              
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
        _player.eulerAngles = new Vector3(0f, _myo.transform.localRotation.eulerAngles.y, 0f);

    }
}
