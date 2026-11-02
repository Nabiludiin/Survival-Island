using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using RG.Parsecs.EventEditor;
using UnityEngine;

public abstract class PlayerDecisionNode : EventNode
{
	protected const string INPUT_IN_NAME = "In";

	protected const string OUTPUT_OUT_NAME = "Out";

	protected const int INPUT_IN_INDEX = 0;

	protected const int OUTPUT_OUT_INDEX = 0;

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
				_headerBox.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_LightBlue.png");
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
				_headerBoxBold.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_LightBlue.png");
			}
			return _headerBoxBold;
		}
	}
}
