using UnityEngine;
using UnityEngine.AI;

public class LandmarkCaveEntrance : MonoBehaviour
{
	[SerializeField]
	private TerrainCollider terrainCollider;

	private NavMeshLink navmeshLink;

	public AreaZone areaZone;

	public Collider playerCollider;

	public Network_Player localPlayer;

	private float canCheckZoneTimer = 0.5f;

	private void OnValidate()
	{
		if (terrainCollider == null)
		{
			Landmark componentInParent = GetComponentInParent<Landmark>();
			if (componentInParent != null)
			{
				terrainCollider = componentInParent.GetComponentInChildren<TerrainCollider>(includeInactive: true);
			}
		}
	}

	private void Update()
	{
		if (areaZone == null)
		{
			enabled = false;
		}
		else if (canCheckZoneTimer > 0f)
		{
			canCheckZoneTimer -= Time.deltaTime;
		}
		else if (localPlayer != null && areaZone.GetNormalizedDistanceToPoint(localPlayer.FeetPosition) <= 0f)
		{
			if (playerCollider != null)
			{
				Physics.IgnoreCollision(terrainCollider, playerCollider, ignore: false);
			}
			playerCollider = null;
			localPlayer = null;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Network_Player componentInChildren = other.GetComponentInChildren<Network_Player>();
		if (componentInChildren != null && componentInChildren.IsLocalPlayer)
		{
			playerCollider = other;
			localPlayer = componentInChildren;
			canCheckZoneTimer = 0.5f;
		}
		Physics.IgnoreCollision(terrainCollider, other, ignore: true);
	}

	private void OnTriggerExit(Collider other)
	{
		Physics.IgnoreCollision(terrainCollider, other, ignore: false);
	}

	private void OnEnable()
	{
		if (navmeshLink == null)
		{
			navmeshLink = GetComponentInChildren<NavMeshLink>();
		}
		if (navmeshLink != null)
		{
			Invoke("FixLink", 5f);
		}
	}

	private void FixLink()
	{
		navmeshLink.enabled = false;
		navmeshLink.enabled = true;
	}

	private bool HasNoTerrainCollider()
	{
		return terrainCollider == null;
	}
}
