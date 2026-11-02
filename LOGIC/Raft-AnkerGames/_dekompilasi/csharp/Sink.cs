using UnityEngine;

public class Sink : MonoBehaviour
{
	[SerializeField]
	private float sinkSpeed;

	[SerializeField]
	private float rotationSped;

	[SerializeField]
	private bool sinking;

	private Vector3 randomRotationValues;

	public bool Sinking
	{
		get
		{
			return sinking;
		}
		set
		{
			if (!sinking & value)
			{
				randomRotationValues = Random.insideUnitSphere;
			}
			sinking = value;
		}
	}

	private void Update()
	{
		if (sinking)
		{
			transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
			transform.eulerAngles += randomRotationValues * Time.deltaTime * rotationSped;
		}
	}
}
