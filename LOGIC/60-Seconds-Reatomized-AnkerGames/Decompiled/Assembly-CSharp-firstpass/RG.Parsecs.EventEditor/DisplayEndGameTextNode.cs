using System;
using System.Collections.Generic;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Functions/Display End Game Text", new Type[] { typeof(EndGameCanvas) })]
public class DisplayEndGameTextNode : ParsecsNode
{
	public const string ID = "EGC_DisplayEndGameText";

	private const int FLOW_INPUT_INDEX = 0;

	private const int TEXT_INPUT_INDEX = 1;

	private const int CHARACTER_INPUT_INDEX = 2;

	private const int ITEM_INPUT_INDEX = 3;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Display end game text";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string TEXT_INPUT_NAME = "Term";

	private const string CHARACTER_INPUT_NAME = "Character";

	private const string ITEM_INPUT_NAME = "Item";

	private const int OFFSET = 4;

	[SerializeField]
	private LocalizedString _text;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private IItem _item;

	[SerializeField]
	private int _counter;

	private bool _showTermProperty;

	private string _termButtonMessage = "Show";

	[SerializeField]
	private EndGameTextId _textId;

	public override string GetID => "EGC_DisplayEndGameText";

	public override Node Create(Vector2 pos)
	{
		DisplayEndGameTextNode displayEndGameTextNode = ScriptableObject.CreateInstance<DisplayEndGameTextNode>();
		displayEndGameTextNode.rect = new Rect(pos.x, pos.y, 300f, 105f);
		displayEndGameTextNode.name = "Display end game text";
		displayEndGameTextNode.CreateMutliInput("In", "Flow");
		displayEndGameTextNode.CreateInput("Term", "LocalizedString");
		displayEndGameTextNode.CreateInput("Character", "Character");
		displayEndGameTextNode.CreateInput("Item", "Item");
		displayEndGameTextNode.CreateOutput("Out", "Flow");
		return displayEndGameTextNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DisplayEndGameTextNode displayEndGameTextNode = (DisplayEndGameTextNode)Create(rect.position + new Vector2(20f, 20f));
		displayEndGameTextNode._text = _text;
		displayEndGameTextNode._character = _character;
		displayEndGameTextNode._item = _item;
		return displayEndGameTextNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void AddNewVariable()
	{
		CreateInput("Var " + _counter, ObjectConnection.ID);
		_counter++;
	}

	private void RemoveVariable()
	{
		if (Inputs.Count > 4)
		{
			Inputs[Inputs.Count - 1].Delete();
			_counter--;
		}
	}

	protected override void OnNodeValidate()
	{
	}

	private List<int> GetLocalVariables(NodeCanvas canvas)
	{
		if (Inputs.Count > 4)
		{
			List<int> list = new List<int>(Inputs.Count - 4);
			for (int i = 4; i < Inputs.Count; i++)
			{
				if (Inputs[i].connection.typeID == "Bool")
				{
					bool currentValue = false;
					GetInputValue(Inputs[i], ref currentValue, canvas);
					list.Add(Convert.ToInt32(currentValue));
				}
				else if (Inputs[i].connection.typeID == "Int")
				{
					int currentValue2 = 0;
					GetInputValue(Inputs[i], ref currentValue2, canvas);
					list.Add(currentValue2);
				}
				else if (Inputs[i].connection.typeID == "PlayerDecision")
				{
					PlayerDecision currentValue3 = null;
					GetInputValue(Inputs[i], ref currentValue3, canvas);
					list.Add(currentValue3.ChoosenNumber);
				}
				else
				{
					list.Add(-1);
					Debug.LogError("Error in DisplayTextNode - wrong connection!!!");
				}
			}
			return list;
		}
		return null;
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _text, canvas);
		GetInputValue(Inputs[2], ref _character, canvas);
		GetInputValue(Inputs[3], ref _item, canvas);
		string translatedText = new EventContentData(EventContentData.EEventContentType.TEXT, _text)
		{
			Character = _character,
			Item = _item,
			LocalVariablesInts = GetLocalVariables(canvas)
		}.GetTranslatedText();
		EndOfGameViewManager.Instance.DisplayText(_textId, translatedText);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
