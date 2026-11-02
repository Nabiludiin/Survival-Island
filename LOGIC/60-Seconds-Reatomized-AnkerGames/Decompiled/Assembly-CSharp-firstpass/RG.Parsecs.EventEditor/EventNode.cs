using NodeEditorFramework;
using RG.Parsecs.NodeEditor;

namespace RG.Parsecs.EventEditor;

public abstract class EventNode : ParsecsNode
{
	public bool Paused { get; protected set; }

	protected ParsecsEvent ParentEvent
	{
		get
		{
			NodeFunction nodeFunction = ParentCanvas as NodeFunction;
			if (nodeFunction != null)
			{
				return nodeFunction.GetOriginalCanvasAs<ParsecsEvent>();
			}
			return (ParsecsEvent)ParentCanvas;
		}
	}

	public void Resume()
	{
	}

	protected override void OnNodeValidate()
	{
		if (Inputs != null && Inputs.Count > 0 && !Inputs[0].isConnected && Inputs[0].typeID.Equals("Flow") && (Inputs[0].name == "In" || Inputs[0].name == "    In" || Inputs[0].name == "     In"))
		{
			LogMessage($"IN flow input is not connected in node {GetID}. This node will never be invoked.", EMessageType.WARNING);
		}
	}
}
