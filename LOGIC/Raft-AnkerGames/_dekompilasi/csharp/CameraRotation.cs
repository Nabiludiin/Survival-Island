using UnityEngine;

public class CameraRotation : MonoBehaviour
{
	private Vector3 v3;

	private Vector3 p3 = new Vector3(0f, 0f, -6f);

	private float cameraZoon = 10f;

	public float speed = 100f;

	public bool zoom = true;

	private void Start()
	{
		v3 = transform.localEulerAngles;
	}

	private void LateUpdate()
	{
		if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
		{
			v3.x -= Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
			v3.y += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
		}
		v3 = clamp(v3);
		transform.localEulerAngles = v3;
		if ((bool)Camera.main && zoom)
		{
			p3.z += Input.GetAxis("Mouse ScrollWheel") * cameraZoon;
			p3.z = Mathf.Clamp(p3.z, -8.5f, -2.5f);
			Camera.main.transform.localPosition = p3;
		}
	}

	private Vector3 clamp(Vector3 v3)
	{
		if (v3.x > 360f)
		{
			v3.x -= 360f;
		}
		if (v3.x < -360f)
		{
			v3.x += 360f;
		}
		if (v3.y > 360f)
		{
			v3.y -= 360f;
		}
		if (v3.y < -360f)
		{
			v3.y += 360f;
		}
		return v3;
	}
}
