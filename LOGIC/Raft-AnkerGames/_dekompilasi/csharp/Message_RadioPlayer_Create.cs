using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_RadioPlayer_Create : Message_NetworkBehaviour
{
	[SerializeField]
	private RGD_RadioPlayer[] rgdRadioPlayers;

	protected bool Equals(Message_RadioPlayer_Create other)
	{
		if (rgdRadioPlayers == null && other.rgdRadioPlayers != null)
		{
			return false;
		}
		if (rgdRadioPlayers != null && other.rgdRadioPlayers == null)
		{
			return false;
		}
		if (Equals((Message_NetworkBehaviour)other))
		{
			if (rgdRadioPlayers != null || other.rgdRadioPlayers != null)
			{
				return Enumerable.SequenceEqual(rgdRadioPlayers, other.rgdRadioPlayers);
			}
			return true;
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
		return Equals((Message_RadioPlayer_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), rgdRadioPlayers);
	}

	public static bool operator ==(Message_RadioPlayer_Create left, Message_RadioPlayer_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_RadioPlayer_Create left, Message_RadioPlayer_Create right)
	{
		return !object.Equals(left, right);
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe<int>(rgdRadioPlayers.Length, default(FastBufferWriter.ForPrimitives));
		RGD_RadioPlayer[] array = rgdRadioPlayers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SerializeFast(writer);
		}
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		int value = 0;
		reader.ReadValue(out value, default(FastBufferWriter.ForPrimitives));
		rgdRadioPlayers = new RGD_RadioPlayer[value];
		for (int i = 0; i < value; i++)
		{
			rgdRadioPlayers[i] = new RGD_RadioPlayer();
			rgdRadioPlayers[i].DeserializeFast(reader);
		}
	}

	public Message_RadioPlayer_Create()
	{
	}

	public Message_RadioPlayer_Create(Messages type, MonoBehaviour_Network behaviour, RadioPlayer[] radioPlayers)
		: base(type, behaviour)
	{
		List<RGD_RadioPlayer> list = new List<RGD_RadioPlayer>();
		for (int i = 0; i < radioPlayers.Length; i++)
		{
			if (!(radioPlayers[i] == null))
			{
				RGD_RadioPlayer rGDRadioPlayer = radioPlayers[i].GetRGDRadioPlayer();
				list.Add(rGDRadioPlayer);
			}
		}
		rgdRadioPlayers = list.ToArray();
	}

	public void RestoreRadios(RadioPlayer[] radioPlayers, NetworkIDManager networkIDManager)
	{
		RGD_RadioPlayer[] array = rgdRadioPlayers;
		foreach (RGD_RadioPlayer rGD_RadioPlayer in array)
		{
			if (rGD_RadioPlayer == null)
			{
				continue;
			}
			foreach (RadioPlayer radioPlayer in radioPlayers)
			{
				if (!(radioPlayer == null) && rGD_RadioPlayer.objectIndex == radioPlayer.ObjectIndex)
				{
					radioPlayer.RestoreRadio(rGD_RadioPlayer);
					break;
				}
			}
		}
	}
}
