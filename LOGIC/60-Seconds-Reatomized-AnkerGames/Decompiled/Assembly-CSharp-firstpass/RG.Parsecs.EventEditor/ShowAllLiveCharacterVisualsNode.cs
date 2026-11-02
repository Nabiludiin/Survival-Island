using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Show All Live Character Visuals Node", new Type[] { typeof(EndGameCanvas) })]
public class ShowAllLiveCharacterVisualsNode : EventNode
{
	[Serializable]
	public class CharacterVisuals
	{
		public Character Character;

		public VisualsData VisualsData;

		public VisualId VisualsId;
	}

	public const string ID = "EE_showAllLiveCharacterVisualsNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Show Live Characters Visuals Node";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	[SerializeField]
	private List<CharacterVisuals> _characterVisuals = new List<CharacterVisuals>();

	public override string GetID => "EE_showAllLiveCharacterVisualsNode";

	public override Node Create(Vector2 pos)
	{
		ShowAllLiveCharacterVisualsNode showAllLiveCharacterVisualsNode = ScriptableObject.CreateInstance<ShowAllLiveCharacterVisualsNode>();
		showAllLiveCharacterVisualsNode.rect = new Rect(pos.x, pos.y, 400f, 40f);
		showAllLiveCharacterVisualsNode.name = "Show Live Characters Visuals Node";
		showAllLiveCharacterVisualsNode.CreateMutliInput("In", "Flow");
		showAllLiveCharacterVisualsNode.CreateOutput("Out", "Flow");
		return showAllLiveCharacterVisualsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ShowAllLiveCharacterVisualsNode showAllLiveCharacterVisualsNode = (ShowAllLiveCharacterVisualsNode)Create(rect.position + new Vector2(20f, 20f));
		showAllLiveCharacterVisualsNode._characterVisuals = _characterVisuals;
		return showAllLiveCharacterVisualsNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		for (int i = 0; i < CharacterManager.Instance.GetCharacterList().CharactersInGame.Count; i++)
		{
			for (int j = 0; j < _characterVisuals.Count; j++)
			{
				string iD = _characterVisuals[j].Character.ID;
				if (CharacterManager.Instance.GetCharacterList().CharactersInGame[i].ID.Equals(iD) && CharacterManager.Instance.GetCharacterList().CharactersInGame[i].RuntimeData.IsAlive())
				{
					_characterVisuals[j].VisualsData.RuntimeData.VisualToDisplay = _characterVisuals[j].VisualsId;
				}
			}
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
