using UnityEngine;

public class Rotator : MonoBehaviour
{
	[SerializeField]
	private float speed = 3f;

	[SerializeField]
	private Vector3 axis = Vector3.up;

	private void Update()
	{
		transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
	}
}
