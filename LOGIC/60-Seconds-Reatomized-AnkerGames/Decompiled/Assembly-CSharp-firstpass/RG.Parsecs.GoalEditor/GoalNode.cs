using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

public abstract class GoalNode : ParsecsNode
{
	protected const string GOAL_IS_NOT_SET_MESSAGE = "Goal is not set!";

	private GUIStyle _headerBox;

	private GUIStyle _headerBoxBold;

	protected override GUIStyle HeaderBox
	{
		get
		{
			if (_headerBox == null || _headerBox.normal.background == null)
			{
				_headerBox = new GUIStyle(NodeEditorGUI.nodeBox);
				_headerBox.normal.textColor = NodeEditorGUI.NE_TextColor;
				_headerBox.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_Purple.png");
			}
			return _headerBox;
		}
	}

	protected override GUIStyle HeaderBoxBold
	{
		get
		{
			if (_headerBoxBold == null || _headerBoxBold.normal.background == null)
			{
				_headerBoxBold = new GUIStyle(NodeEditorGUI.nodeBoxBold);
				_headerBoxBold.normal.textColor = NodeEditorGUI.NE_TextColor;
				_headerBoxBold.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_Purple.png");
			}
			return _headerBoxBold;
		}
	}
}
