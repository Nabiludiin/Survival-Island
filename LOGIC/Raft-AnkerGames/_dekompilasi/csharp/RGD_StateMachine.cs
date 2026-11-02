using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.Netcode;

[Serializable]
public class RGD_StateMachine : SerializableBehaviour
{
	public RGD_AI_State[] rgdStates;

	public int currentStateIndex;

	protected bool Equals(RGD_StateMachine other)
	{
		if (rgdStates == null && other.rgdStates != null)
		{
			return false;
		}
		if (rgdStates != null && other.rgdStates == null)
		{
			return false;
		}
		if ((rgdStates == null && other.rgdStates == null) || Enumerable.SequenceEqual(rgdStates, other.rgdStates))
		{
			return currentStateIndex == other.currentStateIndex;
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
		return Equals((RGD_StateMachine)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(rgdStates, currentStateIndex);
	}

	public static bool operator ==(RGD_StateMachine left, RGD_StateMachine right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(RGD_StateMachine left, RGD_StateMachine right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>((rgdStates != null) ? rgdStates.Length : 0, default(FastBufferWriter.ForPrimitives));
		if (rgdStates != null)
		{
			RGD_AI_State[] array = rgdStates;
			foreach (RGD_AI_State rGD_AI_State in array)
			{
				if (!(rGD_AI_State is RGD_AI_Revive))
				{
					if (!(rGD_AI_State is RGD_Shark_State_BiteRaft))
					{
						if (!(rGD_AI_State is RGD_Stonebird_DropStone))
						{
							if (!(rGD_AI_State is RGD_Stonebird_GrabStone))
							{
								if ((object)rGD_AI_State == null)
								{
									throw new ArgumentOutOfRangeException("rgdAiState");
								}
								writer.WriteValueSafe<AiStateType>(AiStateType.Normal, default(FastBufferWriter.ForEnums));
							}
							else
							{
								writer.WriteValueSafe<AiStateType>(AiStateType.GrabStone, default(FastBufferWriter.ForEnums));
							}
						}
						else
						{
							writer.WriteValueSafe<AiStateType>(AiStateType.DropStone, default(FastBufferWriter.ForEnums));
						}
					}
					else
					{
						writer.WriteValueSafe<AiStateType>(AiStateType.BiteRaft, default(FastBufferWriter.ForEnums));
					}
				}
				else
				{
					writer.WriteValueSafe<AiStateType>(AiStateType.Revive, default(FastBufferWriter.ForEnums));
				}
				rGD_AI_State.SerializeFast(writer);
			}
		}
		writer.WriteValueSafe(in currentStateIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		if (value > 0)
		{
			rgdStates = new RGD_AI_State[value];
			for (int i = 0; i < value; i++)
			{
				reader.ReadValue(out AiStateType value2, default(FastBufferWriter.ForEnums));
				switch (value2)
				{
				case AiStateType.Revive:
					rgdStates[i] = new RGD_AI_Revive();
					break;
				case AiStateType.BiteRaft:
					rgdStates[i] = new RGD_Shark_State_BiteRaft();
					break;
				case AiStateType.DropStone:
					rgdStates[i] = new RGD_Stonebird_DropStone();
					break;
				case AiStateType.GrabStone:
					rgdStates[i] = new RGD_Stonebird_GrabStone();
					break;
				case AiStateType.Normal:
					rgdStates[i] = new RGD_AI_State();
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				rgdStates[i].DeserializeFast(reader);
			}
		}
		reader.ReadValue(out currentStateIndex, default(FastBufferWriter.ForPrimitives));
	}

	public RGD_StateMachine()
	{
	}

	public RGD_StateMachine(AI_StateMachine stateMachine)
	{
		if (stateMachine == null)
		{
			return;
		}
		currentStateIndex = ((stateMachine.currentState == null) ? (-1) : ((int)stateMachine.currentState.stateIndex));
		AI_State[] componentsInChildren = stateMachine.GetComponentsInChildren<AI_State>(includeInactive: true);
		List<RGD_AI_State> list = new List<RGD_AI_State>();
		foreach (AI_State aI_State in componentsInChildren)
		{
			if (!(aI_State == null) && aI_State.SaveState() != null)
			{
				list.Add(aI_State.SaveState());
			}
		}
		if (list.Count > 0)
		{
			rgdStates = list.ToArray();
		}
	}

	public virtual void Restore(AI_StateMachine stateMachine)
	{
		AI_State[] componentsInChildren = stateMachine.GetComponentsInChildren<AI_State>(includeInactive: true);
		if (componentsInChildren == null)
		{
			return;
		}
		if (currentStateIndex != -1)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null && componentsInChildren[i].stateIndex == currentStateIndex)
				{
					stateMachine.ChangeState(componentsInChildren[i]);
					break;
				}
			}
		}
		if (rgdStates != null && rgdStates.Length != 0)
		{
			for (int j = 0; j < rgdStates.Length; j++)
			{
				RGD_AI_State rGD_AI_State = rgdStates[j];
				if (rGD_AI_State == null)
				{
					continue;
				}
				foreach (AI_State aI_State in componentsInChildren)
				{
					if (!(aI_State == null) && rGD_AI_State.stateIndex == aI_State.stateIndex)
					{
						rGD_AI_State.Restore(aI_State);
						break;
					}
				}
			}
		}
		stateMachine.OnStateMachineRestored();
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("RGDStates", rgdStates);
		info.AddValue("CurrentStateIndex", currentStateIndex);
	}

	public RGD_StateMachine(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			string name = item.Name;
			if (!(name == "RGDStates"))
			{
				if (name == "CurrentStateIndex")
				{
					currentStateIndex = Helper.ObjToValue<int>(item.Value);
				}
			}
			else
			{
				rgdStates = (RGD_AI_State[])item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		rgdStates = null;
		currentStateIndex = -1;
	}
}
