using System;
using UnityEngine;

public class Buoyancy : MonoBehaviour
{
	public bool active = true;

	public float surfaceOffset;

	[SerializeField]
	private float lerpSpeed = 3f;

	[SerializeField]
	private RaftBounds raftBounds;

	[SerializeField]
	private Transform lockedPivot;

	[SerializeField]
	private Transform rotatePivot;

	[SerializeField]
	private Transform pointL;

	[SerializeField]
	private Transform pointR;

	[SerializeField]
	private Transform pointT;

	[SerializeField]
	private Transform pointB;

	[SerializeField]
	private Transform pointC;

	public float CenterPointY => pointC.position.y + surfaceOffset;

	public Vector3 BuoyancyRotation
	{
		get
		{
			Vector3 vector = pointT.position - pointB.position;
			Vector3 rhs = pointR.position - pointL.position;
			Vector3 vector2 = Vector3.Cross(vector, rhs);
			if (vector != Vector3.zero && vector2 != Vector3.zero)
			{
				Vector3 eulerAngles = Quaternion.LookRotation(vector, vector2).eulerAngles;
				eulerAngles.y = rotatePivot.eulerAngles.y;
				return eulerAngles;
			}
			return Vector3.zero;
		}
	}

	private void Awake()
	{
		if (this.raftBounds != null)
		{
			RaftBounds raftBounds = this.raftBounds;
			raftBounds.OnBoundsUpdate = (Action)Delegate.Combine(raftBounds.OnBoundsUpdate, new Action(OnBoundsUpdate));
		}
		else
		{
			Debug.Log("There is no bounds attached to buoyancy script, buoyancy will not work correctly");
		}
	}

	private void OnDestroy()
	{
		if (this.raftBounds != null)
		{
			RaftBounds raftBounds = this.raftBounds;
			raftBounds.OnBoundsUpdate = (Action)Delegate.Remove(raftBounds.OnBoundsUpdate, new Action(OnBoundsUpdate));
		}
	}

	private void Update()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().raftSpecificVariables.usesBuoyancy && active)
		{
			Quaternion b = Quaternion.Euler(BuoyancyRotation);
			rotatePivot.rotation = Quaternion.Lerp(rotatePivot.rotation, b, Time.deltaTime * lerpSpeed);
		}
	}

	private void RefreshChildrenToHolderPosition()
	{
		if (rotatePivot != null && lockedPivot != null)
		{
			lockedPivot.SetParent(null);
			Vector3 position = rotatePivot.position;
			position.x = raftBounds.Center.x;
			position.z = raftBounds.Center.z;
			rotatePivot.position = position;
			lockedPivot.SetParent(rotatePivot);
		}
	}

	private void SetBuoyancyPointPosition(Transform point, Vector3 newPos)
	{
		Vector3 position = point.position;
		position.x = newPos.x;
		position.z = newPos.z;
		point.position = position;
	}

	private void RefreshPointsToBounds()
	{
		SetBuoyancyPointPosition(pointL, raftBounds.Left);
		SetBuoyancyPointPosition(pointR, raftBounds.Right);
		SetBuoyancyPointPosition(pointT, raftBounds.Top);
		SetBuoyancyPointPosition(pointB, raftBounds.Bottom);
		SetBuoyancyPointPosition(pointC, raftBounds.Center);
	}

	private void OnBoundsUpdate()
	{
		RefreshPointsToBounds();
		RefreshChildrenToHolderPosition();
	}
}
