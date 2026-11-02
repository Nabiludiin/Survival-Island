using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(true, "Legacy/Get Crew Count Node", new Type[]
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
public class GetCrewCountNode : CharacterNode
{
	public const string Id = "getCrewCountNode";

	private const int INPUT_INDEX = 0;

	private const string INPUT_NAME = "Alive only";

	private const int OUTPUT_OUT_INDEX = 0;

	private const int OUTPUT_COUNT_INDEX_INDEX = 1;

	private const string OUTPUT_NAME = "Count";

	private const string INDEX_NAME = "Index";

	private const string NODE_NAME = "Get Crew Count";

	[SerializeField]
	private bool _aliveCharactersOnly = true;

	public override string GetID => "getCrewCountNode";

	public override Node Create(Vector2 pos)
	{
		GetCrewCountNode getCrewCountNode = ScriptableObject.CreateInstance<GetCrewCountNode>();
		getCrewCountNode.rect = new Rect(pos.x, pos.y, 150f, 40f);
		getCrewCountNode.name = "Get Crew Count";
		getCrewCountNode.CreateInput("Alive only", "Bool");
		getCrewCountNode.CreateOutput("Count", "Int");
		getCrewCountNode.CreateOutput("Index", "Int");
		return getCrewCountNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCrewCountNode getCrewCountNode = (GetCrewCountNode)Create(rect.position + new Vector2(20f, 20f));
		getCrewCountNode._aliveCharactersOnly = _aliveCharactersOnly;
		return getCrewCountNode;
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
		switch (output)
		{
		case 0:
			GetInputValue(Inputs[0], ref _aliveCharactersOnly, canvas);
			if (_aliveCharactersOnly)
			{
				return CastValue<T>(CharacterManager.Instance.GetCharacterList().GetLiveCharactersCount());
			}
			return CastValue<T>(CharacterManager.Instance.GetCharacterList().GetCharacterCount());
		case 1:
			GetInputValue(Inputs[0], ref _aliveCharactersOnly, canvas);
			if (_aliveCharactersOnly)
			{
				return CastValue<T>(CharacterManager.Instance.GetCharacterList().GetLiveCharactersCount() - 1);
			}
			return CastValue<T>(CharacterManager.Instance.GetCharacterList().GetCharacterCount() - 1);
		default:
			throw new NodeOutputNotFoundException(output, "getCrewCountNode");
		}
	}
}
