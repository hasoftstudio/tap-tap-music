using System;
using UnityEngine;

public class ChinarUi3DCamera : MonoBehaviour
{
	public Transform pivot;

	public Vector3 pivotOffset = Vector3.zero;

	public Transform target;

	public float distance = 10f;

	public float minDistance = 2f;

	public float maxDistance = 15f;

	public float zoomSpeed = 1f;

	public float xSpeed = 250f;

	public float ySpeed = 120f;

	public bool allowYTilt = true;

	public float yMinLimit = -90f;

	public float yMaxLimit = 90f;

	private float x;

	private float y;

	private float targetX;

	private float targetY;

	private float xVelocity = 1f;

	private float yVelocity = 1f;

	private float zoomVelocity = 1f;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.targetX = (this.x = eulerAngles.x);
		this.targetY = (this.y = ChinarUi3DCamera.ClampAngle(eulerAngles.y, this.yMinLimit, this.yMaxLimit));
	}

	private void LateUpdate()
	{
		if (!this.pivot)
		{
			return;
		}
		float axis = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			this.distance -= this.zoomSpeed;
		}
		else if (axis < 0f)
		{
			this.distance += this.zoomSpeed;
		}
		this.distance = Mathf.Clamp(this.distance, this.minDistance, this.maxDistance);
		if (Input.GetMouseButton(1) || (Input.GetMouseButton(0) && (UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(KeyCode.RightControl))))
		{
			this.targetX += UnityEngine.Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			if (this.allowYTilt)
			{
				this.targetY -= UnityEngine.Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
				this.targetY = ChinarUi3DCamera.ClampAngle(this.targetY, this.yMinLimit, this.yMaxLimit);
			}
		}
		this.x = Mathf.SmoothDampAngle(this.x, this.targetX, ref this.xVelocity, 0f);
		this.y = ((!this.allowYTilt) ? this.targetY : Mathf.SmoothDampAngle(this.y, this.targetY, ref this.yVelocity, 0f));
		Quaternion rotation = Quaternion.Euler(this.y, this.x, 0f);
		this.distance = Mathf.SmoothDamp(this.distance, this.distance, ref this.zoomVelocity, 0f);
		Vector3 position = rotation * new Vector3(0f, 0f, -this.distance) + this.pivot.position + this.pivotOffset;
		base.transform.rotation = rotation;
		base.transform.position = position;
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
