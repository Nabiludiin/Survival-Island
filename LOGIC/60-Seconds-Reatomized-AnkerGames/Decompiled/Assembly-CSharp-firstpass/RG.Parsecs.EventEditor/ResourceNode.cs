using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public abstract class ResourceNode : EventNode
{
	protected const string NODE_IS_NOT_CONNECTED = "Node is not connected";

	protected const string RESOURCE_IS_NOT_SET = "Resource is not set";

	protected const string NOT_SET_ITEM_MESSAGE = "Node has not set any item!";

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
			}
			return _headerBoxBold;
		}
	}
}
