using UnityEngine;

public class RaftTrail : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem trailL;

	[SerializeField]
	private ParticleSystem trailR;

	[SerializeField]
	private Raft raft;

	[SerializeField]
	private Transform raftCenterTransform;

	[SerializeField]
	private float updateTrailPosRate = 1f;

	private float timer;

	private Block leftBlock;

	private Block rightBlock;

	private void Start()
	{
		FindCorrectBlocks();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= updateTrailPosRate * 60f || leftBlock == null || rightBlock == null)
		{
			timer = 0f;
			FindCorrectBlocks();
		}
		if (!raft.Moving)
		{
			if (trailL.isPlaying)
			{
				trailL.Stop();
			}
			if (trailR.isPlaying)
			{
				trailR.Stop();
			}
		}
		else
		{
			if (!trailL.isPlaying)
			{
				trailL.Play();
			}
			if (!trailR.isPlaying)
			{
				trailR.Play();
			}
		}
		if (leftBlock != null)
		{
			Vector3 position = leftBlock.transform.position;
			trailL.transform.position = new Vector3(position.x, 0f, position.z);
		}
		if (rightBlock != null)
		{
			Vector3 position2 = rightBlock.transform.position;
			trailR.transform.position = new Vector3(position2.x, 0f, position2.z);
		}
	}

	private void FindCorrectBlocks()
	{
		Vector3 normalized = Raft.direction.normalized;
		Vector3 vector = Vector3.Cross(normalized, Vector3.up);
		Vector3 position = raftCenterTransform.position;
		normalized.y = (vector.y = (position.y = 0f));
		float num = 1000f;
		Block block = BlockCreator.ClosestFoundationToPosition(position + vector * num - normalized * (num / 2f));
		if (block != null)
		{
			leftBlock = block;
		}
		block = BlockCreator.ClosestFoundationToPosition(position - vector * num - normalized * (num / 2f));
		if (block != null)
		{
			rightBlock = block;
		}
	}
}
