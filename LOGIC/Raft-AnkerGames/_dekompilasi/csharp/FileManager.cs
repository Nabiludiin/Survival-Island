using System;
using System.IO;
using UnityEngine;

public class FileManager : MonoBehaviour
{
	public static Action<Exception, string, string> OnFileSystemError;

	public static string URLfileAccessError = "https://support.redbeetinteractive.com/hc/en-us/articles/360013656220-Version-X-XXXXXXX-and-Stuck-in-Character-Selection-Menu";

	public static string URLDirectoryNotFound = "https://support.redbeetinteractive.com/hc/en-us/requests/new";

	public static string URLFileNotFoundException = "https://support.redbeetinteractive.com/hc/en-us/requests/new";

	public static string URLIOException = "https://support.redbeetinteractive.com/hc/en-us/articles/360016876040-World-is-still-on-the-load-world-menu-after-deleting-and-has-red-background-";

	public static FileStream OpenFile(string path, FileMode fileMode)
	{
		FileStream result = null;
		try
		{
			result = File.Open(path, fileMode);
		}
		catch (Exception ex)
		{
			HandleSystemError(ex);
			Helper.LogBuild(ex.Message);
		}
		return result;
	}

	public static FileStream CreateFile(string path)
	{
		FileStream result = null;
		try
		{
			result = File.Create(path);
		}
		catch (Exception ex)
		{
			Helper.LogBuild(ex.Message);
			HandleSystemError(ex);
		}
		return result;
	}

	public static void HandleSystemError(Exception e)
	{
		if (e is DirectoryNotFoundException)
		{
			SendOnDirectoryNotFound(e);
		}
		else if (e is FileNotFoundException)
		{
			SendOnFileNotFoundException(e);
		}
		else if (e is IOException)
		{
			SendOnIOException(e);
		}
		else
		{
			SendOnFileAccessError(e);
		}
	}

	public static void SendOnFileAccessError(Exception e)
	{
		OnFileSystemError?.Invoke(e, "File Access Exception!:", URLfileAccessError);
	}

	public static void SendOnDirectoryNotFound(Exception e)
	{
		OnFileSystemError?.Invoke(e, "Directory Not Found!:", URLDirectoryNotFound);
	}

	public static void SendOnFileNotFoundException(Exception e)
	{
		OnFileSystemError?.Invoke(e, "File Not Found!:", URLFileNotFoundException);
	}

	public static void SendOnIOException(Exception e)
	{
		OnFileSystemError?.Invoke(e, "IO Exception!:", URLIOException);
	}
}
