using UnityEngine;

public class LaserChallenge_ReflectiveRaycast : ReflectiveRaycast
{
	public string laserTargetTag = "LaserTarget";

	public float questInteractionCooldown = 1f;

	private float questInteractTimeStamp;

	protected override void HandleCastInteract(GameObject go)
	{
		base.HandleCastInteract(go);
		if (Raft_Network.IsHost && Time.time - questInteractTimeStamp > questInteractionCooldown && go != null && go.CompareTag(laserTargetTag))
		{
			go.GetComponent<QuestInteractable>().SendOnInteractNetwork(null, forceSuccess: true);
			questInteractTimeStamp = Time.time;
		}
	}
}
