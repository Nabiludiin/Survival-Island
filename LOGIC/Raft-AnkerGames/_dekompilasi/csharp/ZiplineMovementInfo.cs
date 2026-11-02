using UnityEngine;

internal struct ZiplineMovementInfo
{
	public Vector3 lastMovedDirection;

	public Vector3 backwardPosition;

	public Vector3 forwardPosition;

	public Vector3 firstPointPosition;

	public Vector3 lastPointPosition;

	public int backwardPointIndex;

	public int forwardPointIndex;

	public void SetInfoFromPointIndex(MeshPath_Zipline path, int startPointIndex, float startZiplineSpeed)
	{
		if (startZiplineSpeed > 0f)
		{
			backwardPointIndex = startPointIndex;
			forwardPointIndex = startPointIndex + 1;
		}
		else
		{
			backwardPointIndex = startPointIndex - 1;
			forwardPointIndex = startPointIndex;
		}
		ClampPointIndexesWithinPath(path);
		path.GetLocalPointPositionFromIndex(backwardPointIndex, ref backwardPosition);
		path.GetLocalPointPositionFromIndex(forwardPointIndex, ref forwardPosition);
	}

	public void IncrementIndex(MeshPath_Zipline path, bool forward)
	{
		if (forward)
		{
			forwardPointIndex++;
			backwardPointIndex++;
		}
		else
		{
			forwardPointIndex--;
			backwardPointIndex--;
		}
		ClampPointIndexesWithinPath(path);
		path.GetLocalPointPositionFromIndex(backwardPointIndex, ref backwardPosition);
		path.GetLocalPointPositionFromIndex(forwardPointIndex, ref forwardPosition);
	}

	public Vector3 GeLocalTargetPositionFromSpeed(float ziplineSpeed)
	{
		if (ziplineSpeed > 0f)
		{
			return forwardPosition;
		}
		return backwardPosition;
	}

	public Vector3 GetLocalTargetPositionFromPlayerRotation(Vector3 playerForwardPos)
	{
		if (Vector3.Distance(playerForwardPos, forwardPosition) <= Vector3.Distance(playerForwardPos, backwardPosition))
		{
			return forwardPosition;
		}
		return backwardPosition;
	}

	private void ClampPointIndexesWithinPath(MeshPath_Zipline path)
	{
		if (forwardPointIndex >= path.points.Count)
		{
			forwardPointIndex = path.points.Count - 1;
			backwardPointIndex = forwardPointIndex - 1;
		}
		else if (backwardPointIndex < 0)
		{
			backwardPointIndex = 0;
			forwardPointIndex = 1;
		}
	}
}
