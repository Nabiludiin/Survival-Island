using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractableComponent_MoveAlongNodes : QuestInteractableComponentBase<QuestInteractable_ComponentData_Node>
{
	[SerializeField]
	private QuestEventBase questEvent;

	private Coroutine moveRoutine;

	private void OnDrawGizmos()
	{
		if (dataComponents == null || dataComponents.Count == 0)
		{
			return;
		}
		QuestInteractable_ComponentData_Node questInteractable_ComponentData_Node = dataComponents[0];
		if (questInteractable_ComponentData_Node != null && questInteractable_ComponentData_Node.transformToMove != null && questInteractable_ComponentData_Node.nodes != null && questInteractable_ComponentData_Node.nodes.Count > 0 && questInteractable_ComponentData_Node.nodes[0] != null && questInteractable_ComponentData_Node.nodes[0].transform != null)
		{
			Helper.DrawArrowPath(questInteractable_ComponentData_Node.transformToMove.position, questInteractable_ComponentData_Node.nodes[0].transform.position, Color.yellow);
		}
		foreach (QuestInteractable_ComponentData_Node dataComponent in dataComponents)
		{
			for (int i = 0; i < dataComponent.nodes.Count - 1; i++)
			{
				QuestInteractable_ComponentData_Node.Node node = dataComponent.nodes[i];
				QuestInteractable_ComponentData_Node.Node node2 = dataComponent.nodes[i + 1];
				if (node != null && node2 != null && !(node.transform == null))
				{
					_ = node2.transform == null;
				}
			}
		}
	}

	protected override void FinishInteractionWithData(QuestInteractable_ComponentData_Node data, Network_Player player, bool atStart, bool successfull)
	{
		base.FinishInteractionWithData(data, player, atStart, successfull);
		if (successfull)
		{
			if (moveRoutine != null)
			{
				CoroutineManager.Singleton.StopCoroutine(moveRoutine);
			}
			moveRoutine = CoroutineManager.Singleton.AddCoroutine(MoveAlongWaypoints(data, atStart));
		}
	}

	private IEnumerator MoveAlongWaypoints(QuestInteractable_ComponentData_Node data, bool instant)
	{
		if (data.nodes == null || data.nodes.Count == 0)
		{
			yield break;
		}
		int index = 0;
		Transform transformToMove = data.transformToMove;
		while (index < data.nodes.Count - 1)
		{
			QuestInteractable_ComponentData_Node.Node node = data.nodes[index];
			if (node == null)
			{
				index++;
				continue;
			}
			if (node.newStateIndex >= 0)
			{
				questEvent.SetNewObjectStateIndex(node.newStateIndex);
			}
			List<Vector3> path = data.GetPathBetweenNodes(index, index + 1);
			if (path != null)
			{
				if (!instant)
				{
					for (int i = 0; i < path.Count; i++)
					{
						Vector3 vector = path[i];
						Vector3 vector2 = vector - transformToMove.position;
						vector2.y = 0f;
						if (node.ShouldMove)
						{
							transformToMove.position = Vector3.MoveTowards(transformToMove.position, vector, Time.deltaTime * node.speed);
						}
						if (node.ShouldRotate)
						{
							transformToMove.rotation = Quaternion.Slerp(transformToMove.rotation, Quaternion.LookRotation(vector2), Time.deltaTime * node.rotSpeed);
						}
						bool flag = false;
						if (node.ShouldMove)
						{
							if (Vector3.Distance(transformToMove.position, vector) < 0.01f)
							{
								flag = true;
							}
						}
						else if (node.ShouldRotate && Mathf.Abs(Vector3.Angle(transformToMove.forward, vector2)) < 5f)
						{
							flag = true;
						}
						if (!flag)
						{
							i--;
						}
						yield return null;
					}
				}
				else
				{
					Vector3 vector3 = (transformToMove.position = path[path.Count - 1]);
					if (path.Count > 1)
					{
						Vector3 vector5 = path[path.Count - 2];
						Vector3 forward = vector3 - vector5;
						forward.y = 0f;
						transformToMove.rotation = Quaternion.LookRotation(forward);
					}
				}
			}
			index++;
			yield return null;
		}
		if (data.turnOffObjectAtEnd)
		{
			transformToMove.gameObject.SetActiveSafe(value: false);
		}
	}
}
