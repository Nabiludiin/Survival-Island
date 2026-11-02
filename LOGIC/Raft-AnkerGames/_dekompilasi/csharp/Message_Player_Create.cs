using System;
using Steamworks;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_Player_Create : Message_NetworkBehaviour
{
	[SerializeField]
	private ulong steamID;

	public RGD_Settings_Character characterSettings;

	public bool dead;

	public bool noteBookOpen;

	public float hazmatTimer;

	[SerializeField]
	private bool aui;

	public CSteamID SteamID
	{
		get
		{
			return new CSteamID(steamID);
		}
		set
		{
			steamID = value.m_SteamID;
		}
	}

	public Network_UserId UserId => steamID;

	public bool Anim_UsingInstrument
	{
		get
		{
			return aui;
		}
		set
		{
			aui = value;
		}
	}

	protected bool Equals(Message_Player_Create other)
	{
		if (Equals((Message_NetworkBehaviour)other) && steamID == other.steamID && object.Equals(characterSettings, other.characterSettings) && dead == other.dead && noteBookOpen == other.noteBookOpen && hazmatTimer.Equals(other.hazmatTimer))
		{
			return aui == other.aui;
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
		return Equals((Message_Player_Create)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), steamID, characterSettings, dead, noteBookOpen, hazmatTimer, aui);
	}

	public static bool operator ==(Message_Player_Create left, Message_Player_Create right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_Player_Create left, Message_Player_Create right)
	{
		return !object.Equals(left, right);
	}

	public Message_Player_Create(Messages type, Network_Player player)
		: base(type, player)
	{
		steamID = player.steamID;
		characterSettings = player.characterSettings;
		dead = player.PlayerScript.IsDead;
		hazmatTimer = (player.HazmatSuit.Equipped ? player.HazmatSuit.Timer : (-1f));
		if (player.NoteBookUI != null)
		{
			noteBookOpen = player.NoteBookUI.isDisplayed;
		}
		Anim_UsingInstrument = player.Animator.IsInFullBodyAnimation(PlayerFullBodyAnimation.UsingInstrument_3);
	}

	public Message_Player_Create()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in steamID, default(FastBufferWriter.ForPrimitives));
		characterSettings.SerializeFast(writer);
		writer.WriteValueSafe(in dead, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in noteBookOpen, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in hazmatTimer, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in aui, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out steamID, default(FastBufferWriter.ForPrimitives));
		characterSettings = new RGD_Settings_Character();
		characterSettings.DeserializeFast(reader);
		reader.ReadValue(out dead, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out noteBookOpen, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out hazmatTimer, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out aui, default(FastBufferWriter.ForPrimitives));
	}
}
