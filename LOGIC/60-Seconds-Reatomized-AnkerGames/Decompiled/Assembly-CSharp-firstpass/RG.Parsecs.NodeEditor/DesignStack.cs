using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

public static class DesignStack
{
	private const string EXTENSION = ".txt";

	private const string FILE_NAME = "Design_Stack";

	private const string LAST_FILE_PREFIX = "Last_";

	private const string DIRECTORY_NAME = "DesignStacks";

	private static StreamWriter _currentDesignStackFile;

	public static void OpenLastDesignStackInDefaultTextEditor()
	{
		Process.Start(Path.Combine(Path.Combine(Application.persistentDataPath, "DesignStacks"), "Last_Design_Stack.txt"));
	}

	public static void Initialize()
	{
		if (_currentDesignStackFile != null)
		{
			_currentDesignStackFile.Close();
			_currentDesignStackFile = null;
		}
		CreateDesignFile();
	}

	public static void CreateDesignFile()
	{
		string text = Path.Combine(Application.persistentDataPath, "DesignStacks");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
			UnityEngine.Debug.LogFormat("Creating directory at path: " + text);
		}
		string text2 = Path.Combine(text, "Last_Design_Stack.txt");
		UnityEngine.Debug.LogFormat("Initializing DesignStack at path: {0}", text2);
		if (File.Exists(text2))
		{
			using StreamReader streamReader = new StreamReader(text2);
			string text3 = streamReader.ReadLine();
			streamReader.Close();
			File.Move(text2, text2.Replace("Last_Design_Stack", "Design_Stack_" + text3));
		}
		_currentDesignStackFile = new StreamWriter(text2, append: false);
		_currentDesignStackFile.WriteLine(DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
		LogMessage($"\n\n-----\nDAY {1}\n");
		_currentDesignStackFile.AutoFlush = true;
	}

	public static void LogMessage(string message)
	{
		if (_currentDesignStackFile == null)
		{
			UnityEngine.Debug.LogError("Something went wrong during creation of Design Stack file. Message won't be written to file.");
		}
		else
		{
			_currentDesignStackFile.WriteLine(message);
		}
	}
}
