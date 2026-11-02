using System;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Text Nodes/Get Text Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ReportEvent),
	typeof(EndGameCanvas)
})]
public class GetTextNode : MessageNode
{
	public const string ID = "EE_GetTextNode";

	[SerializeField]
	private LocalizedString _text;

	public override string GetID => "EE_GetTextNode";

	public override Node Create(Vector2 pos)
	{
		GetTextNode getTextNode = ScriptableObject.CreateInstance<GetTextNode>();
		getTextNode.rect = new Rect(pos.x, pos.y, 300f, 70f);
		getTextNode.name = "Get Text";
		getTextNode.CreateOutput("Out", "LocalizedString");
		return getTextNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetTextNode getTextNode = (GetTextNode)Create(rect.position + new Vector2(20f, 20f));
		getTextNode._text = _text;
		return getTextNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(_text);
	}
}
