using UnityEngine;

public class QuestInteractable_Throwable : QuestInteractable
{
	public LayerMask acceptableCollisionLayers;

	private void OnCollisionEnter(Collision collision)
	{
		if (Helper.ObjectIsOnLayer(collision.gameObject, acceptableCollisionLayers))
		{
			SendOnInteractNetwork(null);
		}
	}
}
