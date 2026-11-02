using UnityEngine;

public class SeagullPoint : MonoBehaviour
{
	[SerializeField]
	private Interval_Float rotationSpeedInterval;

	[SerializeField]
	private float rotationSpeed;

	[SerializeField]
	private int directionModifier;

	private void Start()
	{
		directionModifier = ((Random.Range(0, 2) != 0) ? 1 : (-1));
		rotationSpeed = rotationSpeedInterval.GetRandomValue();
	}

	private void Update()
	{
		float num = rotationSpeed * (float)directionModifier;
		if (transform.parent != null)
		{
			transform.RotateAround(transform.parent.position, Vector3.up, num * Time.deltaTime);
		}
	}
}
