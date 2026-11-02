using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class AI_NetworkBehaviour_Animal : AI_NetworkBehaviour
{
	[SerializeField]
	private float positionLerpSpeed = 1f;

	[SerializeField]
	protected float rotationLerpSpeed = 1f;

	[HideInInspector]
	public Vector3 networkPosition;

	protected Vector3 networkEuler;

	protected AI_StateMachine_Animal stateMachineAnimal;

	protected override void Awake()
	{
		base.Awake();
		stateMachineAnimal = GetComponent<AI_StateMachine_Animal>();
		networkPosition = stateMachineAnimal.transform.position;
		networkEuler = stateMachineAnimal.transform.eulerAngles;
	}

	protected override void UpdateRemoteclient()
	{
		base.UpdateRemoteclient();
		LerpMovement(networkPosition, localPosition: false);
		LerpRotation(networkEuler);
	}

	public override void Initialize(Vector3 spawnPosition, float scaleSize)
	{
		base.Initialize(spawnPosition, scaleSize);
		networkPosition = spawnPosition;
		if (stateMachineAnimal.animController != null && scaleSize != 1f)
		{
			stateMachineAnimal.animController.SetFloat("VelocityModifier", Mathf.Clamp(1f / scaleSize, 0.1f, 10f));
		}
	}

	protected virtual void LerpMovement(Vector3 position, bool localPosition)
	{
		if (localPosition)
		{
			stateMachineAnimal.transform.localPosition = Vector3.Lerp(stateMachineAnimal.transform.localPosition, position, Time.deltaTime * positionLerpSpeed);
		}
		else
		{
			stateMachineAnimal.transform.position = Vector3.Lerp(stateMachineAnimal.transform.position, position, Time.deltaTime * positionLerpSpeed);
		}
	}

	protected virtual void LerpRotation(Vector3 eulerAngles)
	{
		if (networkEntity.IsDead || stateMachineAnimal.movement.model == null)
		{
			stateMachineAnimal.transform.rotation = Quaternion.Lerp(stateMachineAnimal.transform.rotation, Quaternion.Euler(networkEuler), Time.deltaTime * rotationLerpSpeed);
		}
		else
		{
			stateMachineAnimal.movement.model.rotation = Quaternion.Lerp(stateMachineAnimal.movement.model.rotation, Quaternion.Euler(networkEuler), Time.deltaTime * rotationLerpSpeed);
		}
	}

	public override RGD Serialize_Save()
	{
		if (!ConnectedToSpawner && !networkEntity.IsDead)
		{
			return new RGD_AI_NetworkBehaviour(this);
		}
		return null;
	}

	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		if (ConnectedToSpawner && !networkEntity.IsDead)
		{
			return new RGD_AI_NetworkBehaviour(this);
		}
		return null;
	}

	public override void Serialize_CreateFromIDManager(List<Message_NetworkBehaviour> msgs)
	{
		base.Serialize_CreateFromIDManager(msgs);
		if (!ConnectedToSpawner)
		{
			msgs.Add(new Message_CreateAINetworkBehaviour(Messages.CreateAINetworkBehaviour, Network.NetworkIDManager, ComponentManager<Network_Host_Entities>.Value.ObjectIndex, stateMachineAnimal.transform.position, this));
		}
	}

	public override void Serialize_Update()
	{
		base.Serialize_Update();
		if (Raft_Network.IsHost)
		{
			Serialize_Update_Animal();
			CheckSendAnimationTriggers();
		}
	}

	public virtual void Serialize_Update_Animal()
	{
		AddMessage(new Message_Animal_Update(Messages.AINetworkBehaviourUpdate_Animal, network.NetworkIDManager, ObjectIndex, stateMachineAnimal));
	}

	public void CheckSendAnimationTriggers()
	{
		string[] array = null;
		if (stateMachineAnimal.animController != null)
		{
			array = stateMachineAnimal.animController.GetTriggers();
		}
		if (array != null && array.Length != 0)
		{
			Message_Animal_AnimTriggers message = new Message_Animal_AnimTriggers(Messages.AINetworkAnimatorUpdate, network.NetworkIDManager, ObjectIndex, array);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.AINetworkBehaviourUpdate_Shark:
		case Messages.AINetworkBehaviourUpdate_Animal:
		case Messages.AINetworkBehaviourUpdate_Domestic:
		case Messages.AINetworkBehaviourUpdate_NPC:
		case Messages.AINetworkBehaviourUpdate_PufferFish:
		case Messages.AINetworkBehaviourUpdate_StoneBird:
		{
			Message_Animal_Update message_Animal_Update = msg as Message_Animal_Update;
			networkPosition = message_Animal_Update.Position;
			networkEuler = message_Animal_Update.Euler;
			message_Animal_Update.OnDeserialize(stateMachineAnimal);
			return false;
		}
		case Messages.AINetworkAnimatorUpdate:
		{
			string[] anim_triggers = (msg as Message_Animal_AnimTriggers).anim_triggers;
			if (anim_triggers != null)
			{
				for (int i = 0; i < anim_triggers.Length; i++)
				{
					stateMachineAnimal.animController.SetTrigger(anim_triggers[i]);
				}
			}
			return false;
		}
		case Messages.SetIDBehaviourDead:
			if ((msg as Message_SetIDBehaviourDead).removeInstantly || stateMachineAnimal.decayState == null)
			{
				Object.Destroy(stateMachineAnimal.gameObject);
			}
			else
			{
				stateMachineAnimal.ChangeState(stateMachineAnimal.decayState);
			}
			return true;
		case Messages.PickupChannel:
			if (pickupItemChanneling == null)
			{
				Debug.Log("Pickup channeling was null, probably something wrong on " + transform.name);
				return false;
			}
			return pickupItemChanneling.HandleNetworkMessageDeserialze(msg as Message_PickupChannel);
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
