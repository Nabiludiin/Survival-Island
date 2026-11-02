using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

[Node(false, "Flow Nodes/On Status Node", new Type[] { typeof(SystemStatusEvent) })]
public class OnStatusNode : FlowNode
{
	public const string ID = "EE_OnStatusNode";

	public const string NODE_NAME = "On Status";

	public const string ON_EXPEDITION_OUTPUT_NAME = "On Expedition";

	public const string NORMAL_OUTPUT_NAME = "Normal";

	public const int ON_EXPEDITION_OUTPUT_INDEX = 0;

	public const int NORMAL_OUTPUT_INDEX = 1;

	public override string GetID => "EE_OnStatusNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		OnStatusNode onStatusNode = ScriptableObject.CreateInstance<OnStatusNode>();
		onStatusNode.rect = new Rect(pos.x, pos.y, 150f, 30f);
		onStatusNode.name = "On Status";
		onStatusNode.CreateOutput("On Expedition", "Flow");
		onStatusNode.CreateOutput("Normal", "Flow");
		return onStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
		ParsecsEvent parsecsEvent = NodeEditor.curNodeCanvas as ParsecsEvent;
		if (parsecsEvent != null && !parsecsEvent.ContainsParameter("StatusCharacter"))
		{
			LogMessage(string.Format("There is no parameter \"{0}\" in this event. This is required for this event to work propely.", "StatusCharacter"), EMessageType.ERROR);
		}
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		Character characterParameterByName = ParentEvent.GetCharacterParameterByName("StatusCharacter");
		if (characterParameterByName == null)
		{
			throw new UnityException(string.Format("This Status Event parameter: {0} is empty.", "StatusCharacter"));
		}
		if (characterParameterByName.RuntimeData.IsOnExpedition())
		{
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		else
		{
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
	}
}
