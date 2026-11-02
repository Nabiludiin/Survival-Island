using UnityEngine;

public class VelocityCalculator : MonoBehaviour
{
	public float velocityMagnitude;

	[SerializeField]
	private float maxMagnitude;

	private Vector3 previousPosition;

	public float NormalizedVelocity => Mathf.Clamp01(velocityMagnitude / maxMagnitude);

	private void Update()
	{
		Vector3 zero = Vector3.zero;
		if (Time.deltaTime > 0f)
		{
			velocityMagnitude = ((transform.position - previousPosition) / Time.deltaTime).magnitude;
		}
		else
		{
			velocityMagnitude = 0f;
		}
		previousPosition = transform.position;
	}
}
