using System;
using System.Text;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "DebugData", menuName = "RG/Data/Core/DebugData", order = 1)]
public class DebugData : DataObject, ISerializable
{
	private const string ERROR_MSG = "[ERROR]";

	private const string WARNING_MSG = "[WARNING]";

	private const string DEBUG_FORMAT = "{0}{1}{2}";

	private StringBuilder _log = new StringBuilder();

	public override void ResetData()
	{
		_log.Remove(0, _log.Length);
	}

	public void LogError(string msg, string format = null, object context = null)
	{
		Log(msg, "[ERROR]", format, context);
	}

	public void LogWarning(string msg, string format = null, object context = null)
	{
		Log(msg, "[WARNING]", format, context);
	}

	public void Log(string msg, string categoryTag = null, string format = null, object context = null)
	{
		string arg = (string.IsNullOrEmpty(format) ? msg : string.Format(format, msg));
		_log.AppendFormat("{0}{1}{2}", string.IsNullOrEmpty(categoryTag) ? string.Empty : categoryTag, (context == null) ? string.Empty : context.ToString(), arg);
	}

	private string ConstructLogEntry(string msg, string format, object context, params object[] args)
	{
		return string.Empty;
	}

	public void AppendLog(string log, bool continued = false)
	{
		if (continued)
		{
			_log.Append(log);
		}
		else
		{
			_log.AppendLine(log);
		}
	}

	public void AppendLogFormat(string format, params object[] args)
	{
		_log.AppendFormat(format, args);
	}

	public void AppendLogWarningInfo()
	{
		AppendLog("[WARNING]", continued: true);
	}

	public void AppendLogErrorInfo()
	{
		AppendLog("[ERROR]", continued: true);
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
