using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using UnityEngine;

public abstract class ExpeditionNode : EventNode
{
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
