using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Steamworks;

[Serializable]
public class RGD_Users : RGD
{
	public List<RGD_User> users = new List<RGD_User>();

	public RGD_Users()
		: base(RGDType.Users, RGDSortingOrder.First)
	{
	}

	public void AddUser(Guid worldGuid, Network_Player player)
	{
		if (users == null)
		{
			users = new List<RGD_User>();
		}
		RGD_User user = GetUser(worldGuid, SteamUser.GetSteamID().m_SteamID);
		if (user == null)
		{
			user = new RGD_User(worldGuid.ToString(), player);
			users.Add(user);
		}
		else
		{
			user.ReSave(player);
		}
	}

	public RGD_User GetUser(Guid worldGuid)
	{
		for (int i = 0; i < users.Count; i++)
		{
			RGD_User rGD_User = users[i];
			if (rGD_User.worldGuid == worldGuid.ToString())
			{
				return rGD_User;
			}
		}
		return null;
	}

	public RGD_User GetUser(Guid worldGuid, ulong steamID)
	{
		string text = worldGuid.ToString();
		for (int i = 0; i < users.Count; i++)
		{
			RGD_User rGD_User = users[i];
			if (rGD_User.steamID == steamID && rGD_User.worldGuid == text)
			{
				return rGD_User;
			}
		}
		return null;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Users", users);
	}

	public RGD_Users(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			if (item.Name == "Users")
			{
				users = (List<RGD_User>)item.Value;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		users = null;
	}
}
