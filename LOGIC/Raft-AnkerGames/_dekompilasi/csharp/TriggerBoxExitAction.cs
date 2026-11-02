using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerBoxExitAction : TriggerBoxAction
{
	public void OnTriggerExit(Collider other)
	{
		if (IsCollisionOnCorrectLayer(other))
		{
			SendOnTriggerAction(other);
		}
	}
}
