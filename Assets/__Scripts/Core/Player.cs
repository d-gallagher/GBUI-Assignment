using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
	/// <summary>
	/// Player Movement Speed
	/// </summary>
	public float moveSpeed = 5;

	private Camera _cam;
	private PlayerController _controller;

	void Start()
	{
		_controller = GetComponent<PlayerController>();
		_cam = Camera.main;
	}

	void Update()
	{
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		_controller.Move(moveVelocity);

		Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

		if (groundPlane.Raycast(ray, out float rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			Debug.DrawLine(ray.origin, point, Color.red);
			//Debug.DrawRay(ray.origin,ray.direction * 100,Color.red);
			_controller.LookAt(point);
		}
	}
}