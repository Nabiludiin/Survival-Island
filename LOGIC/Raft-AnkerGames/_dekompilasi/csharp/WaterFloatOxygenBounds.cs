using System.Collections.Generic;
using UnityEngine;

public class WaterFloatOxygenBounds : WaterFloatSemih2
{
	[SerializeField]
	private float stationaryWaterFloatInRegionAmplitude = 0.01f;

	[SerializeField]
	private Subregion oxygenRegion;

	public SubRegionComponent_OxygenBounds oxygenRegionComponent;

	[SerializeField]
	private Transform checkForRegionOffset;

	[SerializeField]
	private bool doAdditionalSurfaceRaycast;

	[SerializeField]
	private TimerEventer raycastTimer;

	[SerializeField]
	private float surfaceRaycastUpwardsDistance = 1f;

	private bool hadRegionLastFrame;

	private bool hitRoof;

	public bool debug;

	public bool HasOxygenRegion
	{
		get
		{
			if (oxygenRegion != null)
			{
				return oxygenRegionComponent != null;
			}
			return false;
		}
	}

	public Vector3 CheckFromPosition
	{
		get
		{
			if (HasOxygenRegion)
			{
				return new Vector3(transform.position.x, transform.position.y + (0f - surfaceOffset) + 0.3f, transform.position.z);
			}
			return new Vector3(transform.position.x, (checkForRegionOffset == null) ? transform.position.y : checkForRegionOffset.position.y, transform.position.z);
		}
	}

	protected override void LateUpdate()
	{
		SetToSurface();
	}

	protected override void SetToSurface()
	{
		if (oxygenRegion != null && !setToSurface && oxygenRegion.regionArea.GetNormalizedDistanceToPoint(CheckFromPosition) <= 0f)
		{
			DisconnectCurrentRegion();
		}
		FindOxygenRegionAndComponent();
		if (debug)
		{
			Debug.Log(HasOxygenRegion);
		}
		if (setToSurface && doAdditionalSurfaceRaycast && (raycastTimer.DoUpdate(Time.deltaTime, autoResetTimer: true) || hitRoof))
		{
			if (RoofColliderInTheWay())
			{
				setToSurface = false;
				hitRoof = true;
			}
			else
			{
				hitRoof = false;
			}
		}
		if (!HasOxygenRegion && !hadRegionLastFrame)
		{
			base.SetToSurface();
		}
		else
		{
			SetToSurface_OxygenRegion();
		}
		hadRegionLastFrame = HasOxygenRegion;
	}

	private void DisconnectCurrentRegion()
	{
		oxygenRegion = null;
		oxygenRegionComponent = null;
	}

	private void SetToSurface_OxygenRegion()
	{
		if (HasOxygenRegion)
		{
			if (setToSurface)
			{
				transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, oxygenRegionComponent.SurfacePointY + surfaceOffset, transform.position.z), Time.deltaTime * lerpSpeed);
			}
			previousPosition = transform.position;
			previousPosition.y = oxygenRegionComponent.SurfacePointY;
		}
	}

	private void FindOxygenRegionAndComponent()
	{
		if (!(oxygenRegionComponent == null) && !(oxygenRegion == null))
		{
			return;
		}
		List<Region> regionsFromPosition = RegionManager.GetRegionsFromPosition(CheckFromPosition, typeof(SubRegionComponent_OxygenBounds));
		if (!regionsFromPosition.ContainsItems())
		{
			return;
		}
		foreach (Region item in regionsFromPosition)
		{
			List<Subregion> subregionsFromPosition = item.GetSubregionsFromPosition(CheckFromPosition);
			if (subregionsFromPosition.ContainsItems())
			{
				foreach (Subregion item2 in subregionsFromPosition)
				{
					oxygenRegionComponent = item2.GetSubRegionComponent<SubRegionComponent_OxygenBounds>();
					if (oxygenRegionComponent != null)
					{
						oxygenRegion = item2;
						break;
					}
				}
			}
			if (oxygenRegionComponent != null)
			{
				break;
			}
		}
	}

	private bool RoofColliderInTheWay()
	{
		Physics.BoxCast(transform.position, Vector3.one / 8f, Vector3.up, out var hitInfo, Quaternion.identity, surfaceRaycastUpwardsDistance, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore);
		return hitInfo.collider != null;
	}
}
