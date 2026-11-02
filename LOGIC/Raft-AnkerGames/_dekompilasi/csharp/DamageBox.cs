using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
	public Action<Network_Player> OnDamage;

	private bool canDamage = true;

	[SerializeField]
	private Collider[] colliders;

	[SerializeField]
	protected int damage;

	[SerializeField]
	private int actualDamage;

	[SerializeField]
	private string entityTag = "Player";

	[SerializeField]
	private EntityType entityType = EntityType.Enemy;

	[SerializeField]
	private bool disableDamageOnDamageDealt = true;

	[SerializeField]
	private bool dealDamageInPeacefull = true;

	[SerializeField]
	private float resetCanDamageTime;

	[SerializeField]
	private ParticleSystem damageParticles;

	public EventReference er_damage;

	protected Raft_Network network;

	[SerializeField]
	private AI_NetworkBehaviourType aiTypeBase = AI_NetworkBehaviourType.None;

	public bool CanDamage
	{
		get
		{
			return canDamage;
		}
		set
		{
			canDamage = value;
		}
	}

	protected virtual void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		if (aiTypeBase != AI_NetworkBehaviourType.None)
		{
			EntityVariables difficultyEntityVariableFromAINetworkBehaviourType = GameModeValueManager.GetDifficultyEntityVariableFromAINetworkBehaviourType(aiTypeBase);
			if (difficultyEntityVariableFromAINetworkBehaviourType != null)
			{
				SetDamageAmount(Mathf.CeilToInt((float)damage * difficultyEntityVariableFromAINetworkBehaviourType.damageMultiplier));
			}
		}
		else if (aiTypeBase == AI_NetworkBehaviourType.None)
		{
			SetDamageAmount(damage);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		DamageEntity(other);
	}

	public void SetDamageAmount(int damage)
	{
		actualDamage = damage;
	}

	public void SetDamageBoxActive(bool value, bool checkCollisionImmediatly = false)
	{
		CanDamage = value;
		Collider[] array = colliders;
		foreach (Collider collider in array)
		{
			collider.enabled = value;
			if (!checkCollisionImmediatly || !CanDamage)
			{
				continue;
			}
			Collider[] collisions = Helper.GetCollisions(collider as BoxCollider, LayerMasks.MASK_Players);
			foreach (Collider col in collisions)
			{
				if (DamageEntity(col))
				{
					break;
				}
			}
		}
	}

	private bool DamageEntity(Collider col)
	{
		if (CanDamage && col.CompareTag(entityTag))
		{
			Network_Player component = col.GetComponent<Network_Player>();
			if (component != null)
			{
				return HandleDamagePlayer(component);
			}
		}
		return false;
	}

	protected virtual bool HandleDamagePlayer(Network_Player player)
	{
		if (player != null)
		{
			if (player.IsLocalPlayer && Time.time - player.PlayerScript.respawnTimeStamp > 5f)
			{
				if (disableDamageOnDamageDealt)
				{
					CanDamage = false;
					if (resetCanDamageTime > 0f)
					{
						CancelInvoke("ResetCanDamage");
						Invoke("ResetCanDamage", resetCanDamageTime);
					}
				}
				bool flag = true;
				if (!dealDamageInPeacefull && GameModeValueManager.GetCurrentGameModeValue().gameMode == GameMode.Peaceful)
				{
					flag = false;
				}
				if (flag)
				{
					network.hostNetwork.DamageEntity(player.Stats, null, actualDamage, Vector3.zero, Vector3.zero, entityType);
					if (player.Stats.IsDead)
					{
						AI_NetworkBehaviour componentInParent = GetComponentInParent<AI_NetworkBehaviour>();
						if (componentInParent != null)
						{
							AnalyticsHelper.CustomEventString(AnalyticsEvent.ANIMAL_KILLPLAYER, new Dictionary<string, object> { { "AnimalType", componentInParent.behaviourType } });
						}
					}
					RuntimeManager.PlayOneShot(er_damage, transform.position);
					if (damageParticles != null)
					{
						damageParticles.Play();
					}
				}
				OnDamage?.Invoke(player);
			}
			return true;
		}
		return false;
	}

	private void ResetCanDamage()
	{
		CanDamage = true;
	}
}
