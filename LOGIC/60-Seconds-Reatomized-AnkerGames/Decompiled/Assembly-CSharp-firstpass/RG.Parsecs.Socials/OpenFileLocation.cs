using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace RG.Parsecs.Socials;

public static class OpenFileLocation
{
	public static bool IsInMacOS => SystemInfo.operatingSystem.IndexOf("Mac OS") != -1;

	public static bool IsInWinOS => SystemInfo.operatingSystem.IndexOf("Windows") != -1;

	public static void OpenInMac(string path)
	{
		bool flag = false;
		string text = path.Replace("\\", "/");
		if (Directory.Exists(text))
		{
			flag = true;
		}
		if (!text.StartsWith("\""))
		{
			text = "\"" + text;
		}
		if (!text.EndsWith("\""))
		{
			text += "\"";
		}
		string arguments = (flag ? "" : "-R ") + text;
		try
		{
			Process.Start("open", arguments);
		}
		catch (Win32Exception ex)
		{
			ex.HelpLink = "";
		}
	}

	public static void OpenInWin(string path)
	{
		bool flag = false;
		string text = path.Replace("/", "\\");
		if (Directory.Exists(text))
		{
			flag = true;
		}
		try
		{
			Process.Start("explorer.exe", (flag ? "/root," : "/select,") + text);
		}
		catch (Win32Exception ex)
		{
			ex.HelpLink = "";
		}
	}

	public static void Open(string path)
	{
		if (IsInWinOS)
		{
			OpenInWin(path);
			return;
		}
		if (IsInMacOS)
		{
			OpenInMac(path);
			return;
		}
		OpenInWin(path);
		OpenInMac(path);
	}
}
