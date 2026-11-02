using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using RG.SecondsRemaster.Survival;
using UnityEngine;

namespace RG.SecondsRemaster.Nodes;

[Node(false, "Utility Nodes/Display Sprite Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class DisplaySpriteNode : EventNode
{
	public const string ID = "displaySpriteNode";

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_PRIORITY_NAME = "Priority";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_PRIORITY_INDEX = 1;

	private const int OUTPUT_OUT_INDEX = 0;

	public const string NODE_NAME = "Display Sprite";

	[SerializeField]
	private Sprite _sprite;

	[SerializeField]
	private int _priority;

	[SerializeField]
	private EventContentData.ESpriteAlign _spriteAlign = EventContentData.ESpriteAlign.CENTER;

	public override string GetID => "displaySpriteNode";

	public override Node Create(Vector2 pos)
	{
		DisplaySpriteNode displaySpriteNode = ScriptableObject.CreateInstance<DisplaySpriteNode>();
		displaySpriteNode.rect = new Rect(pos.x, pos.y, 250f, 105f);
		displaySpriteNode.name = "Display Sprite";
		displaySpriteNode.CreateMutliInput("In", "Flow");
		displaySpriteNode.CreateInput("Priority", "Int");
		displaySpriteNode.CreateOutput("Out", "Flow");
		return displaySpriteNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DisplaySpriteNode displaySpriteNode = (DisplaySpriteNode)Create(rect.position + new Vector2(20f, 20f));
		displaySpriteNode._sprite = _sprite;
		displaySpriteNode._spriteAlign = _spriteAlign;
		displaySpriteNode._priority = _priority;
		return displaySpriteNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _priority, canvas);
		SpriteJournalContent content = new SpriteJournalContent(_sprite, _spriteAlign, _priority);
		SecondsEventManager.AddJournalContent(ParentCanvas, content);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
