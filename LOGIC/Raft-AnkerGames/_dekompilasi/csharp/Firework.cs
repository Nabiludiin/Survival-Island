using FMODUnity;
using Steamworks;
using UnityEngine;

public class Firework : MonoBehaviour_ID_Network
{
	[SerializeField]
	public Interval_Float lifeTimeInterval;

	[HideInInspector]
	public float lifeTime = 1f;

	[SerializeField]
	private float initialForce = 5f;

	[SerializeField]
	private float constantForce = 5f;

	[SerializeField]
	private SO_ColorValue defaultColor;

	[SerializeField]
	private Collider fireworkCollider;

	[SerializeField]
	private ParticleSystem[] particleSystemPrefabs;

	[SerializeField]
	private Transform smokeTrail;

	[SerializeField]
	private float damage = 20f;

	private SO_ColorValue currentColor;

	public EventReference er_explode;

	private Rigidbody body;

	private bool exploded;

	private Raft_Network network;

	private void Awake()
	{
		InitializeCollider();
		body = GetComponent<Rigidbody>();
		body.AddForce(transform.forward * initialForce, ForceMode.VelocityChange);
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
	}

	private void Update()
	{
		if (Raft_Network.IsHost)
		{
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0f && !exploded)
			{
				ExplodeRPC();
			}
		}
		body.AddForce(transform.forward * constantForce, ForceMode.Acceleration);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (Raft_Network.IsHost)
		{
			TryToDamageEntity(collision.transform);
			ExplodeRPC();
		}
	}

	public void SetColor(int uniqueColorIndex)
	{
		SO_ColorValue sO_ColorValue = null;
		if (uniqueColorIndex >= 0)
		{
			sO_ColorValue = ColorPicker.GetColorFromUniqueIndex((uint)uniqueColorIndex);
		}
		if (sO_ColorValue == null)
		{
			sO_ColorValue = defaultColor;
		}
		currentColor = sO_ColorValue;
	}

	private void ExplodeRPC()
	{
		if (Raft_Network.IsHost)
		{
			Message_Firework_Explode message_Firework_Explode = new Message_Firework_Explode(Messages.Firework_Explode, network.NetworkIDManager, ObjectIndex, transform.position, Random.Range(0, particleSystemPrefabs.Length));
			network.RPC(message_Firework_Explode, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			Explode(message_Firework_Explode.Position, message_Firework_Explode.particlePrefabIndex);
		}
	}

	private void Explode(Vector3 position, int particlePrefabIndex)
	{
		if (particlePrefabIndex < 0 || particlePrefabIndex >= particleSystemPrefabs.Length)
		{
			particlePrefabIndex = 0;
		}
		exploded = true;
		ParticleSystem particleSystem = Object.Instantiate(particleSystemPrefabs[particlePrefabIndex], position, Quaternion.identity, null);
		ParticleSystem.MainModule main = particleSystem.main;
		main.startColor = currentColor.buttonColor;
		particleSystem.Play(withChildren: true);
		if (smokeTrail != null)
		{
			smokeTrail.SetParentSafe(null);
			if (smokeTrail != null && smokeTrail.gameObject != null)
			{
				Object.Destroy(smokeTrail.gameObject, 6f);
			}
		}
		RuntimeManager.PlayOneShot(er_explode, transform.position);
		NetworkIDManager.RemoveNetworkID(this, typeof(Firework));
		AchievementHandler.AddPlayerFireworksLaunched();
		Object.Destroy(gameObject);
	}

	private void TryToDamageEntity(Transform transform)
	{
		Network_Entity_Redirect component = transform.GetComponent<Network_Entity_Redirect>();
		if (component != null && component.entity != null)
		{
			Network_Host value = ComponentManager<Network_Host>.Value;
			if (value != null)
			{
				value.DamageEntity(component.entity, transform, damage, transform.position, Vector3.up, EntityType.Environment);
			}
		}
	}

	private void InitializeCollider()
	{
		fireworkCollider.enabled = false;
		if (Raft_Network.IsHost)
		{
			Invoke("EnableCollider", 0.1f);
		}
	}

	private void EnableCollider()
	{
		fireworkCollider.enabled = Raft_Network.IsHost;
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Firework_Explode)
		{
			Message_Firework_Explode message_Firework_Explode = msg as Message_Firework_Explode;
			Explode(message_Firework_Explode.Position, message_Firework_Explode.particlePrefabIndex);
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
