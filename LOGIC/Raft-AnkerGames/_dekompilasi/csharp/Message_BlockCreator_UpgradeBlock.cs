using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Message_BlockCreator_UpgradeBlock : Message_BlockCreator_PlaceBlock
{
	[SerializeField]
	private ulong playerUpgradingSteamID;

	public uint removeBlockObjectIndex;

	public int upgradeItemIndex;

	public Network_UserId PlayerUpgradingSteamID => playerUpgradingSteamID;

	protected bool Equals(Message_BlockCreator_UpgradeBlock other)
	{
		if (Equals((Message_BlockCreator_PlaceBlock)other) && playerUpgradingSteamID == other.playerUpgradingSteamID && removeBlockObjectIndex == other.removeBlockObjectIndex)
		{
			return upgradeItemIndex == other.upgradeItemIndex;
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
		return Equals((Message_BlockCreator_UpgradeBlock)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), playerUpgradingSteamID, removeBlockObjectIndex, upgradeItemIndex);
	}

	public static bool operator ==(Message_BlockCreator_UpgradeBlock left, Message_BlockCreator_UpgradeBlock right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Message_BlockCreator_UpgradeBlock left, Message_BlockCreator_UpgradeBlock right)
	{
		return !object.Equals(left, right);
	}

	public Message_BlockCreator_UpgradeBlock(Messages type, MonoBehaviour_Network behaviour, int buildableUniqueIndex, uint blockObjectIndex, uint networkedObjectIndex, uint networkedBehaviourIndex, Vector3 localPosition, Vector3 localEuler, int hotSlotIndex, DPS dpsType, uint removeBlockObjectIndex, int upgradeItemIndex, ulong playerUpgradingSteamID)
		: base(type, behaviour, buildableUniqueIndex, blockObjectIndex, networkedObjectIndex, networkedBehaviourIndex, localPosition, localEuler, hotSlotIndex, dpsType)
	{
		this.removeBlockObjectIndex = removeBlockObjectIndex;
		this.upgradeItemIndex = upgradeItemIndex;
		this.playerUpgradingSteamID = playerUpgradingSteamID;
	}

	public Message_BlockCreator_UpgradeBlock()
	{
	}

	public override void SerializeFast(FastBufferWriter writer)
	{
		base.SerializeFast(writer);
		writer.WriteValueSafe(in playerUpgradingSteamID, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in removeBlockObjectIndex, default(FastBufferWriter.ForPrimitives));
		writer.WriteValueSafe(in upgradeItemIndex, default(FastBufferWriter.ForPrimitives));
	}

	public override void DeserializeFast(FastBufferReader reader)
	{
		base.DeserializeFast(reader);
		reader.ReadValue(out playerUpgradingSteamID, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out removeBlockObjectIndex, default(FastBufferWriter.ForPrimitives));
		reader.ReadValue(out upgradeItemIndex, default(FastBufferWriter.ForPrimitives));
	}
}
