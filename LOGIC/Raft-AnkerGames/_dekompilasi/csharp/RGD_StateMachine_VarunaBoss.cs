using System;
using System.Runtime.Serialization;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RGD_StateMachine_VarunaBoss : RGD_StateMachine
{
	[SerializeField]
	private ObjectOfInterestType currentCirculationID;

	[SerializeField]
	private ObjectOfInterestType confirmedCirculationID;

	protected bool Equals(RGD_StateMachine_VarunaBoss other)
	{
		if (Equals((RGD_StateMachine)other) && currentCirculationID == other.currentCirculationID)
		{
			return confirmedCirculationID == other.confirmedCirculationID;
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
		return Equals((RGD_StateMachine_VarunaBoss)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), (int)currentCirculationID, (int)confirmedCirculationID);
	}

	public static bool operator ==(RGD_StateMachine_VarunaBoss left, RGD_StateMachine_VarunaBoss right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_StateMachine_VarunaBoss left, RGD_StateMachine_VarunaBoss right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in currentCirculationID, default(FastBufferWriter.ForEnums));
		writer.WriteValueSafe(in confirmedCirculationID, default(FastBufferWriter.ForEnums));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out currentCirculationID, default(FastBufferWriter.ForEnums));
		reader.ReadValue(out confirmedCirculationID, default(FastBufferWriter.ForEnums));
	}

	public RGD_StateMachine_VarunaBoss()
	{
	}

	public RGD_StateMachine_VarunaBoss(AI_StateMachine_Boss_Varuna stateMachine)
		: base(stateMachine)
	{
		currentCirculationID = stateMachine.waypointCirculation.currentCirculationID;
		confirmedCirculationID = stateMachine.waypointCirculation.confirmedCirculationID;
	}

	public override void Restore(AI_StateMachine stateMachine)
	{
		base.Restore(stateMachine);
		AI_StateMachine_Boss_Varuna obj = stateMachine as AI_StateMachine_Boss_Varuna;
		obj.waypointCirculation.UpdateCirculationID(currentCirculationID);
		obj.waypointCirculation.confirmedCirculationID = confirmedCirculationID;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("CurrentCirculationID", (int)currentCirculationID);
		info.AddValue("ConfirmedCirculationID", (int)confirmedCirculationID);
	}

	public RGD_StateMachine_VarunaBoss(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "CurrentCirculationID"))
			{
				if (name == "ConfirmedCirculationID")
				{
					confirmedCirculationID = (ObjectOfInterestType)item.Value;
				}
			}
			else
			{
				currentCirculationID = (ObjectOfInterestType)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		currentCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_1;
		confirmedCirculationID = ObjectOfInterestType.Varuna_Boss_Floor_1;
	}
}
