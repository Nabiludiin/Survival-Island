using System;
using System.Runtime.Serialization;
using Steamworks;

[Serializable]
public class RGD_User : SerializableBehaviour
{
	public ulong steamID;

	public string worldGuid;

	public RGD_Player rgdPlayer;

	public RGD_User(string worldGuid, Network_Player player)
	{
		steamID = SteamUser.GetSteamID().m_SteamID;
		this.worldGuid = worldGuid;
		rgdPlayer = new RGD_Player(player);
	}

	public void ReSave(Network_Player player)
	{
		rgdPlayer = new RGD_Player(player);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("SteamID", steamID);
		info.AddValue("WorldGuid", worldGuid);
		info.AddValue("Player", rgdPlayer);
	}

	public RGD_User(SerializationInfo info, StreamingContext sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "SteamID":
				steamID = (ulong)item.Value;
				break;
			case "WorldGuid":
				worldGuid = (string)item.Value;
				break;
			case "Player":
				rgdPlayer = (RGD_Player)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		steamID = 0uL;
		worldGuid = "Default Guid";
		rgdPlayer = null;
	}
}
