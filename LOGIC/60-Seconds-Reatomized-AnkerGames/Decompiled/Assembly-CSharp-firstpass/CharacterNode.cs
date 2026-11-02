using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using RG.Parsecs.EventEditor;
using UnityEngine;

public abstract class CharacterNode : EventNode
{
	protected const string CHARACTER_NOT_SET_MESSAGE = "Character is not set!";

	protected const string CHARACTER_LIST_NOT_SET_MESSAGE = "Character list is not set!";

	protected const string INPUT_NOT_CONNECTED_MESSAGE = "Input is not connected";

	protected const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	protected const string STATUS_NOT_SET_MESSAGE = "Status is not set!";

	protected const string ATTRIBUTE_NOT_SET_MESSAGE = "Attribute is not set!";

	protected const string NEED_NOT_SET_MESSAGE = "Need is not set!";

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
				_headerBox.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_Brown.png");
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
				_headerBoxBold.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_Brown.png");
			}
			return _headerBoxBold;
		}
	}
}
