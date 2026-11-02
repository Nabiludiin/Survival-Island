using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class StoneDrop : MonoBehaviour
{
	public Rigidbody rigidBody;

	public AI_NetworkBehaviourType aiType;

	[SerializeField]
	private LayerMask collisionMask;

	[SerializeField]
	private LayerMask obstuctionMask;

	[SerializeField]
	private float damage = 20f;

	[SerializeField]
	private float aoeRange = 2f;

	[SerializeField]
	private float minYDistanceToDmg = 1.5f;

	public EventReference er_hitGround;

	public EventReference er_hitPlayer;

	[SerializeField]
	private StudioEventEmitter eventEmitter_fallSound;

	private Network_Host network_Host;

	private bool hasProccedAoe;

	public float Damage
	{
		get
		{
			EntityVariables difficultyEntityVariableFromAINetworkBehaviourType = GameModeValueManager.GetDifficultyEntityVariableFromAINetworkBehaviourType(aiType);
			if (difficultyEntityVariableFromAINetworkBehaviourType == null)
			{
				return damage;
			}
			return damage * difficultyEntityVariableFromAINetworkBehaviourType.damageMultiplier;
		}
	}

	private void Start()
	{
		network_Host = ComponentManager<Network_Host>.Value;
	}

	private void Update()
	{
		if (eventEmitter_fallSound != null && rigidBody != null)
		{
			float num = 15f;
			float value = Mathf.Clamp(rigidBody.velocity.y, float.MinValue, 0f);
			value = value.Remap(0f - num, 0f, 0f, num);
			eventEmitter_fallSound.SetParameter("velocity", num - value);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!hasProccedAoe && Helper.ObjectIsOnLayer(collision.gameObject, collisionMask))
		{
			Network_Player componentInParent = collision.gameObject.GetComponentInParent<Network_Player>();
			hasProccedAoe = true;
			OnStoneImpact(componentInParent != null);
		}
	}

	public void OnStoneImpact(bool hitPlayersHead)
	{
		if (hitPlayersHead)
		{
			AudioHelper.PlayOneShotWithParameters(er_hitPlayer, transform.position, ("velocty", 20f));
			ParticleManager.PlaySystem("Vulture_StoneBreakHead", transform.position);
		}
		else
		{
			RuntimeManager.PlayOneShot(er_hitGround, transform.position);
			ParticleManager.PlaySystem("Vulture_StoneBreak", transform.position);
		}
		DamageAOE(transform.position);
		Object.Destroy(gameObject);
	}

	private void DamageAOE(Vector3 centerPoint)
	{
		List<Network_Player> list = Helper.PlayersWithinRange(transform.position, aoeRange, includeDeadPlayers: false);
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (Mathf.Abs(list[i].transform.position.y - centerPoint.y) <= minYDistanceToDmg)
			{
				DamagePlayer(list[i], null);
			}
			else if (Helper.HasClearVision(centerPoint + Vector3.up * 0.3f, list[i].transform.position + Vector3.up, obstuctionMask))
			{
				DamagePlayer(list[i], null);
			}
		}
	}

	private void DamagePlayer(Network_Player player, Transform hitTransform)
	{
		if (!player.IsLocalPlayer)
		{
			return;
		}
		network_Host.DamageEntity(player.Stats, hitTransform, Damage, transform.position, Vector3.up, EntityType.Enemy);
		if (player.Stats.IsDead)
		{
			AI_NetworkBehaviour componentInParent = GetComponentInParent<AI_NetworkBehaviour>();
			if (componentInParent != null)
			{
				AnalyticsHelper.CustomEventString(AnalyticsEvent.ANIMAL_KILLPLAYER, new Dictionary<string, object> { { "AnimalType", componentInParent.behaviourType } });
			}
		}
	}
}
