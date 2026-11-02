using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class PufferFishParticleCloud : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem cloudParticles;

	[SerializeField]
	private float cloudRadius = 1f;

	[SerializeField]
	private float cloudLifeTime = 10f;

	[SerializeField]
	private SO_Buff_DOT buff_poison;

	[SerializeField]
	private float tickRate = 1f;

	[SerializeField]
	private float tickTimer;

	[SerializeField]
	private StudioEventEmitter cloudEmitter;

	private Network_Host network_Host;

	private Network_Player localPlayer;

	private void Start()
	{
		network_Host = ComponentManager<Network_Host>.Value;
	}

	private void OnEnable()
	{
		StartCoroutine(RemoveCloud());
	}

	private void Update()
	{
		if (Raft_Network.IsHost)
		{
			tickTimer += Time.deltaTime;
			if (tickTimer >= tickRate)
			{
				tickTimer = 0f;
				Tick();
			}
		}
		HandlePoisonSound();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawSphere(transform.position, cloudRadius);
	}

	private void HandlePoisonSound()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
			return;
		}
		float num = Vector3.Distance(transform.position, localPlayer.transform.position);
		if (num > cloudRadius)
		{
			cloudEmitter.SetParameter("damage", 0f);
		}
		else
		{
			cloudEmitter.SetParameter("damage", 1f - Helper.MapValue(0f, cloudRadius, num, 0f, 1f));
		}
	}

	private IEnumerator RemoveCloud()
	{
		yield return new WaitForSeconds(cloudLifeTime);
		if (cloudParticles != null)
		{
			cloudParticles.Stop();
			while (cloudParticles.IsAlive())
			{
				yield return null;
			}
		}
		Object.Destroy(gameObject);
	}

	private void Tick()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		List<Network_Player> list = Helper.PlayersWithinRange(transform.position, cloudRadius, includeDeadPlayers: false);
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Network_Player network_Player = list[i];
			if (!(network_Player == null) && !network_Player.PlayerScript.IsDead)
			{
				network_Host.DamageEntity(network_Player.Stats, null, 0f, network_Player.transform.position, Vector3.up, EntityType.Enemy, buff_poison);
				if (network_Player.Stats.IsDead)
				{
					AnalyticsHelper.CustomEventString(AnalyticsEvent.ANIMAL_KILLPLAYER, new Dictionary<string, object> { 
					{
						"AnimalType",
						AI_NetworkBehaviourType.PufferFish
					} });
				}
			}
		}
	}
}
