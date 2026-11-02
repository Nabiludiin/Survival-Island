using System;
using UnityEngine;

public class LandmarkItem : MonoBehaviour
{
	public bool resetOnLandmarkReset = true;

	public MonoBehaviour_ID_Network connectedBehaviourID;

	public Vector3 localLandmarkPosition;

	[SerializeField]
	private UnityEngine.Object trackBehaviourIDType;

	protected virtual void OnDestroy()
	{
		if (connectedBehaviourID != null)
		{
			NetworkIDManager.RemoveNetworkID(connectedBehaviourID, connectedBehaviourID.GetType());
		}
	}

	private void OnValidate()
	{
		if (connectedBehaviourID == null)
		{
			connectedBehaviourID = GetComponent<MonoBehaviour_ID_Network>();
		}
	}

	public virtual RGD_LandmarkItem GetSave()
	{
		return new RGD_LandmarkItem(this);
	}

	public virtual void OnLandmarkReset()
	{
		ResetLandmarkItem();
	}

	protected virtual void ResetLandmarkItem()
	{
		if (resetOnLandmarkReset)
		{
			gameObject.SetActiveSafe(value: true);
		}
	}

	public virtual void AssignIndexAndTrack(uint objectIndex)
	{
		if (connectedBehaviourID != null)
		{
			if (connectedBehaviourID == null)
			{
				Debug.LogError("Can't track landmarkitem because connectedBehaviourID is null");
				return;
			}
			connectedBehaviourID.ObjectIndex = objectIndex;
			Type type = ((trackBehaviourIDType != null) ? Type.GetType(trackBehaviourIDType.name) : connectedBehaviourID.GetType());
			NetworkIDManager.AddNetworkID(connectedBehaviourID, type);
		}
	}
}
