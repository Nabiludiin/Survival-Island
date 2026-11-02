using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.RemoteConfig;
using UnityEngine;

public class RemoteConfigManager : MonoBehaviour
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct userAttributes
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct appAttributes
	{
	}

	public static List<Network_UserId> validDevIds = new List<Network_UserId>();

	public static RemoteConfigManager instance;

	public static bool hasRecievedCallBack = false;

	private static Raft_Network network;

	private static bool initialized = false;

	private void Awake()
	{
		if (initialized)
		{
			Object.Destroy(this);
			return;
		}
		instance = this;
		initialized = true;
		ConfigManager.FetchConfigs<userAttributes, appAttributes>(default, default);
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		ConfigManager.FetchCompleted += ApplyUserStatus;
	}

	private void OnDestroy()
	{
		ConfigManager.FetchCompleted -= ApplyUserStatus;
	}

	public static bool ExternalLocalUserIsDevCheck(Network_UserId id)
	{
		ConfigManager.FetchConfigs<userAttributes, appAttributes>(default, default);
		return CheckIfUserIsDev(id);
	}

	public static bool CheckIfUserIsDev(Network_UserId id)
	{
		string text = ConfigManager.appConfig.GetString(id.ToString());
		bool result = false;
		if (text != string.Empty)
		{
			result = true;
			if (!validDevIds.Contains(id))
			{
				validDevIds.Add(id);
			}
		}
		return result;
	}

	private static void ApplyUserStatus(ConfigResponse response)
	{
		switch (response.requestOrigin)
		{
		case ConfigOrigin.Remote:
			hasRecievedCallBack = true;
			break;
		case ConfigOrigin.Default:
		case ConfigOrigin.Cached:
			break;
		}
	}
}
