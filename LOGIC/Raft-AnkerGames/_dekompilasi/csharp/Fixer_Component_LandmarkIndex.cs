using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fixer_Component_LandmarkIndex : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		return IsCorrectIndexNavMeshIndexComparedToSceneName(target);
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}

	private bool IsCorrectIndexNavMeshIndexComparedToSceneName(Transform target)
	{
		bool flag = false;
		NavMeshSurface componentInChildren = target.GetComponentInChildren<NavMeshSurface>(includeInactive: true);
		int sceneIndexFromSceneName = GetSceneIndexFromSceneName(target);
		int num = 0;
		if (componentInChildren != null)
		{
			num = GetNavmeshObjectNameFromTransformName(componentInChildren);
			if (sceneIndexFromSceneName == num)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
			Debug.LogError(target.transform.name + " does not have the same name index as its respective scene" + target.gameObject.scene.name + " sceneindex: " + sceneIndexFromSceneName + " navIndex " + num);
		}
		return flag;
	}

	private int GetSceneIndexFromSceneName(Transform target)
	{
		return int.Parse(target.gameObject.scene.name.Split('#')[0]);
	}

	private int GetNavmeshObjectNameFromTransformName(NavMeshSurface surf)
	{
		return int.Parse(GetStringIntAtEndofString(surf.gameObject.name));
	}

	private string GetStringIntAtEndofString(string input)
	{
		Stack<char> stack = new Stack<char>();
		int num = input.Length - 1;
		while (num >= 0 && char.IsNumber(input[num]))
		{
			stack.Push(input[num]);
			num--;
		}
		return new string(stack.ToArray());
	}
}
