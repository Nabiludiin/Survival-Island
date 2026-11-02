using System.Collections.Generic;
using UltimateWater;
using UnityEngine;

public class WaterFloatSemih2 : MonoBehaviour
{
	public enum DisplacementMode
	{
		Height,
		Displacement
	}

	[SerializeField]
	protected DisplacementMode _DisplacementMode;

	public float surfaceOffset;

	[Range(0.04f, 1f)]
	[SerializeField]
	protected float waterPrecision = 0.2f;

	public float lerpSpeed = 10f;

	[SerializeField]
	private bool floatTowardsSurfaceInOxygenRegion = true;

	private bool spawnedInOrUnderOxygenRegion;

	protected static Water water;

	protected Vector3 initialPosition;

	protected Vector3 previousPosition;

	protected WaterSample sample;

	public bool setToSurface = true;

	private float? lastYPosition;

	private float? smoothedYPosition;

	private float waterInterpolationDelta = 3f;

	private bool hitWaterLast;

	public float PrevYPos
	{
		get
		{
			if (!Cheat.UseOldWater)
			{
				if (!smoothedYPosition.HasValue)
				{
					SetToSurface();
					Update();
				}
				return smoothedYPosition.Value;
			}
			return previousPosition.y;
		}
	}

	public float UnSmoothedPrevYPos => previousPosition.y;

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		if (Raft_Network.InMenuScene)
		{
			water = Object.FindObjectOfType<Water>();
		}
		if (water == null && SingletonGeneric<GameManager>.Singleton != null)
		{
			water = SingletonGeneric<GameManager>.Singleton.water;
		}
		if (water == null)
		{
			Debug.Log(gameObject.name + " has no water reference at Start");
			return;
		}
		initialPosition = transform.position;
		previousPosition = initialPosition;
		sample = new WaterSample(water, (WaterSample.DisplacementMode)_DisplacementMode, waterPrecision);
		sample.Start(transform.position);
	}

	private void OnDestroy()
	{
		if (sample != null)
		{
			sample.Stop();
			sample = null;
		}
	}

	private void OnDisable()
	{
		if (sample != null)
		{
			sample.Stop();
		}
	}

	protected virtual void OnEnable()
	{
		if (sample != null)
		{
			sample.Start(transform.position);
			initialPosition = (previousPosition = transform.position);
		}
		if (floatTowardsSurfaceInOxygenRegion)
		{
			return;
		}
		List<Region> regionsFromPosition = RegionManager.GetRegionsFromPosition(transform.position);
		if (regionsFromPosition == null)
		{
			return;
		}
		foreach (Region item in regionsFromPosition)
		{
			if (item == null)
			{
				continue;
			}
			if (item.subregions != null)
			{
				Subregion[] subregions = item.subregions;
				foreach (Subregion subregion in subregions)
				{
					if (!(subregion == null))
					{
						Vector3 topCenterOfArea = subregion.regionArea.GetTopCenterOfArea();
						if (transform.position.y < topCenterOfArea.y && subregion.regionArea.GetNormalizedDistanceToPoint(new Vector3(transform.position.x, subregion.regionArea.GetCenterOfArea().y, transform.position.z)) > 0f)
						{
							spawnedInOrUnderOxygenRegion = true;
							break;
						}
					}
				}
			}
			if (spawnedInOrUnderOxygenRegion)
			{
				break;
			}
		}
	}

	private void Update()
	{
		if (!lastYPosition.HasValue)
		{
			lastYPosition = previousPosition.y;
		}
		smoothedYPosition = Mathf.Lerp(lastYPosition.Value, previousPosition.y, Time.deltaTime * waterInterpolationDelta);
		lastYPosition = smoothedYPosition.Value;
	}

	protected virtual void LateUpdate()
	{
		if (sample == null)
		{
			sample = new WaterSample(water, (WaterSample.DisplacementMode)_DisplacementMode, waterPrecision);
		}
		if (Vector3.Distance(previousPosition, transform.position) > 5f)
		{
			if (sample != null)
			{
				sample.Stop();
				sample.Start(transform.position);
			}
			initialPosition = (previousPosition = transform.position);
		}
		SetToSurface();
	}

	protected virtual void SetToSurface()
	{
		if (sample == null)
		{
			return;
		}
		initialPosition += transform.position - previousPosition;
		Vector3 andReset = sample.GetAndReset(initialPosition.x, initialPosition.z, WaterSample.ComputationsMode.ForceCompletion);
		andReset.y += surfaceOffset;
		if (setToSurface)
		{
			Vector3 position = transform.position;
			if (_DisplacementMode == DisplacementMode.Height)
			{
				andReset.x = position.x;
				andReset.z = position.z;
			}
			if (floatTowardsSurfaceInOxygenRegion || !spawnedInOrUnderOxygenRegion)
			{
				transform.position = Vector3.Lerp(transform.position, andReset, Time.deltaTime * lerpSpeed);
			}
		}
		previousPosition = andReset;
	}

	protected void ResetFloating()
	{
		if (sample == null)
		{
			sample = new WaterSample(water, (WaterSample.DisplacementMode)_DisplacementMode, waterPrecision);
		}
		if (sample != null)
		{
			sample.Start(transform.position);
			initialPosition = (previousPosition = transform.position);
		}
	}
}
