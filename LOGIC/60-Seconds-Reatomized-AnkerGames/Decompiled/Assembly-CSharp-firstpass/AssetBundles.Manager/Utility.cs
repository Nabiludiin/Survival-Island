using UnityEngine;

namespace AssetBundles.Manager;

public class Utility
{
	public static string GetPlatformName()
	{
		return GetPlatformForAssetBundles(Application.platform);
	}

	private static string GetPlatformForAssetBundles(RuntimePlatform platform)
	{
		return platform switch
		{
			RuntimePlatform.Android => "Android", 
			RuntimePlatform.IPhonePlayer => "iOS", 
			RuntimePlatform.WebGLPlayer => "WebGL", 
			RuntimePlatform.WindowsPlayer => "Windows", 
			RuntimePlatform.OSXPlayer => "OSX", 
			_ => null, 
		};
	}
}
