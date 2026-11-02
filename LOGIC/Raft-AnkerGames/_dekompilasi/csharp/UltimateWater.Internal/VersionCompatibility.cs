using System.Text.RegularExpressions;
using UnityEngine;

namespace UltimateWater.Internal;

public class VersionCompatibility
{
	private static int _Version;

	private static readonly string[] _Unsupported;

	private static readonly string[] _Bugged;

	private static readonly string[] _BugInfo;

	public static int Version
	{
		get
		{
			if (_Version == -1)
			{
				_Version = CalculateVersion();
			}
			return _Version;
		}
	}

	static VersionCompatibility()
	{
		_Version = -1;
		_Unsupported = new string[27]
		{
			"5.0.0", "5.0.1", "5.0.2", "5.0.3", "5.0.4", "5.1.0", "5.1.1", "5.1.2", "5.1.3", "5.1.4",
			"5.1.5", "5.2.0", "5.2.1", "5.2.2", "5.2.3", "5.2.4", "5.2.5", "5.3.0", "5.3.1", "5.3.2",
			"5.3.3", "5.3.4", "5.3.5", "5.4.0", "5.4.1", "5.4.2", "5.4.4"
		};
		_Bugged = new string[2] { "5.6.0", "5.6.1" };
		_BugInfo = new string[2] { "This Unity version introduces bugs in depth computations, please use earlier (5.5.x-) or later (5.6.2+) versions", "This Unity version introduces bugs in depth computations, please use earlier (5.5.x-) or later (5.6.2+) versions" };
		CalculateVersion();
		CheckVersion();
	}

	private static int CalculateVersion()
	{
		string[] array = Regex.Replace(Application.unityVersion, "[^0-9.]+[0-9]*", string.Empty).Split('.');
		int num = int.Parse(array[0]);
		int num2 = int.Parse(array[1]);
		int num3 = int.Parse(array[2]);
		return num * 100 + num2 * 10 + num3;
	}

	private static void CheckVersion()
	{
	}

	private static bool IsSupported(string version)
	{
		string[] unsupported = _Unsupported;
		for (int i = 0; i < unsupported.Length; i++)
		{
			if (unsupported[i].Contains(version))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsBuggy(string version)
	{
		string[] bugged = _Bugged;
		for (int i = 0; i < bugged.Length; i++)
		{
			if (bugged[i].Contains(version))
			{
				return true;
			}
		}
		return false;
	}

	private static string BugInfo(string version)
	{
		for (int i = 0; i < _Bugged.Length; i++)
		{
			if (_Bugged[i].Contains(version))
			{
				return _BugInfo[i];
			}
		}
		return string.Empty;
	}
}
