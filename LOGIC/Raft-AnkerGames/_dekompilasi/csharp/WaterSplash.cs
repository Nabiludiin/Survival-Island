using FMODUnity;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
	[SerializeField]
	private WaterPointGetter waterPointGetter;

	[SerializeField]
	private float customWaterLevel;

	[SerializeField]
	private ParticleSystem particleSystem;

	[SerializeField]
	private bool onWayDown = true;

	[SerializeField]
	private bool onWayUp = true;

	public EventReference field_upSound;

	public EventReference field_downSound;

	private float previousPosition;

	private void LateUpdate()
	{
		float waterPoint = customWaterLevel;
		if (waterPointGetter != null)
		{
			waterPoint = waterPointGetter.GetWaterPoint(transform.position);
		}
		if (onWayUp && transform.position.y >= waterPoint && previousPosition < waterPoint)
		{
			if (particleSystem != null)
			{
				particleSystem.Play();
			}
			RuntimeManager.PlayOneShot(field_upSound, transform.position);
		}
		if (onWayDown && transform.position.y <= waterPoint && previousPosition > waterPoint)
		{
			if (particleSystem != null)
			{
				particleSystem.Play();
			}
			RuntimeManager.PlayOneShot(field_downSound, transform.position);
		}
		previousPosition = transform.position.y;
	}
}
