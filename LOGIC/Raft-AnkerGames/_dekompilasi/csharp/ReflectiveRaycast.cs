using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[RequireComponent(typeof(TimerEventer))]
public class ReflectiveRaycast : MonoBehaviour
{
	[HideInInspector]
	public List<RaycastHit> storedHits;

	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private SO_Data_ReflectiveRaycast raycastData;

	[SerializeField]
	protected Transform startTransform;

	[SerializeField]
	private TimerEventer doCastTimer;

	[SerializeField]
	protected Transform hitVFX;

	[SerializeField]
	protected Transform startVFX;

	[SerializeField]
	private Transform bounceVFXPrefab;

	[SerializeField]
	[EventRef]
	private string eventRef_damagePlayer;

	private List<Vector3> lineRendererPoints = new List<Vector3>();

	private List<RaycastHit> calculationHits = new List<RaycastHit>();

	private List<Transform> bounceTransforms = new List<Transform>();

	private bool isActiveCasting;

	private Network_Host network_Host;

	private float dealtDamageTimeStamp;

	private void Awake()
	{
		SetCastingActive(raycastData.isActiveDefault);
		if (raycastData.storeHits)
		{
			storedHits = new List<RaycastHit>();
		}
		for (int i = 0; i < raycastData.maxBounces; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate(bounceVFXPrefab, base.transform);
			transform.gameObject.SetActive(value: false);
			bounceTransforms.Add(transform);
		}
		TimerEventer timerEventer = doCastTimer;
		timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(NewCast));
	}

	public void Update()
	{
		if (isActiveCasting)
		{
			doCastTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
		}
	}

	public void NewCast()
	{
		calculationHits.Clear();
		int bounces = raycastData.maxBounces;
		Vector3 endPoint = Vector3.zero;
		DoReflectiveCast(startTransform.position, startTransform.forward, ref calculationHits, ref bounces, ref endPoint);
		SetupLineRendererFromHits(lineRenderer, startTransform.position, endPoint, calculationHits);
		if (raycastData.storeHits)
		{
			storedHits = calculationHits;
		}
		HandleVFX(calculationHits, endPoint);
	}

	public void SetCastingActive(bool value)
	{
		isActiveCasting = value;
		startVFX.gameObject.SetActiveSafe(value);
		if (!value)
		{
			ClearData();
			ClearLinerenderer(lineRenderer);
		}
	}

	protected virtual void HandleCastInteract(GameObject go)
	{
	}

	protected virtual void HandleCastDamage(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		Network_Player componentInChildren = go.GetComponentInChildren<Network_Player>();
		if (componentInChildren == null || !(Time.time - dealtDamageTimeStamp > raycastData.dealDamageCooldown))
		{
			return;
		}
		dealtDamageTimeStamp = Time.time;
		if (Raft_Network.IsHost)
		{
			if (network_Host == null)
			{
				network_Host = ComponentManager<Network_Host>.Value;
			}
			network_Host.DamageEntity(componentInChildren.Stats, go.transform, raycastData.damage, go.transform.position, Vector3.up, EntityType.Environment);
		}
		RuntimeManager.PlayOneShot(eventRef_damagePlayer, componentInChildren.transform.position);
	}

	protected virtual void HandleVFX(List<RaycastHit> hits, Vector3 endPoint)
	{
		if (hitVFX != null)
		{
			bool flag = false;
			if (hits != null && hits.Count > 0 && hits[hits.Count - 1].point == endPoint)
			{
				flag = true;
			}
			hitVFX.gameObject.SetActiveSafe(flag);
			if (flag)
			{
				hitVFX.position = endPoint;
			}
		}
		if (hits == null || hits.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < bounceTransforms.Count; i++)
		{
			Transform transform = bounceTransforms[i];
			if (i < hits.Count - 1)
			{
				transform.gameObject.SetActiveSafe(value: true);
				transform.position = hits[i].point;
			}
			else
			{
				transform.gameObject.SetActiveSafe(value: false);
			}
		}
	}

	private void DoReflectiveCast(Vector3 fromPosition, Vector3 direction, ref List<RaycastHit> hits, ref int bounces, ref Vector3 endPoint)
	{
		Ray ray = new Ray(fromPosition, direction);
		Physics.Raycast(ray, out var hitInfo, raycastData.raySegmentMaxLength, raycastData.layer_Hit, QueryTriggerInteraction.Ignore);
		if (hitInfo.collider != null)
		{
			hitInfo.point += ray.direction.normalized * raycastData.hitPointOffset;
			hits.Add(hitInfo);
			bounces--;
			endPoint = hitInfo.point;
			if (raycastData.shouldInteract && (int)raycastData.layer_Interact == ((int)raycastData.layer_Interact | (1 << hitInfo.collider.gameObject.layer)))
			{
				HandleCastInteract(hitInfo.collider.gameObject);
			}
			else if (raycastData.dealsDamage && (int)raycastData.layer_dealDamage == ((int)raycastData.layer_dealDamage | (1 << hitInfo.collider.gameObject.layer)))
			{
				HandleCastDamage(hitInfo.collider.gameObject);
			}
			if (bounces > 0 && (int)raycastData.layer_Bounce == ((int)raycastData.layer_Bounce | (1 << hitInfo.collider.gameObject.layer)))
			{
				Vector3 direction2 = ((raycastData.reflectionMode == SO_Data_ReflectiveRaycast.ReflectionMode.OBJECTNORMAL) ? hitInfo.normal : Vector3.Reflect(direction, hitInfo.normal));
				DoReflectiveCast(hitInfo.point, direction2, ref hits, ref bounces, ref endPoint);
			}
		}
		else
		{
			endPoint = ray.GetPoint(raycastData.raySegmentMaxLength);
		}
	}

	private void SetupLineRendererFromHits(LineRenderer renderer, Vector3 startPosition, Vector3 endPoint, List<RaycastHit> hits)
	{
		Vector3[] array = HitsToPoints(startPosition, endPoint, hits).ToArray();
		renderer.positionCount = array.Length;
		renderer.SetPositions(array);
	}

	private List<Vector3> HitsToPoints(Vector3 startPoint, Vector3 endPoint, List<RaycastHit> hits)
	{
		lineRendererPoints.Clear();
		lineRendererPoints.Add(startPoint);
		if (hits.Count > 0)
		{
			for (int i = 0; i < hits.Count; i++)
			{
				lineRendererPoints.Add(hits[i].point);
			}
			lineRendererPoints.Add(endPoint);
		}
		else
		{
			lineRendererPoints.Add(endPoint);
		}
		return lineRendererPoints;
	}

	private void ClearData()
	{
		if (storedHits != null)
		{
			storedHits.Clear();
		}
		lineRendererPoints.Clear();
		calculationHits.Clear();
	}

	private void ClearLinerenderer(LineRenderer renderer)
	{
		renderer.positionCount = 0;
	}
}
