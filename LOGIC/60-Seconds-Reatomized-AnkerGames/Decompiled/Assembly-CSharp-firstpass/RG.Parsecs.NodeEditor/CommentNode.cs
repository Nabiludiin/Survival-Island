using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Comment Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent),
	typeof(EndGameCanvas)
})]
public class CommentNode : ParsecsNode
{
	public const string ID = "GE_CommentNode";

	public const string NODE_NAME = "Comment";

	[SerializeField]
	private string _textInput;

	[SerializeField]
	private float _sizeX;

	[SerializeField]
	private float _sizeY;

	public override string GetID => "GE_CommentNode";

	public override Node Create(Vector2 pos)
	{
		CommentNode commentNode = ScriptableObject.CreateInstance<CommentNode>();
		commentNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		commentNode._sizeX = 150f;
		commentNode._sizeY = 100f;
		commentNode.name = "Comment";
		return commentNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		CommentNode commentNode = (CommentNode)Create(rect.position + new Vector2(20f, 20f));
		commentNode._textInput = _textInput;
		commentNode._sizeX = _sizeX;
		commentNode._sizeY = _sizeY;
		return commentNode;
	}

	public override Rect GetNodeRect()
	{
		return new Rect(rect.position.x, rect.position.y, _sizeX, _sizeY);
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}
}
