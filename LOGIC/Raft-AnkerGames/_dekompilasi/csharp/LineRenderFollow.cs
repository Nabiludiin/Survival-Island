using UnityEngine;

public class LineRenderFollow : MonoBehaviour
{
	[SerializeField]
	private LineRenderer lineRenderer;

	[SerializeField]
	private Rope.RopeAutoFollow[] followTargets;

	private void Button_SetPositions()
	{
		SetPositionsToFollowTargets();
	}

	private void Start()
	{
		if (followTargets == null || followTargets.Length == 0 || lineRenderer == null)
		{
			enabled = false;
		}
	}

	private void Update()
	{
		SetPositionsToFollowTargets();
	}

	private void SetPositionsToFollowTargets()
	{
		for (int i = 0; i < followTargets.Length; i++)
		{
			Rope.RopeAutoFollow ropeAutoFollow = followTargets[i];
			lineRenderer.SetPosition(ropeAutoFollow.index, ropeAutoFollow.followParent.position);
		}
	}
}
