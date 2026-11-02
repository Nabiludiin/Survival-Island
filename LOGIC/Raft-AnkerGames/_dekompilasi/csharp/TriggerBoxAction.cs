using System;
using UnityEngine;

public class TriggerBoxAction : MonoBehaviour
{
	public Action<Collider> OnTriggerAction;

	public LayerMask triggeredLayer;

	protected void SendOnTriggerAction(Collider other)
	{
		OnTriggerAction?.Invoke(other);
	}

	protected bool IsCollisionOnCorrectLayer(Collider collision)
	{
		if (((int)triggeredLayer & (1 << collision.gameObject.layer)) == 1 << collision.gameObject.layer)
		{
			return true;
		}
		return false;
	}
}
