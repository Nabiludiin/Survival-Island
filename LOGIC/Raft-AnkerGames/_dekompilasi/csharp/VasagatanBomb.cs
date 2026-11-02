using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class VasagatanBomb : MonoBehaviour
{
	[SerializeField]
	private Vector3 explosionOffset = Vector3.zero;

	[SerializeField]
	private int explosionDamage = 10;

	[SerializeField]
	private float explosionRadius = 1f;

	[SerializeField]
	private float cameraShakeRadius = 1f;

	[SerializeField]
	private SO_CameraShaker cameraShakeSettings;

	public EventReference er_explosion;

	private ScreenEffectManager screenEffectManager;

	public Vector3 ExplosionPosition => transform.position + transform.forward * explosionOffset.z + transform.up * explosionOffset.y + transform.right * explosionOffset.x;

	private void Awake()
	{
		screenEffectManager = ComponentManager<ScreenEffectManager>.Value;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(ExplosionPosition, 0.5f);
		Gizmos.DrawWireSphere(ExplosionPosition, explosionRadius);
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(ExplosionPosition, cameraShakeRadius);
	}

	public void Animation_Explosion()
	{
		Network_Host value = ComponentManager<Network_Host>.Value;
		if (Raft_Network.IsHost && value != null)
		{
			List<Network_Player> list = Helper.PlayersWithinRange(ExplosionPosition, explosionRadius, includeDeadPlayers: false);
			if (list.ContainsItems())
			{
				foreach (Network_Player item in list)
				{
					if (Helper.HasVisionToPlayer(item, ExplosionPosition, useFeetPosition: false))
					{
						value.DamageEntity(item.Stats, null, explosionDamage, item.transform.position, Vector3.up, EntityType.None);
					}
				}
			}
		}
		if (screenEffectManager != null && cameraShakeSettings != null && Helper.LocalPlayerIsWithinDistance(ExplosionPosition, cameraShakeRadius))
		{
			screenEffectManager.ShakeCamera(cameraShakeSettings);
		}
		RuntimeManager.PlayOneShot(er_explosion, ExplosionPosition);
	}
}
