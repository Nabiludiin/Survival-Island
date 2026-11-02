using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_Node : QuestInteractable_ComponentData
{
	[Serializable]
	public class Node
	{
		public Transform transform;

		public float speed = 6f;

		public float rotSpeed = 5f;

		public float curve;

		public int newStateIndex = -1;

		public bool ShouldMove => speed > 0f;

		public bool ShouldRotate => rotSpeed > 0f;
	}

	public Transform transformToMove;

	public int subPathCount = 7;

	public bool turnOffObjectAtEnd = true;

	public List<Node> nodes = new List<Node>();

	public List<Vector3> GetPathBetweenNodes(int nodeIndexA, int nodeIndexB)
	{
		if (nodeIndexA >= 0 && nodeIndexB >= 0 && nodeIndexB < nodes.Count)
		{
			Node node = nodes[nodeIndexA];
			Node node2 = nodes[nodeIndexB];
			List<Vector3> list = new List<Vector3>();
			if (node.curve <= 0f)
			{
				list.Add(node.transform.position);
				list.Add(node2.transform.position);
				return list;
			}
			for (int i = 0; i < subPathCount; i++)
			{
				list.Add(Helper.GetBezierCurveValue(node.transform.position, node2.transform.position, node.transform.forward * node.curve, (float)i / (float)(subPathCount - 1)));
			}
			return list;
		}
		return null;
	}
}
