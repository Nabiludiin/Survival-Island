using System;
using UnityEngine;

[Serializable]
public class QuestInteractable_ComponentData_ChangeMesh : QuestInteractable_ComponentData
{
	public MeshFilter meshFilter;

	public Mesh newMesh;

	public override void OnFinishedInteraction(Network_Player player, bool atStart, bool successfull)
	{
		base.OnFinishedInteraction(player, atStart, successfull);
		meshFilter.sharedMesh = newMesh;
	}
}
