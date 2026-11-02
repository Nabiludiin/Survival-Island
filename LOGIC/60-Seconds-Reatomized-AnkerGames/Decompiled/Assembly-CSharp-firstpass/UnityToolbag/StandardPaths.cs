using System;
using System.IO;
using UnityEngine;

namespace UnityToolbag;

public static class StandardPaths
{
	private static class Windows
	{
		public static string GetPath(string subdirectory)
		{
			return AppendDirectory(Application.persistentDataPath, subdirectory);
		}
	}

	private static class OSX
	{
		public static string GetApplicationSupportPath(string subdirectory)
		{
			return AppendDirectory(Application.persistentDataPath, subdirectory);
		}

		public static string GetLogsPath()
		{
			return AppendProductPath(Path.Combine(Environment.GetEnvironmentVariable("HOME"), "Library/Logs"));
		}
	}

	private static class Linux
	{
		public static string GetSaveDirectory()
		{
			string text = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
			if (string.IsNullOrEmpty(text))
			{
				text = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".local/share");
			}
			return AppendProductPath(text);
		}

		public static string GetConfigDirectory()
		{
			string text = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
			if (string.IsNullOrEmpty(text))
			{
				text = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".config");
			}
			return AppendProductPath(text);
		}

		public static string GetLogDirectory()
		{
			return AppendProductPath("/var/log");
		}
	}

	public static bool includeCompanyName { get; set; }

	public static string gifsDataDirectory
	{
		get
		{
			string text;
			switch (Application.platform)
			{
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				text = Windows.GetPath("GIFs");
				break;
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				text = OSX.GetApplicationSupportPath("GIFs");
				break;
			case RuntimePlatform.LinuxPlayer:
				text = Linux.GetSaveDirectory();
				break;
			default:
				text = Application.persistentDataPath;
				break;
			}
			Directory.CreateDirectory(text);
			return text;
		}
	}

	public static string saveDataDirectory
	{
		get
		{
			string text;
			switch (Application.platform)
			{
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				text = Windows.GetPath("Saves");
				break;
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				text = OSX.GetApplicationSupportPath("Saves");
				break;
			case RuntimePlatform.LinuxPlayer:
				text = Linux.GetSaveDirectory();
				break;
			default:
				text = Application.persistentDataPath;
				break;
			}
			Directory.CreateDirectory(text);
			return text;
		}
	}

	public static string configDirectory
	{
		get
		{
			string text;
			switch (Application.platform)
			{
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				text = Windows.GetPath("Config");
				break;
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				text = OSX.GetApplicationSupportPath("Config");
				break;
			case RuntimePlatform.LinuxPlayer:
				text = Linux.GetConfigDirectory();
				break;
			default:
				text = Application.persistentDataPath;
				break;
			}
			Directory.CreateDirectory(text);
			return text;
		}
	}

	public static string logDirectory
	{
		get
		{
			string text;
			switch (Application.platform)
			{
			case RuntimePlatform.WindowsPlayer:
			case RuntimePlatform.WindowsEditor:
				text = Windows.GetPath("Logs");
				break;
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				text = OSX.GetLogsPath();
				break;
			case RuntimePlatform.LinuxPlayer:
				text = Linux.GetLogDirectory();
				break;
			default:
				text = Application.persistentDataPath;
				break;
			}
			Directory.CreateDirectory(text);
			return text;
		}
	}

	static StandardPaths()
	{
		includeCompanyName = true;
	}

	private static string AppendProductPath(string path)
	{
		if (includeCompanyName)
		{
			path = AppendDirectory(path, Application.companyName);
		}
		return AppendDirectory(path, Application.productName);
	}

	private static string AppendDirectory(string path, string dir)
	{
		if (string.IsNullOrEmpty(dir))
		{
			return path;
		}
		dir = CleanForPath(dir);
		return Path.Combine(path, dir);
	}

	private static string CleanForPath(string str)
	{
		char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
		foreach (char oldChar in invalidFileNameChars)
		{
			str = str.Replace(oldChar, '_');
		}
		return str;
	}
}
