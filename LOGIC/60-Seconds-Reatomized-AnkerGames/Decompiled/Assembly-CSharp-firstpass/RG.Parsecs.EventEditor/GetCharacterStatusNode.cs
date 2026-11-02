using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Get Character Status Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetCharacterStatusNode : CharacterNode
{
	public const string ID = "EE_GetCharacterStatusNode";

	public const string NODE_NAME = "Get Character Status";

	public const string OUTPUT_NAME = "Out";

	public const int OUTPUT_OUT_INDEX = 0;

	[SerializeField]
	private CharacterStatus _status;

	public override string GetID => "EE_GetCharacterStatusNode";

	public override Node Create(Vector2 pos)
	{
		GetCharacterStatusNode getCharacterStatusNode = ScriptableObject.CreateInstance<GetCharacterStatusNode>();
		getCharacterStatusNode.rect = new Rect(pos.x, pos.y, 150f, 90f);
		getCharacterStatusNode.name = "Get Character Status";
		getCharacterStatusNode.CreateOutput("Out", "CharacterStatusConnection");
		return getCharacterStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCharacterStatusNode getCharacterStatusNode = (GetCharacterStatusNode)Create(rect.position + new Vector2(20f, 20f));
		getCharacterStatusNode._status = _status;
		return getCharacterStatusNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
		base.OnNodeValidate();
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (_status == null)
		{
			throw new UnityException("No character status provided for Get Character Status Node!");
		}
		return CastValue<T>(_status);
	}

	public override string ToString()
	{
		return _status.ToString();
	}
}
