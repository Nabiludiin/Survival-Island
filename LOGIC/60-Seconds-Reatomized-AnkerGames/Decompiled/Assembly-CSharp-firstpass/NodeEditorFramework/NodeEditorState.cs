using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework;

public class NodeEditorState : ScriptableObject
{
	public NodeCanvas canvas;

	public NodeEditorState parentEditor;

	public string Path;

	public bool drawing = true;

	public List<Node> selectedNodes = new List<Node>();

	[NonSerialized]
	public Node focusedNode;

	[NonSerialized]
	public NodeKnob focusedNodeKnob;

	public Vector2 panOffset;

	public float zoom = 1f;

	[NonSerialized]
	public NodeOutput connectOutput;

	[NonSerialized]
	public bool dragNode;

	[NonSerialized]
	public bool panWindow;

	[NonSerialized]
	public Vector2 dragStart;

	[NonSerialized]
	public Vector2 dragPos;

	[NonSerialized]
	public Vector2 dragOffset;

	[NonSerialized]
	public bool navigate;

	[NonSerialized]
	public Rect canvasRect;

	[NonSerialized]
	public Vector2 zoomPanAdjust;

	[NonSerialized]
	public List<Rect> ignoreInput = new List<Rect>();

	public Node SelectedNode
	{
		get
		{
			if (selectedNodes.Count > 0)
			{
				return selectedNodes[0];
			}
			return null;
		}
		set
		{
			if (selectedNodes.Count <= 0)
			{
				selectedNodes.Add(value);
			}
			else
			{
				selectedNodes[0] = value;
			}
		}
	}

	public Vector2 zoomPos => canvasRect.size / 2f;
}
