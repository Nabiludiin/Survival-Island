using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerBoxEnterAction : TriggerBoxAction
{
	public void OnTriggerEnter(Collider other)
	{
		if (IsCollisionOnCorrectLayer(other))
		{
			SendOnTriggerAction(other);
		}
	}
}
