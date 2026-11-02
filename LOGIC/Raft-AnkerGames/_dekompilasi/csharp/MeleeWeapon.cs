using FMODUnity;
using UnityEngine;

public class MeleeWeapon : UsableTool
{
	public LayerMask attackMask;

	public LayerMask obstacleMask = LayerMasks.MASK_Obstruction;

	[SerializeField]
	private float attackRange = 3f;

	[SerializeField]
	private int damage = 5;

	[SerializeField]
	private bool goThroughInvurnability;

	[SerializeField]
	private bool useSphereCast;

	[SerializeField]
	private float sphereRadius = 0.25f;

	public EventReference er_weaponSwing;

	private Network_Host hostNetwork;

	protected override void Start()
	{
		base.Start();
		hostNetwork = ComponentManager<Network_Host>.Value;
	}

	public virtual void OnMeleeStart()
	{
		RuntimeManager.PlayOneShotAttached(er_weaponSwing, gameObject);
	}

	public virtual void OnMeleeHit()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			OnMeleeWeaponSuccesfullRayHit(out var _);
		}
	}

	public virtual void OnMeleeWeaponSuccesfullRayHit(out RaycastHit hit)
	{
		bool flag = false;
		if (!useSphereCast)
		{
			if (Helper.HitAtCursor(out hit, attackRange, attackMask))
			{
				flag = true;
			}
		}
		else if (Helper.SphereHitAtCursor(out hit, attackRange, sphereRadius, attackMask))
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		Network_Entity_Redirect componentInParent = hit.collider.transform.GetComponentInParent<Network_Entity_Redirect>();
		if (componentInParent != null)
		{
			Network_Entity entity = componentInParent.entity;
			if (entity != null)
			{
				OnHitEntity(hit, entity);
			}
		}
	}

	public virtual void OnMeleeWeaponSuccesfullRayHitAll(out RaycastHit[] hit)
	{
		bool flag = false;
		if (!useSphereCast)
		{
			if (Helper.HitAllAtCursor(out hit, attackRange, attackMask))
			{
				flag = true;
			}
		}
		else if (Helper.SphereHitAllAtCursor(out hit, attackRange, sphereRadius, attackMask))
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		float num = float.MaxValue;
		RaycastHit hit2 = default;
		Network_Entity network_Entity = null;
		for (int i = 0; i < hit.Length; i++)
		{
			Network_Entity_Redirect componentInParent = hit[i].collider.transform.GetComponentInParent<Network_Entity_Redirect>();
			if (!(componentInParent != null))
			{
				continue;
			}
			Network_Entity entity = componentInParent.entity;
			if (entity != null && entity.gameObject != playerNetwork.gameObject)
			{
				float num2 = Vector3.Distance(transform.position, hit[i].collider.transform.position);
				if (num2 < num)
				{
					num = num2;
					hit2 = hit[i];
					network_Entity = entity;
				}
			}
		}
		if (network_Entity != null)
		{
			OnHitEntity(hit2, network_Entity);
		}
	}

	public virtual void OnHitEntity(RaycastHit hit, Network_Entity entity)
	{
		if (entity != null)
		{
			if (goThroughInvurnability && entity.IsInvurnerable)
			{
				entity.IsInvurnerable = false;
			}
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.DamageEnemy);
			hostNetwork.DamageEntity(entity, hit.transform, damage, hit.point, hit.normal, EntityType.Player);
			if (!entity.IsInvurnerable && entity.removesDurabilityWhenHit)
			{
				playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
			}
		}
	}
}
