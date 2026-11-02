using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace AssetBundles.Manager;

public class ServerSetting : ScriptableObject
{
	private static string s_localServerURL;

	[SerializeField]
	private string m_serverURL;

	[SerializeField]
	private string m_localAssetBundleDirectory;

	[SerializeField]
	private string m_manifestFileName;

	[SerializeField]
	private ServerSettingType m_serverType;

	[SerializeField]
	private bool m_withPlatformSubDir;

	public string Name => name;

	public string ServerURL
	{
		get
		{
			string text = string.Empty;
			switch (m_serverType)
			{
			case ServerSettingType.Local:
				text = GetLocalServerURL();
				break;
			case ServerSettingType.Remote:
				text = m_serverURL;
				break;
			case ServerSettingType.StreamingAssets:
				text = GetStreamingAssetsURL(m_serverURL);
				break;
			}
			if (m_withPlatformSubDir)
			{
				return $"{text}{Utility.GetPlatformName()}/";
			}
			return text;
		}
	}

	public string AssetBundleDirectory => m_localAssetBundleDirectory;

	public string ManifestFileName
	{
		get
		{
			if (m_withPlatformSubDir)
			{
				return Utility.GetPlatformName();
			}
			return m_manifestFileName;
		}
	}

	public ServerSettingType ServerType
	{
		get
		{
			return m_serverType;
		}
		set
		{
			m_serverType = value;
		}
	}

	public bool UsePlatformSubDir => m_withPlatformSubDir;

	public static ServerSetting CreateServerSetting(string name, ServerSettingType t)
	{
		ServerSetting serverSetting = ScriptableObject.CreateInstance<ServerSetting>();
		serverSetting.name = name;
		serverSetting.m_serverType = t;
		serverSetting.m_withPlatformSubDir = false;
		serverSetting.m_localAssetBundleDirectory = string.Empty;
		serverSetting.m_serverURL = string.Empty;
		return serverSetting;
	}

	private static string GetLocalServerURL()
	{
		if (s_localServerURL == null)
		{
			string text = "";
			string hostName = Dns.GetHostName();
			IPHostEntry hostEntry;
			try
			{
				hostEntry = Dns.GetHostEntry(hostName);
			}
			catch (SocketException)
			{
				hostEntry = Dns.GetHostEntry("127.0.0.1");
			}
			IPAddress[] addressList = hostEntry.AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					text = iPAddress.ToString();
					break;
				}
			}
			s_localServerURL = "http://" + text + ":7888/";
		}
		return s_localServerURL;
	}

	private static string GetStreamingAssetsURL(string str)
	{
		return $"file://{Application.streamingAssetsPath}/{str}";
	}
}
