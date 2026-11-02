using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Set Planet Landing Animation Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent)
})]
public class SetPlanetLandingAnimationNode : ParsecsNode
{
	public const string ID = "PE_SetPlanetLandingAnimationNode";

	public const string NODE_NAME = "Set Planet Landing Animation";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_SHOW_ANIM_INDEX = 1;

	private const int OUTPUT_FLOW_INDEX = 0;

	private const string INPUT_FLOW_NAME = "In";

	private const string OUTPUT_FLOW_NAME = "Out";

	private const string INPUT_SHOW_ANIM_NAME = "Show Animation";

	[SerializeField]
	private bool _showAnimation = true;

	public override string GetID => "PE_SetPlanetLandingAnimationNode";

	public override Node Create(Vector2 pos)
	{
		SetPlanetLandingAnimationNode setPlanetLandingAnimationNode = ScriptableObject.CreateInstance<SetPlanetLandingAnimationNode>();
		setPlanetLandingAnimationNode.rect = new Rect(pos.x, pos.y, 200f, 100f);
		setPlanetLandingAnimationNode.name = "Set Planet Landing Animation";
		setPlanetLandingAnimationNode.CreateInput("In", "Flow");
		setPlanetLandingAnimationNode.CreateInput("Show Animation", "Bool");
		setPlanetLandingAnimationNode.CreateOutput("Out", "Flow");
		return setPlanetLandingAnimationNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetPlanetLandingAnimationNode setPlanetLandingAnimationNode = (SetPlanetLandingAnimationNode)Create(pos + new Vector2(20f, 20f));
		setPlanetLandingAnimationNode._showAnimation = _showAnimation;
		return setPlanetLandingAnimationNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _showAnimation, canvas);
		EndOfDayManager.Instance.Data.LandedOnPlanet = !_showAnimation;
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
