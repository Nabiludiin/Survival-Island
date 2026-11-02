using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class DebugAction : EventAction
{
	public string Message;

	public DebugMessageType DebugMessageType;

	private string _format = "{0} : {1} : {2}";

	protected override void Act(EventActionArgs args = null)
	{
		string text = "";
		string text2 = "";
		if (args != null)
		{
			if (args.Actor != null)
			{
				text = args.Actor.Name;
			}
			if (args.Object != null)
			{
				text2 = args.Object.name;
			}
		}
		switch (DebugMessageType)
		{
		case DebugMessageType.Log:
			Debug.LogFormat(_format, Message, text, text2);
			break;
		case DebugMessageType.Warning:
			Debug.LogWarningFormat(_format, Message, text, text2);
			break;
		case DebugMessageType.Error:
			Debug.LogErrorFormat(_format, Message, text, text2);
			break;
		}
	}
}
