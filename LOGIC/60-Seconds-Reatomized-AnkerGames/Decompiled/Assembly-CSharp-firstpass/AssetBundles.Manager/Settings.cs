using System.Collections.Generic;
using UnityEngine;

namespace AssetBundles.Manager;

public class Settings : ScriptableObject
{
	public class Path
	{
		public static string SettingsFileName => "AssetBundleManagerSettings";
	}

	public enum AssetBundleManagerMode
	{
		SimulationMode,
		SimulationModeGraphTool,
		Server
	}

	[SerializeField]
	private List<ServerSetting> m_settings;

	[SerializeField]
	private ServerSetting m_currentSetting;

	[SerializeField]
	private ServerSetting m_devBuildSetting;

	[SerializeField]
	private ServerSetting m_releaseBuildSetting;

	[SerializeField]
	private ServerSetting m_streamingAssetSetting;

	[SerializeField]
	private AssetBundleManagerMode m_mode;

	[SerializeField]
	private AssetMap m_assetMap;

	[SerializeField]
	private bool m_clearCacheOnPlay;

	private static Settings s_settings;

	public static ServerSetting CurrentSetting => ReleaseBuildSetting;

	public static ServerSetting DevelopmentBuildSetting => GetSettings().m_devBuildSetting;

	public static ServerSetting ReleaseBuildSetting => GetSettings().m_releaseBuildSetting;

	public static ServerSetting StreamingAssetsSetting => GetSettings().m_streamingAssetSetting;

	public static AssetBundleManagerMode Mode => GetSettings().m_mode;

	public static AssetMap Map => GetSettings().m_assetMap;

	private static Settings GetSettings()
	{
		if (s_settings == null && !Load())
		{
			s_settings = ScriptableObject.CreateInstance<Settings>();
			s_settings.m_settings = new List<ServerSetting>();
		}
		return s_settings;
	}

	private static bool Load()
	{
		s_settings = Resources.Load(Path.SettingsFileName) as Settings;
		return true;
	}
}
