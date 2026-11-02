using FMODUnity;
using UnityEngine;

public class Explosive : MonoBehaviour
{
	public ParticleSystem system;

	public ParticleSystem fuse;

	public ParticleSystem fire;

	public GameObject groundDecal;

	public GameObject groundCylinder;

	public GameObject fireParent;

	public GameObject groundParent;

	public float explosionRadius = 4f;

	public float damage = 15f;

	public float fireTime = 5f;

	public float fuseTime = 3f;

	public EventReference er_explode;

	private float timer;

	private bool hasLanded;

	private bool hasExploded;

	private int hapticIndex;

	public float Damage => damage * GameModeValueManager.GetCurrentGameModeValue().landmarkVariables.explosiveThrowerDamageMultiplier;

	private void Update()
	{
		if (hasLanded && !hasExploded)
		{
			timer += Time.deltaTime;
			float num = Mathf.Clamp01(timer / fuseTime);
			Vector3 localScale = fuse.transform.localScale;
			localScale.y = Mathf.Lerp(1f, 0f, num);
			fuse.transform.localScale = localScale;
			if (num >= 1f)
			{
				Explode();
			}
		}
	}

	public void OnLand()
	{
		groundCylinder.SetActiveSafe(value: true);
		hasLanded = true;
	}

	private void Explode()
	{
		hasExploded = true;
		system.transform.SetParentSafe(null);
		system.Play();
		RuntimeManager.PlayOneShot(er_explode, transform.position);
		hapticIndex = SimpleMonoBehaviourSingleton<HapticManager>.Instance.DynamitePool.FetchNextIndex();
		if (hapticIndex != -1)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.StartAttenuated((HapticEvent)(29 + hapticIndex), transform.position);
		}
		Raft_Network value = ComponentManager<Raft_Network>.Value;
		if (value != null && value.remoteUsers != null)
		{
			foreach (Network_Player value2 in value.remoteUsers.Values)
			{
				if (value2 == null)
				{
					return;
				}
				Vector3 position = value2.transform.position;
				if (position.y > transform.position.y && Vector3.Distance(transform.position, position) <= explosionRadius && !Physics.Linecast(transform.position + Vector3.up * 0.5f, position, LayerMasks.MASK_Obstruction))
				{
					value2.Stats.Damage(Damage, transform.position, Vector3.up, EntityType.Enemy, isLocal: false);
				}
			}
		}
		groundParent.SetActiveSafe(value: false);
		fireParent.SetActiveSafe(value: true);
		Invoke("RemoveFire", fireTime);
	}

	private void RemoveFire()
	{
		fire.transform.SetParentSafe(transform.parent);
		fire.Stop(withChildren: true);
		groundDecal.transform.SetParentSafe(transform.parent);
		Object.Destroy(groundDecal, fireTime / 2f);
		Object.Destroy(gameObject);
	}
}
