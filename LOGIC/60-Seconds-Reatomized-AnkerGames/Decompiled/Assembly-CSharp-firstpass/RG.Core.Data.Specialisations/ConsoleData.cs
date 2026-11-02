using System;
using System.Text;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "ConsoleData", menuName = "RG/Data/Core/ConsoleData", order = 1)]
public class ConsoleData : DataObject, ISerializable
{
	private const string ERROR_MSG = "[ERROR]";

	private const string WARNING_MSG = "[WARNING]";

	private const string DEBUG_FORMAT = "{0} {1} {2}";

	private StringBuilder _log = new StringBuilder();

	public override void ResetData()
	{
		_log.Remove(0, _log.Length);
	}

	public void LogInfo(string msg, string format = null, object context = null, params object[] args)
	{
		Log(msg, null, format, context, args);
	}

	public void LogError(string msg, string format = null, object context = null, params object[] args)
	{
		Log(msg, "[ERROR]", format, context, args);
	}

	public void LogWarning(string msg, string format = null, object context = null, params object[] args)
	{
		Log(msg, "[WARNING]", format, context, args);
	}

	private void Log(string msg, string categoryTag = null, string format = null, object context = null, params object[] args)
	{
		string arg = (string.IsNullOrEmpty(format) ? msg : string.Format(format, args));
		_log.AppendFormat("{0} {1} {2}", string.IsNullOrEmpty(categoryTag) ? string.Empty : categoryTag, (context == null) ? string.Empty : context.ToString(), arg);
	}

	public void Load()
	{
		throw new NotImplementedException();
	}

	public void Save()
	{
		throw new NotImplementedException();
	}
}
