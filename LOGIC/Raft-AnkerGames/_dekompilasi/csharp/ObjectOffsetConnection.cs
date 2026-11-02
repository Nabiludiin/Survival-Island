using System;
using UnityEngine;

[Serializable]
public class ObjectOffsetConnection
{
	public string connectionName = string.Empty;

	[SerializeField]
	private Vector3 localPosition = Vector3.zero;

	[SerializeField]
	private Vector3 localEuler = Vector3.zero;

	[SerializeField]
	private Transform parent;

	public void SetTransformToSettings(Transform target)
	{
		if (parent != null)
		{
			target.SetParentSafe(parent);
		}
		target.localPosition = localPosition;
		target.localEulerAngles = localEuler;
	}
}
