using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

public abstract class CalculationNode : EventNode
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
				_headerBox.normal.textColor = Color.black;
				_headerBox.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_Yellow.png");
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
				_headerBoxBold.normal.textColor = Color.black;
				_headerBoxBold.normal.background = ResourceManager.LoadTexture("Textures/NE_Box_Yellow.png");
			}
			return _headerBoxBold;
		}
	}
}
