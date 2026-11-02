using UnityEngine;

[RequireComponent(typeof(WaterPointGetter))]
public class WaterFloatSemih : MonoBehaviour
{
	public bool onlyUnderWater;

	public bool lerp;

	public float lerpSpeed = 1f;

	public float updatePerSecond;

	private float updateTimer;

	private float lastKnownWaterPos;

	private WaterPointGetter waterPointGetter;

	private void Awake()
	{
		waterPointGetter = GetComponent<WaterPointGetter>();
	}

	protected virtual void Update()
	{
		UpdateWaterPos();
		if (onlyUnderWater)
		{
			if (transform.position.y < 0f)
			{
				SetToSurface();
			}
		}
		else
		{
			SetToSurface();
		}
	}

	private void UpdateWaterPos()
	{
		float num = 1f / updatePerSecond;
		updateTimer += Time.deltaTime;
		if (updateTimer >= num)
		{
			updateTimer -= num;
			lastKnownWaterPos = waterPointGetter.GetWaterPoint(transform.position);
		}
	}

	private void SetToSurface()
	{
		Vector3 position = transform.position;
		if (position.y != lastKnownWaterPos)
		{
			if (lerp)
			{
				position.y = Mathf.Lerp(position.y, lastKnownWaterPos, Time.deltaTime * lerpSpeed);
			}
			else
			{
				position.y = lastKnownWaterPos;
			}
			transform.position = position;
		}
	}
}
