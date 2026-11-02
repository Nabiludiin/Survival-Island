using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_AI_NetworkBehaviour : RGD
{
	public AI_NetworkBehaviourType networkBehaviourType;

	public uint aiObjectIndex;

	public uint aiEntityObjectIndex;

	public SerializableVector3 position;

	public SerializableVector3 euler;

	public RGD_StateMachine rgdStateMachine;

	public float scaleSize;

	public int health;

	public bool parentedToRaft;

	public bool gameobjectActive;

	public int pickupYieldCount;

	protected bool Equals(RGD_AI_NetworkBehaviour other)
	{
		if (networkBehaviourType == other.networkBehaviourType && aiObjectIndex == other.aiObjectIndex && aiEntityObjectIndex == other.aiEntityObjectIndex && object.Equals(position, other.position) && object.Equals(euler, other.euler) && object.Equals(rgdStateMachine, other.rgdStateMachine) && scaleSize.Equals(other.scaleSize) && health == other.health && parentedToRaft == other.parentedToRaft && gameobjectActive == other.gameobjectActive)
		{
			return pickupYieldCount == other.pickupYieldCount;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((RGD_AI_NetworkBehaviour)obj);
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default;
		hashCode.Add((int)networkBehaviourType);
		hashCode.Add(aiObjectIndex);
		hashCode.Add(aiEntityObjectIndex);
		hashCode.Add(position);
		hashCode.Add(euler);
		hashCode.Add(rgdStateMachine);
		hashCode.Add(scaleSize);
		hashCode.Add(health);
		hashCode.Add(parentedToRaft);
		hashCode.Add(gameobjectActive);
		hashCode.Add(pickupYieldCount);
		return hashCode.ToHashCode();
	}

	public static bool operator ==(RGD_AI_NetworkBehaviour left, RGD_AI_NetworkBehaviour right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_AI_NetworkBehaviour left, RGD_AI_NetworkBehaviour right)
	{
		return !object.Equals(left, right);
	}

	public RGD_AI_NetworkBehaviour()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		if (!writer.TryBeginWrite(50))
		{
			throw new OverflowException("Not enough space in the buffer");
		}
		writer.WriteValue(in networkBehaviourType, default(FastBufferWriter.ForEnums));
		writer.WriteValue(in aiObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in aiEntityObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in position.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in euler.x, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in euler.y, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in euler.z, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in scaleSize, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in health, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in parentedToRaft, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in gameobjectActive, default(FastBufferWriter.ForPrimitives));
		writer.WriteValue(in pickupYieldCount, default(FastBufferWriter.ForPrimitives));
		if (rgdStateMachine is RGD_StateMachine_VarunaBoss)
		{
			writer.WriteValueSafe<AiStateMachine>(AiStateMachine.Varuna, default(FastBufferWriter.ForEnums));
		}
		else if (rgdStateMachine is RGD_StateMachine_HyenaBoss)
		{
			writer.WriteValueSafe<AiStateMachine>(AiStateMachine.Hyena, default(FastBufferWriter.ForEnums));
		}
		else
		{
			writer.WriteValueSafe<AiStateMachine>(AiStateMachine.Normal, default(FastBufferWriter.ForEnums));
		}
		rgdStateMachine.SerializeFast(writer);
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out networkBehaviourType, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out aiObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out aiEntityObjectIndex, default(FastBufferWriter.ForPrimitives));
		position = new SerializableVector3();
		reader.ReadValue(out position.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out position.z, default(FastBufferWriter.ForPrimitives));
		euler = new SerializableVector3();
		reader.ReadValue(out euler.x, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out euler.y, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out euler.z, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out scaleSize, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out health, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out parentedToRaft, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out gameobjectActive, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out pickupYieldCount, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out AiStateMachine value, default(FastBufferWriter.ForEnums));
		switch (value)
		{
		case AiStateMachine.Normal:
			rgdStateMachine = new RGD_StateMachine();
			break;
		case AiStateMachine.Varuna:
			rgdStateMachine = new RGD_StateMachine_VarunaBoss();
			break;
		case AiStateMachine.Hyena:
			rgdStateMachine = new RGD_StateMachine_HyenaBoss();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		rgdStateMachine.DeserializeFast(reader);
	}

	public RGD_AI_NetworkBehaviour(AI_NetworkBehaviour networkBehaviour)
		: base(RGDType.AI_NetworkBehaviour, RGDSortingOrder.Entities)
	{
		networkBehaviourType = networkBehaviour.behaviourType;
		aiObjectIndex = networkBehaviour.ObjectIndex;
		aiEntityObjectIndex = networkBehaviour.networkEntity.ObjectIndex;
		parentedToRaft = networkBehaviour.transform.ParentedToRaft();
		position = new SerializableVector3(parentedToRaft ? networkBehaviour.transform.localPosition : networkBehaviour.transform.position);
		euler = new SerializableVector3(networkBehaviour.transform.eulerAngles);
		health = (int)networkBehaviour.networkEntity.stat_health.Value;
		gameobjectActive = networkBehaviour.gameObject.activeSelf;
		scaleSize = networkBehaviour.scaledSize;
		pickupYieldCount = ((!(networkBehaviour.pickupItemChanneling == null)) ? networkBehaviour.pickupItemChanneling.ItemsRemaining : 0);
		AI_StateMachine component = networkBehaviour.GetComponent<AI_StateMachine>();
		if (component != null)
		{
			rgdStateMachine = component.GetStateMachineData();
		}
	}

	public void RestoreAINetworkBehaviour(AI_NetworkBehaviour networkBehaviour)
	{
		networkBehaviour.OnRestoreStart(this);
		if (parentedToRaft)
		{
			networkBehaviour.transform.SetParentSafe(SingletonGeneric<GameManager>.Singleton.lockedPivot);
			networkBehaviour.transform.localPosition = position.Value;
		}
		else
		{
			networkBehaviour.transform.position = position.Value;
		}
		networkBehaviour.scaledSize = scaleSize;
		networkBehaviour.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
		networkBehaviour.transform.eulerAngles = euler.Value;
		if (health != -1)
		{
			networkBehaviour.networkEntity.stat_health.Value = health;
		}
		AI_StateMachine component = networkBehaviour.GetComponent<AI_StateMachine>();
		if (rgdStateMachine != null && component != null)
		{
			rgdStateMachine.Restore(component);
		}
		networkBehaviour.hasBeenLoaded = true;
		networkBehaviour.OnRestoreFinished(this);
		if (networkBehaviour.pickupItemChanneling != null)
		{
			networkBehaviour.pickupItemChanneling.RestoreYield(pickupYieldCount);
		}
		if (!gameobjectActive)
		{
			networkBehaviour.gameObject.SetActive(value: false);
		}
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("NetworkBehaviourType", networkBehaviourType);
		info.AddValue("AIObjectIndex", aiObjectIndex);
		info.AddValue("AIEntityObjectIndex", aiEntityObjectIndex);
		info.AddValue("Position", position);
		info.AddValue("Euler", euler);
		info.AddValue("Health", health);
		info.AddValue("StateMachine", rgdStateMachine);
		info.AddValue("ParentedToRaft", parentedToRaft);
		info.AddValue("GameObjectActive", gameobjectActive);
		info.AddValue("PickupYieldCount", pickupYieldCount);
		info.AddValue("ScaleSize", scaleSize);
	}

	public RGD_AI_NetworkBehaviour(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "NetworkBehaviourType":
				networkBehaviourType = Helper.ObjToValue<AI_NetworkBehaviourType>(item.Value);
				break;
			case "AIObjectIndex":
				aiObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "AIEntityObjectIndex":
				aiEntityObjectIndex = Helper.ObjToValue<uint>(item.Value);
				break;
			case "Position":
				position = Helper.ObjToValue<SerializableVector3>(item.Value);
				break;
			case "Euler":
				euler = Helper.ObjToValue<SerializableVector3>(item.Value);
				break;
			case "Health":
				health = Helper.ObjToValue<int>(item.Value);
				break;
			case "StateMachine":
				rgdStateMachine = (RGD_StateMachine)item.Value;
				break;
			case "ParentedToRaft":
				parentedToRaft = Helper.ObjToValue<bool>(item.Value);
				break;
			case "GameObjectActive":
				gameobjectActive = Helper.ObjToValue<bool>(item.Value);
				break;
			case "PickupYieldCount":
				pickupYieldCount = Helper.ObjToValue<int>(item.Value);
				break;
			case "ScaleSize":
				scaleSize = Helper.ObjToValue<float>(item.Value);
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		networkBehaviourType = AI_NetworkBehaviourType.None;
		aiObjectIndex = 0u;
		aiEntityObjectIndex = 0u;
		position = new SerializableVector3();
		euler = new SerializableVector3();
		health = -1;
		rgdStateMachine = null;
		parentedToRaft = false;
		gameobjectActive = true;
		pickupYieldCount = 0;
		scaleSize = 1f;
	}
}
