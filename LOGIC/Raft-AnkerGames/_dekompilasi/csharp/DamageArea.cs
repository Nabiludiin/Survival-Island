using FMODUnity;
using UnityEngine;

public class DamageArea : AreaZone_PlayerCounter
{
	[SerializeField]
	private int damage = 10;

	[SerializeField]
	private SO_Buff debuff;

	[SerializeField]
	private EntityType damageType;

	[SerializeField]
	[EventRef]
	private string eventRef_DamageSound;

	public EventReference er_DamageSound;

	protected Network_Host network_Host;

	private SoundManager soundManager;

	protected override void Start()
	{
		base.Start();
		soundManager = ComponentManager<SoundManager>.Value;
		if (!Raft_Network.IsHost)
		{
			isConsideredDestroyed = true;
		}
		else
		{
			network_Host = ComponentManager<Network_Host>.Value;
		}
	}

	protected virtual void DamageEntitiesInArea()
	{
		if (!IsConsideredActive)
		{
			return;
		}
		for (int i = 0; i < playersInArea.Count; i++)
		{
			Network_Player network_Player = playersInArea[i];
			if (network_Player == null)
			{
				playersInArea.RemoveAt(i);
				i--;
			}
			else
			{
				DamageEntity(network_Player);
			}
		}
	}

	protected virtual void DamageEntity(Network_Player player)
	{
		if (IsConsideredActive && player != null && !player.PlayerScript.IsDead)
		{
			float num = (float)damage * GameModeValueManager.GetCurrentGameModeValue().specialEffectsVariables.damageAreaMultiplier;
			network_Host.DamageEntity(player.Stats, null, num, player.transform.position, Vector3.up, damageType, debuff);
			if (soundManager != null)
			{
				soundManager.PlayOneShotNetworked(er_DamageSound, player.transform.position);
			}
		}
	}

	public virtual void ClearEntitiesFromArea()
	{
		playersInArea.Clear();
	}

	protected override void OnAddNewPlayer(Network_Player player)
	{
		base.OnAddNewPlayer(player);
		DamageEntity(player);
	}
}
