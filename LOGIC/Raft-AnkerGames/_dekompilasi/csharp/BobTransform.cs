using UnityEngine;

public class BobTransform : MonoBehaviour
{
	public bool randomizeTimeOffset;

	[Space(10f)]
	[Header("Up and down settings")]
	public float frequency = 20f;

	public float magnitude = 0.5f;

	public Vector3 waterOffset;

	[Header("Rotate settings")]
	public float frequencyRotation = 20f;

	public float magnitudeRotation = 0.5f;

	public Vector3 rotationOffset;

	public Transform rotatePoint;

	public float angle;

	private Vector3 startPos;

	public float timeOffset;

	public Vector3 targetEuler;

	public float sin;

	private void Start()
	{
		startPos = transform.position;
		targetEuler = transform.eulerAngles;
		if (randomizeTimeOffset)
		{
			timeOffset = Random.Range(-10000, 10000);
		}
	}

	private void Update()
	{
		startPos.x = transform.position.x;
		startPos.z = transform.position.z;
		transform.position = startPos + waterOffset + Vector3.up * Mathf.Sin((Time.time + timeOffset) * frequency) * magnitude;
	}
}
