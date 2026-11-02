using System;
using System.Collections.Generic;
using UnityEngine;

public class PipeGroupManager : MonoBehaviour, SceneEventInterface
{
	public static Action<PipeGroup> OnPipeGroupAdded;

	public static Action<PipeGroup> OnPipeGroupRemoved;

	private static Dictionary<Vector3, Pipe> allPipes = new Dictionary<Vector3, Pipe>();

	private static List<PipeGroup> pipeGroups = null;

	private void OnDestroy()
	{
		ResetPipeGroups();
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			ResetPipeGroups();
		}
	}

	public static PipeGroup CreateNewPipeGroup()
	{
		PipeGroup pipeGroup = new PipeGroup();
		if (AddPipeGroup(pipeGroup))
		{
			OnPipeGroupAdded?.Invoke(pipeGroup);
			return pipeGroup;
		}
		return null;
	}

	public static void RemovePipeGroup(PipeGroup pipeGroup)
	{
		if (pipeGroup != null)
		{
			OnPipeGroupRemoved?.Invoke(pipeGroup);
			pipeGroup.DisbandGroup();
			pipeGroups.Remove(pipeGroup);
		}
	}

	public static void TrackPipe(Vector3 position, Pipe pipe)
	{
		position = KeySafeRounding(position);
		if (allPipes != null && !allPipes.ContainsKey(position))
		{
			allPipes.Add(position, pipe);
		}
	}

	public static void UntrackPipe(Vector3 position)
	{
		position = KeySafeRounding(position);
		if (allPipes != null)
		{
			allPipes.Remove(position);
		}
	}

	public static Pipe GetPipeFromPosition(Vector3 position)
	{
		position = KeySafeRounding(position);
		if (allPipes != null && allPipes.ContainsKey(position))
		{
			return allPipes[position];
		}
		return null;
	}

	private static bool AddPipeGroup(PipeGroup pipeGroup)
	{
		if (pipeGroups == null)
		{
			pipeGroups = new List<PipeGroup>();
		}
		return pipeGroups.AddUniqueOnly(pipeGroup);
	}

	private static void ResetPipeGroups()
	{
		if (pipeGroups != null)
		{
			pipeGroups.Clear();
		}
		if (allPipes != null)
		{
			allPipes.Clear();
		}
	}

	private static Vector3 KeySafeRounding(Vector3 value)
	{
		return new Vector3(Mathf.Round(value.x * 10f) / 10f, Mathf.Round(value.y * 10f) / 10f, Mathf.Round(value.z * 10f) / 10f);
	}
}
