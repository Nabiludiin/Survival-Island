using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_NetworkBehaviour_Chicken : AI_NetworkBehaviour_Domestic
{
	private static ObjectManager _objectManager;

	[SerializeField]
	private GameObject eggPrefab;

	[SerializeField]
	private Transform dropEggPoint;

	[SerializeField]
	private Interval_Float eggDropInterval;

	[SerializeField]
	private AI_State_Wait layEggState;

	[SerializeField]
	private float _eggTimer;

	public EventReference er_layEgg;

	public float EggTimer
	{
		get
		{
			return _eggTimer;
		}
		set
		{
			_eggTimer = value;
		}
	}

	private ObjectManager ObjectManager
	{
		get
		{
			if (_objectManager == null)
			{
				_objectManager = ComponentManager<ObjectManager>.Value;
			}
			return _objectManager;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (Raft_Network.IsHost)
		{
			EggTimer = eggDropInterval.GetRandomValue();
		}
	}

	protected override void UpdateHost()
	{
		base.UpdateHost();
		if (!networkEntity.IsDead && IsDomestic && !IsStarving && DomesticState == DomesticStateType.Raft && !carryScript.IsBeingCarried && !captureScript.IsCaptured)
		{
			EggTimer -= Time.deltaTime;
			if (EggTimer <= 0f)
			{
				EggTimer = eggDropInterval.GetRandomValue();
				stateMachine_Domestic.animController.SetTrigger("LayEgg");
				stateMachine_Domestic.ChangeState(layEggState);
			}
		}
	}

	public void Animation_LayEgg()
	{
		if (Raft_Network.IsHost)
		{
			LayEggNetworked();
		}
	}

	public void Animation_LayEggFootstep()
	{
		if (stateMachine_Domestic.eventRef_footStep != null)
		{
			stateMachine_Domestic.eventRef_footStep.Play();
			stateMachine_Domestic.eventRef_footStep.SetParameter("speed", 1f);
		}
	}

	private void LayEggNetworked()
	{
		if (Raft_Network.IsHost)
		{
			Vector3 localRaftPosition = GetChickensLocalRaftPosition() + Vector3.up * 0.25f;
			uint uniqueObjectIndex = SaveAndLoad.GetUniqueObjectIndex();
			LayEgg(localRaftPosition, uniqueObjectIndex);
			Network.RPC(new Message_Chicken_LayEgg(Messages.Chicken_LayEgg, Network.NetworkIDManager, ObjectIndex, localRaftPosition, uniqueObjectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void LayEgg(Vector3 localRaftPosition, uint eggObjectIndex)
	{
		if (ObjectManager != null)
		{
			ObjectManager.Object_Chicken.LayEgg(localRaftPosition, eggObjectIndex);
			RuntimeManager.PlayOneShot(er_layEgg, transform.position);
		}
		else
		{
			Debug.LogError("ObjectManager was null in AI_NetworkBehaviour_Chicken.cs -> LayEgg()");
		}
	}

	private Vector3 GetChickensLocalRaftPosition()
	{
		return lockedPivot.InverseTransformPoint(dropEggPoint.position);
	}

	public override RGD Serialize_Save()
	{
		if (connectedSpawner == null && !networkEntity.IsDead)
		{
			return new RGD_Domestic_Chicken(this);
		}
		return null;
	}

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		if (connectedSpawner != null && !networkEntity.IsDead)
		{
			return new RGD_Domestic_Chicken(this);
		}
		return null;
	}

	public override void OnRestoreFinished(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		base.OnRestoreFinished(rgdNetworkBehaviour);
		RGD_Domestic_Chicken rGD_Domestic_Chicken = rgdNetworkBehaviour as RGD_Domestic_Chicken;
		if (rGD_Domestic_Chicken != null)
		{
			rGD_Domestic_Chicken.RestoreDomesticBehaviour(this);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Chicken_LayEgg)
		{
			Message_Chicken_LayEgg message_Chicken_LayEgg = msg as Message_Chicken_LayEgg;
			LayEgg(message_Chicken_LayEgg.localRaftPosition.Value, message_Chicken_LayEgg.eggObjectIndex);
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
