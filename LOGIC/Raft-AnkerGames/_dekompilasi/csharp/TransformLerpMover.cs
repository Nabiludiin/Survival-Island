using UnityEngine;

public class TransformLerpMover : MonoBehaviour
{
	public Transform toFollow;

	public Transform toMove;

	public float movementLerpSpeed;

	public float rotationLerpSpeed;

	public bool doDistanceAndTeleportCheck = true;

	public float maxDistanceToTeleport = 5f;

	private void Start()
	{
		if (Raft_Network.IsHost)
		{
			toMove.SetParent(null);
		}
	}

	private void LateUpdate()
	{
		if (!(toMove == null))
		{
			toMove.transform.position = Vector3.MoveTowards(toMove.position, toFollow.position, movementLerpSpeed * Time.deltaTime);
			toMove.rotation = Quaternion.Lerp(toMove.rotation, toFollow.rotation, rotationLerpSpeed * Time.deltaTime);
			if (doDistanceAndTeleportCheck && CheckToTeleport(maxDistanceToTeleport))
			{
				SetPositionTo(toMove, toFollow.transform.position);
			}
		}
	}

	private void OnDestroy()
	{
		if (Raft_Network.IsHost && toMove != null && toMove.gameObject != null)
		{
			Object.Destroy(toMove.gameObject);
		}
	}

	private bool CheckToTeleport(float maxDistance)
	{
		if (Vector3.Distance(toFollow.transform.position, toMove.transform.position) > maxDistance)
		{
			return true;
		}
		return false;
	}

	private void SetPositionTo(Transform affected, Vector3 position)
	{
		affected.transform.position = position;
	}
}
