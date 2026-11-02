using FMODUnity;
using RG.Parsecs.Survival;
using Rewired;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class ActorBrain : MonoBehaviour, IActorBrain, IHaveEventHub<ActorBrain, ActorEventsArgs, ActorEvents>
{
	public PlayerSounds PlayerSounds;

	public EventHub<ActorBrain, ActorEventsArgs, ActorEvents> ActorEventsHub = new DictionaryEventHub<ActorBrain, ActorEventsArgs, ActorEvents>(default(ActorEventsComparer));

	public Character Character;

	[SerializeField]
	private float _speed;

	[Tooltip("Exploration time seconds")]
	[SerializeField]
	private int _intelligence;

	[SerializeField]
	private int _handsCount = 4;

	public Sprite handSprite;

	public virtual EventHub<ActorBrain, ActorEventsArgs, ActorEvents> EventHub => ActorEventsHub;

	public Transform Transform { get; set; }

	public GameObject GameObject { get; set; }

	public string Name { get; set; }

	public Transform PositionMarker { get; set; }

	public float Speed => _speed;

	public int HandsCount => _handsCount;

	public int Intelligence => _intelligence;

	public Player Player { get; set; }

	internal void SendEvent(ActorEvents e)
	{
		ActorEventsHub.Invoke(this, new ActorEventsArgs(e));
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == 12)
		{
			PlaySound(PlayerSounds.Collision);
			SendEvent(ActorEvents.FurnitureCollision);
		}
	}

	public void PlaySound(string sound)
	{
		if (!string.IsNullOrEmpty(sound))
		{
			RuntimeManager.PlayOneShot(sound);
		}
	}

	public void InitAttributes()
	{
		CharacterAttributeValue attributeById = Character.StaticData.GetAttributeById("attr_int");
		_intelligence = (int)attributeById.Attribute.GetAttributeLevel(attributeById.Value).ScavengeValue;
		CharacterAttributeValue attributeById2 = Character.StaticData.GetAttributeById("attr_str");
		_handsCount = (int)attributeById2.Attribute.GetAttributeLevel(attributeById2.Value).ScavengeValue;
		CharacterAttributeValue attributeById3 = Character.StaticData.GetAttributeById("attr_agl");
		_speed = attributeById3.Attribute.GetAttributeLevel(attributeById3.Value).ScavengeValue;
	}

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		GlobalActorEventsHub.I.Subscribe(this);
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	public virtual void ResetActor()
	{
	}
}
