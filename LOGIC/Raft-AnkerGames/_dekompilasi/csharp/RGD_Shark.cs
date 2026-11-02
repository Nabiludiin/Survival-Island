using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RGD_Shark : RGD
{
	public SerializableVector3 Position;

	public SerializableVector3 ForwardDirection;

	public uint objectIndex;

	public uint behaviourIndex;

	public uint entityObjectIndex;

	public int targetBlockIndex;

	public bool bitingRaft;

	public SharkState state;

	public int health;

	public bool parentedToRaft;

	public int yieldCount;

	public RGD_Shark(RGDType type, Shark shark)
		: base(type, RGDSortingOrder.Entities)
	{
		objectIndex = shark.ObjectIndex;
		behaviourIndex = shark.BehaviourIndex;
		entityObjectIndex = shark.entityComponent.ObjectIndex;
		parentedToRaft = shark.transform.parent != null;
		if (parentedToRaft)
		{
			Position = new SerializableVector3(shark.transform.localPosition);
		}
		else
		{
			Position = new SerializableVector3(shark.transform.position);
		}
		ForwardDirection = new SerializableVector3(shark.transform.forward);
		bitingRaft = shark.bitingRaft;
		state = shark.state;
		health = (int)shark.entityComponent.stat_health.Value;
		targetBlockIndex = ((shark.targetBlock == null) ? (-1) : ((int)shark.targetBlock.ObjectIndex));
		yieldCount = shark.yield.Count;
	}

	public void RestoreShark(Shark shark)
	{
		if (parentedToRaft)
		{
			shark.transform.SetParent(shark.globalBuoyancyParent);
			shark.transform.localPosition = Position.Value;
		}
		else
		{
			shark.transform.position = Position.Value;
		}
		shark.transform.rotation = Quaternion.LookRotation(ForwardDirection.Value);
		if (health != -1)
		{
			shark.entityComponent.stat_health.Value = health;
		}
		shark.sharkMaterial.SetFloat("_SharkHP", shark.entityComponent.stat_health.NormalValue);
		if (state == SharkState.DeadSink || state == SharkState.Dead || state == SharkState.AttackRaft)
		{
			shark.ChangeState(state);
		}
		if (bitingRaft && targetBlockIndex >= 0)
		{
			shark.targetBlock = BlockCreator.GetBlockByObjectIndex((uint)targetBlockIndex);
		}
		if (yieldCount != -1)
		{
			while (shark.yield.Count > yieldCount)
			{
				shark.RemoveYield();
			}
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Position", Position);
		info.AddValue("ForwardDirection", ForwardDirection);
		info.AddValue("ParentedToRaft", parentedToRaft);
		info.AddValue("ObjectIndex", objectIndex);
		info.AddValue("BehaviourIndex", behaviourIndex);
		info.AddValue("EntityObjectIndex", entityObjectIndex);
		info.AddValue("TargetBlockIndex", targetBlockIndex);
		info.AddValue("BitingRaft", bitingRaft);
		info.AddValue("State", state);
		info.AddValue("Health", health);
		info.AddValue("YieldCount", yieldCount);
	}

	public RGD_Shark(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Position":
				Position = (SerializableVector3)item.Value;
				break;
			case "ForwardDirection":
				ForwardDirection = (SerializableVector3)item.Value;
				break;
			case "ParentedToRaft":
				parentedToRaft = (bool)item.Value;
				break;
			case "ObjectIndex":
				objectIndex = (uint)item.Value;
				break;
			case "BehaviourIndex":
				behaviourIndex = (uint)item.Value;
				break;
			case "EntityObjectIndex":
				entityObjectIndex = (uint)item.Value;
				break;
			case "TargetBlockIndex":
				targetBlockIndex = (int)item.Value;
				break;
			case "BitingRaft":
				bitingRaft = (bool)item.Value;
				break;
			case "State":
				state = (SharkState)item.Value;
				break;
			case "Health":
				health = (int)item.Value;
				break;
			case "YieldCount":
				yieldCount = (int)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		Position = new SerializableVector3();
		ForwardDirection = new SerializableVector3();
		parentedToRaft = false;
		objectIndex = 0u;
		behaviourIndex = 0u;
		entityObjectIndex = 0u;
		targetBlockIndex = -1;
		bitingRaft = false;
		state = SharkState.PassiveSurface;
		health = -1;
		yieldCount = -1;
	}
}
